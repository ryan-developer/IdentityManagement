<template>
    <v-container fluid>
        <v-toolbar elevation="1">
            <v-toolbar-title>Account Profile</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn :to="{ name: 'sign-out' }" icon>
                <v-icon>mdi-logout-variant</v-icon>
            </v-btn>
        </v-toolbar>
        <v-row>
            <v-col>
                <v-card elevation="1">
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { mapState } from 'vuex';

    @Component({
        components: {},
        computed: {
            ...mapState('auth', { userAuth: state => (state as any).userAuthentication })
        }
    })
    export default class ProfilePage extends Vue {
        public IsInitialized: boolean = false;
        protected async created() {
            this.IsInitialized = true;
        }
    }
</script>