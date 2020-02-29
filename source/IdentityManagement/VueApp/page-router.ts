import Vue from 'vue';
import VueRouter, { RouteConfig, Route, Location, NavigationGuard } from 'vue-router';
import HomePage from '@/Pages/HomePage.vue';
import NotFoundPage from '@/Pages/Shared/NotFoundPage.vue';
import SignInPage from '@/Pages/Account/SignIn.vue';
import ProfilePage from '@/Pages/Account/ProfilePage.vue';
import UsersPage from '@/Pages/Security/UsersPage.vue';
import RolesPage from '@/Pages/Security/RolesPage.vue';
import TenantsPage from '@/Pages/Security/TenantsPage.vue';
import UnavailablePage from '@/Pages/Shared/UnavailablePage.vue';
import ApplicationsPage from '@/Pages/Security/ApplicationsPage.vue';

Vue.use(VueRouter);


const routes: RouteConfig[] = [
    { path: '/', name: 'home', component: HomePage, meta: { title: 'Identity Management' } },
    { path: '/account/sign-in', name: 'sign-in', component: SignInPage, meta: { title: 'Sign In', requireAnon: true, allowAnon: true } },
    { path: '/account/profile', name: 'account-profile', component: ProfilePage, meta: { title: 'Account Profile' } },
    { path: '/account/sign-in/openid/:provider', name: 'sign-in-openid' },
    { path: '/account/sign-out', name: 'sign-out' },
    { path: '/users', name: 'users', component: UsersPage, meta: { title: 'Users' } },
    { path: '/users/roles', name: 'roles', component: RolesPage, meta: { title: 'User Roles' } },
    { path: '/applications', name: 'applications', component: ApplicationsPage, meta: { title: 'Applications' } },
    { path: '/tenants', name: 'tenants', component: TenantsPage, meta: { title: 'Tenants' } },
    { path: '/error/unavailable', name: 'unavailable', component: UnavailablePage, meta: { title: 'Services Unavailable' } },
    { path: '*', name: 'notfound', component: NotFoundPage, meta: { allowAnon: true, title: 'Page not found' } },
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});
router.beforeEach(async (to: Route, from: Route, next: Function) => {
    switch (to.name) {
        case 'sign-in-openid':
        case 'sign-out':
            window.location.replace(to.fullPath);
            return;
    }

    let store = router.app.$store;
    await store.dispatch('auth/authenticate');
    let userAuth = store.getters['auth/userAuthentication'];
    let serviceUnavailable = userAuth.serviceUnavailable;
    if (serviceUnavailable) {
        next({ name: 'unavailable', replace: 'true' });
    }

    let isAuthenticated = userAuth.isAuthenticated;


    if (!to.meta.allowAnon && !isAuthenticated) {
        next({ name: 'sign-in', replace: 'true' });
    }
    else if (to.meta.requireAnon && isAuthenticated) {
        router.replace({ name: 'home' });
    }
    else {
        document.title = to.meta.title;
        next();
    }
});

export default router;