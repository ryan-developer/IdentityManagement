<template>
    <v-container fluid>
        <v-toolbar elevation="1">
            <v-toolbar-title>User Management</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-tooltip bottom>
                <template v-slot:activator="{ on }">
                    <v-btn icon v-on="on">
                        <v-icon>mdi-plus</v-icon>
                    </v-btn>
                </template>
                <span>Add User</span>
            </v-tooltip>

            <v-btn icon>
                <v-icon>mdi-magnify</v-icon>
            </v-btn>
        </v-toolbar>
        <v-row>
            <v-col>
                <v-card elevation="1">
                    <v-data-table v-if="ItemList" :items="ItemList.items"
                                  :headers="TableHeaders"
                                  :sort-by="['tenantID', 'email']"
                                  :search="Search">
                        <template v-slot:item.action="{ item }">
                            <v-tooltip bottom>
                                <template v-slot:activator="{ on }">
                                    <v-btn text icon v-on="on" v-on:click="Edit(item)">
                                        <v-icon small>
                                            mdi-pencil
                                        </v-icon>
                                    </v-btn>
                                </template>
                                <span>Edit</span>
                            </v-tooltip>
                        </template>
                    </v-data-table>
                </v-card>
            </v-col>
        </v-row>
        <v-dialog v-model="ShowDialog" fullscreen hide-overlay transition="dialog-bottom-transition">
            <v-card>
                <v-toolbar dark color="primary">
                    <v-toolbar-title>Edit User</v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-toolbar-items>
                        <v-btn dark text>Save</v-btn>
                        <v-btn dark text @click="CloseEditor">Cancel</v-btn>
                    </v-toolbar-items>
                </v-toolbar>
                <v-container fluid v-if="User">
                    <v-row align="stretch">
                        <v-col>
                            <h3>Profile</h3>
                            {{ User.email }}
                        </v-col>
                        <v-col>asdf</v-col>
                    </v-row>
                </v-container>
            </v-card>
        </v-dialog>
    </v-container>

</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import HttpClient from '@/HttpClients/Users/UsersClient';
    import UserIdentity from '@/HttpClients/Users/Contracts/UserIdentity';
    import UserResponse from '@/HttpClients/Users/Contracts/UserResponse';
    import PaginationList from '@/HttpClients/Shared/PaginationList';

    @Component
    export default class UsersPage extends Vue {
        public IsInitialized: boolean = false;
        public ItemList: PaginationList<UserResponse> = null;
        public User: UserResponse = null;

        public TableHeaders: Array<any>;
        public Search: string = '';
        public ShowDialog: boolean = false;

        constructor() {
            super();
            this.TableHeaders = [
                { text: 'User Id', value: 'id', align: 'left' },
                { text: 'Email Address', value: 'email' },
                { text: 'First Name', value: 'givenName' },
                { text: 'Last Name', value: 'familyName' },
                { text: 'Provider', value: 'identityProvider' },
                { text: 'Tenant', value: 'tenantID' },
                { text: 'Actions', value: 'action', sortable: false },
            ];
        }

        protected async created() {
            this.IsInitialized = true;
        }

        async mounted() {
            let response = await HttpClient.GetAllUsers();
            if (response && response.status === 200) {
                this.ItemList = response.data;
            }
        }
        async Edit(user: UserIdentity) {
            console.dir(user);
            var response = await HttpClient.GetUser(user.id);
            if (response && response.status === 200) {
                this.User = response.data;
            }
            this.ShowDialog = true;
        }

        async CloseEditor() {
            this.User = null;
            this.ShowDialog = false;
        }
    }
</script>