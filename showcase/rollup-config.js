import fable from 'rollup-plugin-fable';
import livereload from 'rollup-plugin-livereload';
import serve from 'rollup-plugin-serve';
import cjs from 'rollup-plugin-commonjs';
import nodeResolve from 'rollup-plugin-node-resolve';
import path from 'path';

const isProduction = process.env.BUID == "production";

function resolve(filePath) {
    return path.resolve(__dirname, filePath)
}

export default {
    input: resolve('./Showcase.fsproj'),
    output: {
        file: resolve('./public/dist/js/bundle.js'),
        format: 'iife'
    },
    plugins: [
        fable({
            define: isProduction ? [] : ["DEBUG"]
        }),
        livereload(),
        serve({
            contentBase: 'public',
            port: 8081
        }),
        cjs({
            include: resolve('./node_modules/**'),
            namedExports: {
            }
        }),
        nodeResolve({ jsnext: true, main: true, browser: true })
    ],
    external: ['stats.js'],
    globals: {
        'stats.js': 'Stats'
    },
    name: 'hinkShowcase'
};
