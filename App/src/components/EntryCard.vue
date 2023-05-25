<script setup lang="ts">
    import { Entry } from "../models/entry";

    const props = defineProps({
        entryCard:Entry,
        showEmail:Boolean
    });

</script>
<template>
    <div>
        <h2 v-if="props.entryCard?.isFlagged">🏅 You Won! 🏅</h2>
        <h3>{{ props.entryCard?.name }}</h3>
        <p v-if="props.showEmail">{{ props.entryCard?.email }}</p>
        <p>{{ props.entryCard?.city }}</p>
    </div>
</template>