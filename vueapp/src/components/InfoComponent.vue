<template>
    <div class="turnPlayer">
        Last card was played by: {{ game.pileTopCard.name }}
    </div>
    <div class="turnPlayer">
        Player one's handsize: {{ game.handSizePlayerOne }}
    </div>
    <div class="turnPlayer">
        Player two's handsize: {{ game.handSizePlayerTwo }}
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        props: ['game']
    })
</script>

<style scoped>
    .turnPlayer {
        text-align: center;
    }
</style>