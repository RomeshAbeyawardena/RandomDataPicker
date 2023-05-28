<script setup lang="ts">
    import "../scss/entry-card.scss";
    import { Entry } from "../models/entry";
    import { PersistedEntry } from "../models/persistedEntry";
    import { DateHelper } from "../helpers/dateHelpers";

    const props = defineProps({
        entryCard:Entry,
        persistedEntry:PersistedEntry,
        showEmail:Boolean
    });

    function formatDate(date:Date | undefined):string {
        return DateHelper.format(date, "DD/MM/YYYY HH:mm");
    }
</script>
<template>
    <div class="entry-card" v-if="props.entryCard != undefined">
        <h2 v-if="props.entryCard?.isFlagged">🏅 You Won! 🏅</h2>
        <h3>{{ props.entryCard?.name }}</h3>
        <p v-if="props.showEmail">{{ props.entryCard?.email }}</p>
        <p class="mb-2">{{ props.entryCard?.city }}</p>
    </div>
    <div class="entry-card" v-if="props.persistedEntry != undefined">
        <h2 v-if="props.persistedEntry?.isFlagged">🏅 You Won! 🏅</h2>
        <h3>{{ props.persistedEntry?.name }}</h3>
        <p v-if="props.showEmail">{{ props.entryCard?.email }}</p>
        <p>{{ props.persistedEntry?.city }}</p>
        <p class="mb-2">{{ formatDate(props.persistedEntry.created) }}</p>
    </div>
</template>