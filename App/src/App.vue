<script setup lang="ts">
import { createEntryStore } from "./stores/entry";
import { storeToRefs } from "pinia";
import EntryList from "./components/EntryList.vue";
import EntryCard from "./components/EntryCard.vue";
import AddEntry from "./components/AddEntry.vue";
import Status from "./components/Status.vue";
import { onMounted } from "vue";
import { Entry, IEntry } from "./models/entry";
import Button from "primevue/button";
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
  <EntryList :entries="winningEntries.map(getEntry)" />
  <br />
  <Button v-if="status?.isLoaded" @click="pickWinners" label="Pick winners" />
</template>
