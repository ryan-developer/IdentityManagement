<template>
    <v-container fluid>
        <v-toolbar elevation="1">
            <v-toolbar-title>Tenant Management</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-tooltip bottom>
                <template v-slot:activator="{ on }">
                    <v-btn icon v-on="on">
                        <v-icon>mdi-plus</v-icon>
                    </v-btn>
                </template>
                <span>Add Tenant</span>
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
                                  :sort-by="['id', 'name']">
                        <template v-slot:item.action="{ item }">
                            <v-tooltip bottom>
                                <span>Edit</span>
                            </v-tooltip>
                        </template>
                    </v-data-table>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import RoleResponse from '@/HttpClients/Roles/Contracts/RoleResponse';
    import PaginationList from '@/HttpClients/Shared/PaginationList';
    import HttpClient from '@/HttpClients/Applications/ApplicationsClient';
    @Component
    export default class ApplicationsPage extends Vue {
        public ItemList: PaginationList<RoleResponse> = null;
        public IsInitialized: boolean = false;
        public TableHeaders: Array<any>;

        constructor() {
            super();
            this.TableHeaders = [
                { text: 'Id', value: 'id', align: 'left' },
                { text: 'Name', value: 'name', sortable: false },
            ];
        }
        async mounted() {
            let response = await HttpClient.GetAll();
            if (response && response.status === 200) {
                this.ItemList = response.data;
            }
        }

        protected async created() {
            this.IsInitialized = true;
        }
    }
</script>