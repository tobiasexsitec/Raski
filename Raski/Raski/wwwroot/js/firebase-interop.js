// Single JS surface towards Firebase. Loaded lazily as an ES module from C#.
// Firebase JS SDK v10 (modular) is pulled from gstatic so no npm/build step is required.

import { initializeApp } from "https://www.gstatic.com/firebasejs/10.12.5/firebase-app.js";
import {
    getAuth,
    GoogleAuthProvider,
    signInWithPopup,
    signInWithRedirect,
    getRedirectResult,
    signInWithCredential,
    onAuthStateChanged,
    signOut as fbSignOut
} from "https://www.gstatic.com/firebasejs/10.12.5/firebase-auth.js";
import {
    getFirestore,
    collection,
    doc,
    getDoc,
    getDocs,
    setDoc,
    addDoc,
    updateDoc,
    deleteDoc,
    onSnapshot,
    query,
    where,
    orderBy,
    limit as fbLimit,
    startAt,
    endAt,
    arrayUnion,
    arrayRemove,
    writeBatch
} from "https://www.gstatic.com/firebasejs/10.12.5/firebase-firestore.js";

let app = null;
let auth = null;
let db = null;

// Unsubscribe callbacks keyed by a handle string handed back to C#.
const subscriptions = new Map();
let subscriptionCounter = 0;

export function initialize(config) {
    if (app) {
        return;
    }

    app = initializeApp(config);
    auth = getAuth(app);
    db = getFirestore(app);
}

function requireDb() {
    if (!db) {
        throw new Error("Firebase har inte initierats.");
    }

    return db;
}

/* ---------------------------------------------------------------- auth ---- */

function mapUser(user) {
    if (!user) {
        return null;
    }

    return {
        uid: user.uid,
        displayName: user.displayName ?? "",
        email: user.email ?? "",
        photoUrl: user.photoURL ?? ""
    };
}

export async function signInWithGoogle() {
    const provider = new GoogleAuthProvider();
    provider.setCustomParameters({ prompt: "select_account" });

    try {
        const result = await signInWithPopup(auth, provider);
        return mapUser(result.user);
    } catch (error) {
        // Popups are blocked in several mobile browsers and in-app webviews.
        const fallbackCodes = [
            "auth/popup-blocked",
            "auth/popup-closed-by-user",
            "auth/cancelled-popup-request",
            "auth/operation-not-supported-in-this-environment"
        ];

        if (fallbackCodes.includes(error?.code)) {
            await signInWithRedirect(auth, provider);
            return null;
        }

        throw error;
    }
}

// Google Identity Services signs in directly against accounts.google.com and hands back
// an ID token. Unlike popup/redirect via authDomain this needs no third-party storage,
// which WebKit (all iOS browsers) blocks.
let gisLoader = null;

function loadGoogleIdentityServices() {
    if (window.google?.accounts?.id) {
        return Promise.resolve();
    }

    gisLoader ??= new Promise((resolve, reject) => {
        const script = document.createElement("script");
        script.src = "https://accounts.google.com/gsi/client";
        script.async = true;
        script.onload = () => resolve();
        script.onerror = () => {
            gisLoader = null;
            reject(new Error("Kunde inte ladda Google-inloggningen."));
        };
        document.head.appendChild(script);
    });

    return gisLoader;
}

export async function renderGoogleButton(element, clientId, dotNetRef) {
    await loadGoogleIdentityServices();

    window.google.accounts.id.initialize({
        client_id: clientId,
        ux_mode: "popup",
        callback: async response => {
            try {
                const credential = GoogleAuthProvider.credential(response.credential);
                await signInWithCredential(auth, credential);
            } catch (error) {
                dotNetRef.invokeMethodAsync("OnGoogleSignInFailed", error?.code ?? error?.message ?? "");
            }
        }
    });

    const width = Math.min(Math.max(element.clientWidth || 280, 200), 400);

    window.google.accounts.id.renderButton(element, {
        type: "standard",
        theme: "outline",
        size: "large",
        text: "continue_with",
        shape: "pill",
        locale: "sv",
        width
    });
}

