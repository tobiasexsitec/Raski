export default {
  extends: ['@commitlint/config-conventional'],
  rules: {
    // Commit messages are written in Swedish, so sentence-case rules would reject å/ä/ö-starting or capitalized subjects.
    'subject-case': [0],
    'body-max-line-length': [0],
    'footer-max-line-length': [0],
  },
};
