<template>
    <v-navigation-drawer clipped app v-if="userAuth && userAuth.isAuthenticated">
        <template v-slot:prepend>
            <v-list>
                <v-list-item link :to="{ name: 'account-profile' }" exact>
                    <v-list-item-avatar>
                        <v-img :src="userAuth.gravatarUrl"></v-img>
                    </v-list-item-avatar>
                    <v-list-item-content twoline>
                        <v-list-item-title>
                            {{ userAuth.firstName }} {{ userAuth.lastName }}
                        </v-list-item-title>
                        <v-list-item-subtitle>
                            {{ userAuth.emailAddress }}
                        </v-list-item-subtitle>
                    </v-list-item-content>
                </v-list-item>
            </v-list>
        </template>
        
        <v-divider></v-divider>
        <v-list nav>
            <v-list-item link :to="{ name: 'tenants' }" exact>
                <v-list-item-avatar>
                    <v-icon>mdi-office-building</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                    <v-list-item-title>Tenants</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
            <v-list-item link :to="{ name: 'users' }" exact>
                <v-list-item-avatar>
                    <v-icon>mdi-account-group</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                    <v-list-item-title>Users</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
            <v-list-item link :to="{ name: 'roles' }" exact>
                <v-list-item-avatar>
                    <v-icon>mdi-shield-account</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                    <v-list-item-title>Roles</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
            <v-list-item link :to="{ name: 'applications' }" exact>
                <v-list-item-avatar>
                    <v-icon>mdi-apps</v-icon>
                </v-list-item-avatar>
                <v-list-item-content>
                    <v-list-item-title>Applications</v-list-item-title>
                </v-list-item-content>
            </v-list-item>
        </v-list>
        <v-divider></v-divider>
    </v-navigation-drawer>
</template>
<script lang="ts">
    import UserAuthResponse from '@/HttpClients/Auth/UserAuthResponse';
    import { Component, Vue } from 'vue-property-decorator';
    import { mapState } from 'vuex';

    @Component({
        name: 'left-admin-navigation',
        components: {},
        computed: {
            ...mapState('auth', { userAuth: state => (state as any).userAuthentication })
        }
    })
    export default class LeftAdminNavigation extends Vue {
        userAuth!: UserAuthResponse

        SignOut() {
            window.location.replace('/account/sign-out');
        }
    }
</script>
<style lang="scss">
</style>
