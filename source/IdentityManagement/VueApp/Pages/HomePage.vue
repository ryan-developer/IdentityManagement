<template>
    <div class="page-container">
        <h1>Home</h1>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    @Component
    export default class HomePage extends Vue {
        public IsInitialized: boolean = false;
        protected async created() {
            this.IsInitialized = true;
        }
    }
</script>