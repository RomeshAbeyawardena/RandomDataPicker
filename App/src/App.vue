<script setup lang="ts">
import { createEntryStore } from "./stores/entry";
import { storeToRefs } from "pinia";
import EntryCard from "./components/EntryCard.vue";
import { onMounted } from "vue";
const store = createEntryStore();
const { winningEntries, status } = storeToRefs(store);

onMounted(async() => {
  await store.initialise();
})

async function pickWinners() :Promise<void>{
  await store.getWinners(5);
}

</script>

<template>
  
  <div v-if="status?.isLoaded" v-for="entry in winningEntries">
    <EntryCard :entry-card="entry" />
  </div><br />
  <button v-if="status?.isLoaded" @click="pickWinners">Pick winners</button>
</template>
