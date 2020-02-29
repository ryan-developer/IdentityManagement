<template>
    <v-app>
        <left-admin-navigation></left-admin-navigation>
        <v-content>
            <v-container fluid>
                <router-view></router-view>
            </v-container>
        </v-content>
    </v-app>
</template>
<script lang="ts">
    import LeftAdminNavigation from '@/Components/Navigation/LeftAdminNavigation.vue';
    import UserAuthResponse from '@/HttpClients/Auth/UserAuthResponse';
    import { Component, Vue } from 'vue-property-decorator';
    import Vuetify from 'vuetify/lib';
    import { mapState } from 'vuex';

    Vue.use(Vuetify);

    @Component({
        name: 'application',
        components: {
            LeftAdminNavigation
        },
        computed: {
            ...mapState('auth', { userAuth: state => (state as any).userAuthentication })
        }
    })
    export default class Application extends Vue {
        userAuth!: UserAuthResponse
    }
</script>
<style lang="scss">
    html, body {
      padding: 0;
      margin: 0;
      font-size: 14px;
      font-family: Tahoma, sans-serif;
      letter-spacing: .07rem;
    }
</style>
