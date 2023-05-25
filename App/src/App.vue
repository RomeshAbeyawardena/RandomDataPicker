<script setup lang="ts">
import { createEntryStore } from "./stores/entry";
import { storeToRefs } from "pinia";
import EntryCard from "./components/EntryCard.vue";
import AddEntry from "./components/AddEntry.vue";
import Status from "./components/Status.vue";
import { onMounted } from "vue";
import { Entry, IEntry } from "./models/entry";
const store = createEntryStore();
const { winningEntries, status } = storeToRefs(store);

onMounted(async() => {
  await store.initialise();
})

async function pickWinners() :Promise<void>{
  await store.getWinners(5);
}

function getEntry(entry: IEntry) : Entry {
    return Entry.convert(entry);
}

</script>

<template>
  <Status />
  <AddEntry v-if="status?.isPopulated" />
  <div v-if="status?.isLoaded" v-for="entry in winningEntries">
    <EntryCard :entry-card="getEntry(entry)" />
  </div><br />
  <button v-if="status?.isLoaded" @click="pickWinners">Pick winners</button>
</template>
