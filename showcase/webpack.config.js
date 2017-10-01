var path = require("path");
var webpack = require("webpack");
var fableUtils = require("fable-utils");
var htmlWebpackPlugin = require('html-webpack-plugin');

function resolve(filePath) {
    return path.join(__dirname, filePath)
}

var babelOptions = fableUtils.resolveBabelOptions({
    presets: [["env", { "modules": false }]],
    plugins: ["transform-runtime"]
});

var isProduction = process.argv.indexOf("-p") >= 0;
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

module.exports = {
    devtool: "source-map",
    entry: resolve('./Showcase.fsproj'),
    output: {
        filename: 'dist/js/bundle.js',
        path: resolve('./dist'),
    },
    resolve: {
        modules: [resolve("./node_modules/")]
    },
    devServer: {
        contentBase: resolve('./dist'),
        port: 8081
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: babelOptions,
                        define: isProduction ? [] : ["DEBUG"],
                        extra: { optimizeWatch: true }
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
    plugins: [
        new htmlWebpackPlugin({
            template: resolve("./src/index.html"),
            hash: true
        })
    ]
};
