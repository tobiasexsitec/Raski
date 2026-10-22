// Sets lastSeenVersion on every document in users/ so existing users see release notes newer than that version.
// Usage: node set-last-seen-version.mjs [version] [--dry-run]
// Auth: Application Default Credentials (gcloud auth application-default login)
// or GOOGLE_APPLICATION_CREDENTIALS pointing to a service account key (never commit it).
import { initializeApp, applicationDefault } from "firebase-admin/app";
import { getFirestore } from "firebase-admin/firestore";

const args = process.argv.slice(2);
const dryRun = args.includes("--dry-run");
const version = args.find(a => !a.startsWith("--")) ?? "0.20.0";

initializeApp({ credential: applicationDefault(), projectId: "raski-ba97f" });
const db = getFirestore();

const snapshot = await db.collection("users").get();
console.log(`${snapshot.size} användare hittades. Sätter lastSeenVersion=${version}${dryRun ? " (dry run)" : ""}.`);

let writer = db.bulkWriter();
for (const doc of snapshot.docs) {
  console.log(`${doc.id}: ${doc.get("lastSeenVersion") ?? "(saknas)"} -> ${version}`);
  if (!dryRun) writer.update(doc.ref, { lastSeenVersion: version });
}
await writer.close();
console.log("Klart.");
