const path = require('path');

module.exports = {
    css: { extract: false },
    filenameHashing: false,
    lintOnSave: false,
    productionSourceMap: false,
    outputDir: process.env.NODE_ENV === 'production' ? './wwwroot/dist/app' : './wwwroot/dev/app',
    configureWebpack: {
        performance: { hints: false },
        externals: {
            vue: 'Vue'
        }
        //optimization: { splitChunks: { minSize:10000, maxSize:250000 } }
    },
    chainWebpack: config => {
        config.entry('app').clear();
        config.entry('app').add('./VueApp/site.ts');
        config.resolve.alias.set('@', path.resolve(__dirname, 'VueApp'));
        config.resolve.alias.set('vue$', 'vue/dist/vue.esm.js');
        if (process.env.NODE_ENV === 'production') {
            config.output.filename('app.min.js');    
            // production
        } else {
            config.output.filename('app.js');    
            // development
        }
    }
}