export async function completeRedirectSignIn() {
    const result = await getRedirectResult(auth);
    return mapUser(result?.user);
}

export function signOut() {
    window.google?.accounts?.id?.disableAutoSelect();
    return fbSignOut(auth);
}

export function currentUser() {
    return mapUser(auth?.currentUser);
}

// Pushes auth state changes into C#. Returns a handle used to unsubscribe.
export function listenToAuthState(dotNetRef, methodName) {
    const handle = `auth-${++subscriptionCounter}`;
    const unsubscribe = onAuthStateChanged(auth, user => {
        dotNetRef.invokeMethodAsync(methodName, mapUser(user));
    });

    subscriptions.set(handle, unsubscribe);
    return handle;
}

/* ----------------------------------------------------------- firestore ---- */

function withId(snapshot) {
    return { id: snapshot.id, ...snapshot.data() };
}

export async function getDocument(path) {
    const snapshot = await getDoc(doc(requireDb(), path));
    return snapshot.exists() ? withId(snapshot) : null;
}

export async function setDocument(path, data, merge) {
    await setDoc(doc(requireDb(), path), data, { merge: merge === true });
}

export async function addDocument(collectionPath, data) {
    const reference = await addDoc(collection(requireDb(), collectionPath), data);
    return reference.id;
}

export async function updateDocument(path, data) {
    await updateDoc(doc(requireDb(), path), data);
}

export async function deleteDocument(path) {
    await deleteDoc(doc(requireDb(), path));
}

// Atomically adds a value to an array field without rewriting the document.
export async function addToArray(path, field, value) {
    await updateDoc(doc(requireDb(), path), { [field]: arrayUnion(value) });
}

// Atomically removes a value from an array field.
export async function removeFromArray(path, field, value) {
    await updateDoc(doc(requireDb(), path), { [field]: arrayRemove(value) });
}

// Writes several documents atomically. Each operation is { path, data, merge }.
export async function batchSet(operations) {
    const batch = writeBatch(requireDb());

    for (const operation of operations) {
        batch.set(doc(requireDb(), operation.path), operation.data, { merge: operation.merge === true });
    }

    await batch.commit();
}

// Builds query constraints from a serializable description sent by C#.
function buildConstraints(spec) {
    const constraints = [];

    if (!spec) {
        return constraints;
    }

    for (const filter of spec.where ?? []) {
        constraints.push(where(filter.field, filter.op, filter.value));
    }

    if (spec.orderBy) {
        constraints.push(orderBy(spec.orderBy, spec.descending === true ? "desc" : "asc"));
    }

    if (spec.startAt !== undefined && spec.startAt !== null) {
        constraints.push(startAt(spec.startAt));
    }

    if (spec.endAt !== undefined && spec.endAt !== null) {
        constraints.push(endAt(spec.endAt));
    }

    if (spec.limit) {
        constraints.push(fbLimit(spec.limit));
    }

    return constraints;
}

export async function queryCollection(collectionPath, spec) {
    const reference = collection(requireDb(), collectionPath);
    const snapshot = await getDocs(query(reference, ...buildConstraints(spec)));
    return snapshot.docs.map(withId);
}

// Streams collection changes into C#. Returns a handle used to unsubscribe.
export function observeCollection(collectionPath, spec, dotNetRef, methodName) {
    const handle = `col-${++subscriptionCounter}`;
    const reference = collection(requireDb(), collectionPath);

    const unsubscribe = onSnapshot(
        query(reference, ...buildConstraints(spec)),
        snapshot => dotNetRef.invokeMethodAsync(methodName, snapshot.docs.map(withId)),
        error => dotNetRef.invokeMethodAsync(`${methodName}Error`, error?.message ?? "Okänt fel"));

    subscriptions.set(handle, unsubscribe);
    return handle;
}

export function unsubscribe(handle) {
    const stop = subscriptions.get(handle);

    if (stop) {
        stop();
        subscriptions.delete(handle);
    }
}
