<template>
    <div class="pileTopCard">
        <img class="pileImage" :src="game.pileTopCard.path" />
        <div class="pileColour">Allowed colours: {{ game.pileTopCard.activeColour }}</div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        props: ['game']
    })
</script>

<style scoped>
    .pileTopCard {
        margin: 0 auto;
    }

    .pileImage {
        width: 15rem;
        height: auto;
        border: 1px solid black;
        border-radius: 1rem;
    }

    .pileColour {
        text-align: center;
    }
</style>