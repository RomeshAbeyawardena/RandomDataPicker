<script setup lang="ts">
    import Toolbar from 'primevue/toolbar';
    import Button from 'primevue/button';
    import { createEntryStore } from '../stores/entry';
    import { storeToRefs } from 'pinia';
    const store = createEntryStore();
    const { status } = storeToRefs(store);

    function getIcon(bool:boolean | undefined) {
        if(bool != undefined && bool){
            return "pi pi-check";
        }
        else return "pi pi-times";
    }
</script>
<template>
    <Toolbar class="mb-2">
        <template #start>
            <Button class="mr-2" badge-class="p-badge-success" :badge="status?.totalNumberOfEntries?.toString()" type="button" label="Entries" />
            <Button class="mr-2" iconPos="right" :icon="getIcon(status?.isLoaded)"  type="button" label="Loaded" />
            <Button iconPos="right" :icon="getIcon(status?.isPopulated)" type="button" label="Populated" />
        </template>
        <template #end>
            <h3>Random entry draw generator</h3>
        </template>
    </Toolbar>
</template>
<style>
    .p-badge {
        border-radius: 1rem;
        padding: 0.4rem;
    }
</style>