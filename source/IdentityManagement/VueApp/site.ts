import Vue from 'vue';
import Application from '@/Application.vue';
import AppState from '@/app-state';
import PageRouter from '@/page-router';
import vuetify from '@/Plugins/vuetify';

new Vue({
    store: AppState,
    router: PageRouter,
    render: (h) => h(Application),
    vuetify,
    provide: {
    }
}).$mount('#app');
