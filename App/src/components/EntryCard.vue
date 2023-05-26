<script setup lang="ts">
    import { Entry } from "../models/entry";
    import { PersistedEntry } from "../models/persistedEntry";
    const props = defineProps({
        entryCard:Entry,
        persistedEntry:PersistedEntry,
        showEmail:Boolean
    });

</script>
<template>
    <div v-if="props.entryCard != undefined">
        <h2 v-if="props.entryCard?.isFlagged">🏅 You Won! 🏅</h2>
        <h3>{{ props.entryCard?.name }}</h3>
        <p v-if="props.showEmail">{{ props.entryCard?.email }}</p>
        <p>{{ props.entryCard?.city }}</p>
    </div>
    <div v-if="props.persistedEntry != undefined">
        <h2 v-if="props.persistedEntry?.isFlagged">🏅 You Won! 🏅</h2>
        <h3>{{ props.persistedEntry?.name }}</h3>
        <p v-if="props.showEmail">{{ props.entryCard?.email }}</p>
        <p>{{ props.persistedEntry?.city }}</p>
        <p>{{ props.persistedEntry.created }}</p>
    </div>
</template>