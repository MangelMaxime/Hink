import fable from 'rollup-plugin-fable';
import livereload from 'rollup-plugin-livereload';
import serve from 'rollup-plugin-serve';
import cjs from 'rollup-plugin-commonjs';
import nodeResolve from 'rollup-plugin-node-resolve';


const isProduction = process.env.BUID == "production";

export default {
    entry: './Showcase.fsproj',
    dest: './public/dist/js/bundle.js',
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
            include: './node_modules/**',
            namedExports: {
            }
        }),
        nodeResolve({ jsnext: true, main: true, browser: true })
    ],
    external: ['stats.js'],
    globals: {
        'stats.js': 'Stats'
    },
    format: 'iife',
    moduleName: 'hinkShowcase'
};
