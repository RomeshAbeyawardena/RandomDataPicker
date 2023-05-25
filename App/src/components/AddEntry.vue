<script setup lang="ts">
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Button from "primevue/button";
import { Entry } from "../models/entry";
import { ref, computed } from "vue";
import { createEntryStore } from '../stores/entry';

const numberOfEntries = ref(150);
const entry = ref(new Entry());

const isValid = computed(() => {
    return entry.value != undefined
        && entry.value.city != undefined
        && entry.value.city.length > 5
        && entry.value.email != undefined
        && entry.value.email?.length > 5
        && entry.value.name != undefined
        && entry.value.name?.length > 5
        && numberOfEntries.value > 0;
});


const store = createEntryStore();
async function injectEntries() {
    await store.injectEntry(entry.value, numberOfEntries.value);
}

</script>
<template>
    <div class="card flex flex-wrap justify-content-center gap-3">
        <div>
            <span class="p-input-icon-left">
                <i class="pi pi-user" />
                <InputText v-model="entry.name" placeholder="Name" />
            </span>
        </div>
        <div>
            <span class="p-input-icon-left">
                <i class="pi pi-envelope" />
                <InputText v-model="entry.email" placeholder="Email address" />
            </span>
        </div>
        <div>
            <span class="p-input-icon-left">
                <i class="pi pi-map-marker" />
                <InputText v-model="entry.city" placeholder="City" />
            </span>
        </div>
        <div>
            <span class="p-input-icon-left">
                <i class="pi pi-search" />
                <InputNumber v-model="numberOfEntries" placeholder="Number of entries" />
            </span>
        </div>
        <div>
            <Button @click="injectEntries" :disabled="!isValid">Inject entry</Button>
        </div>
    </div>
</template>
<style>
span.p-input-icon-left>svg:first-of-type,
.p-input-icon-left>i:first-of-type {
    left: 1.75rem;
    position: relative;
}

span.p-input-icon-left>.p-inputnumber>input.p-inputnumber-input {
    padding-left: 2.5rem;
}
</style>