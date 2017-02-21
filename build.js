const path = require('path');
const fs = require('fs-extra');
const fable = require('fable-compiler');


const PKG_JSON = "package.json"
const README = "README.md"
const RELEASE_NOTES = "RELEASE_NOTES.md"
const OUT_DIR = "npm"

const targets = {
  clean() {
    return fable.promisify(fs.remove, OUT_DIR)
  },
  build() {
    return this.clean()
      .then(_ => fable.compile({ projFile: "./" }))
  },
  dev() {
    return this.clean()
      .then(_ => fable.compile({ projFile: "./", watch: true }))
  }
}

// As with FAKE scripts, run a default target if no one is specified
targets[process.argv[2] || "build"]().catch(err => {
  console.log(err);
  process.exit(-1);
});
