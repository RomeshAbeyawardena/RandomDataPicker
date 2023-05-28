<script setup lang="ts">
import { createEntryStore } from "./stores/entry";
import { storeToRefs } from "pinia";
import EntryList from "./components/EntryList.vue";
import AddEntry from "./components/AddEntry.vue";
import Status from "./components/Status.vue";
import { onMounted, ref } from "vue";
import { Entry, IEntry } from "./models/entry";
import { IPersistedEntry, PersistedEntry } from "./models/persistedEntry";
import Button from "primevue/button";
const store = createEntryStore();
const { winningEntries, status } = storeToRefs(store);
const historicWinningEntries = ref(new Array<IPersistedEntry>());
onMounted(async () => {
  await store.initialise();
})

async function pickWinners(): Promise<void> {
  await store.getWinners(5);
}

async function getHistoricWinners() {
  historicWinningEntries.value = await store.getWinnerHistory(25);
}

function getEntry(entry: IEntry): Entry {
  return Entry.convert(entry);
}

function getPersistedEntry(entry: IPersistedEntry): PersistedEntry {
  return PersistedEntry.convert(entry);
}

</script>

<template>
  <Status />
  <AddEntry v-if="status?.isPopulated" />
  <div>
    <Button class="mr-2" v-if="status?.isLoaded" @click="pickWinners" label="Pick winners" />
    <Button @click="getHistoricWinners" label="View historic entries" />
  </div>
  <div class="grid">
    <div class="col">
      <h2>Winners</h2>
      <EntryList :entries="winningEntries.map(getEntry)" />
    </div>
    <div class="col">
      <h2>Previous winners</h2>

      <EntryList :persisted-entries="historicWinningEntries.map(getPersistedEntry)" />
    </div>
  </div>
</template>
