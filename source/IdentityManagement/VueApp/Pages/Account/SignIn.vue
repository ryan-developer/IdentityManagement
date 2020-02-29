<template>
    <v-container fluid>
        <v-row>
            <v-col>
                <h1>Sign In</h1>
                <v-btn to="/account/sign-in/openid/aad">
                    Sign In To Microsoft
                </v-btn>
            </v-col>
        </v-row>

    </v-container>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    @Component
    export default class SignInPage extends Vue {
        public IsInitialized: boolean = false;
        protected async created() {
            this.IsInitialized = true;
        }
    }
</script>