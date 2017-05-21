var path = require("path");
var webpack = require("webpack");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

var babelOptions = {
  presets: [["es2015", { "modules": false }]],
  plugins: ["transform-runtime"]
}

var isProduction = process.argv.indexOf("-p") >= 0;
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

module.exports = {
  devtool: "source-map",
  entry: resolve('./Showcase.fsproj'),
  output: {
    filename: 'bundle.js',
    path: resolve('./public'),
  },
    resolve: {
    modules: [
      "node_modules", resolve("./node_modules/"), resolve("../packages")
    ]
  },
  devServer: {
    contentBase: resolve('./public'),
    port: 8080
  },
  externals: {
    "PIXI": "PIXI"
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: {
            babel: babelOptions,
            define: isProduction ? [] : ["DEBUG"]
          }
        }
      },
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: babelOptions
        },
      }
    ]
  },
  resolve: {
    symlinks: false
  }
};
