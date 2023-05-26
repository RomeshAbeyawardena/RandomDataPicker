<script setup lang="ts">
    import { Entry } from "../models/entry";
    import { PersistedEntry } from "../models/persistedEntry";
    import EntryCard from "./EntryCard.vue";
    const props = defineProps({
        entries:Array<Entry>,
        persistedEntries:Array<PersistedEntry>
    });
    
</script>
<template>
    <div v-for="entry in props.entries">
        <EntryCard :entry-card="entry" />
    </div>
    <div v-for="entry in props.persistedEntries">
        <EntryCard :persisted-entry="entry" />
    </div>
</template>