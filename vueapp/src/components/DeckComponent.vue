<template>
    <div class="buttonDiv">
        <div class="DeckImage"
             @click.prevent="drawCard">
            <img src="../assets/UnoCardBack.png" />
        </div>
        <div class="deckText">Draw a card ({{ game.counter }} remaining)</div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    const drawSound = require("@/assets/MammaMiaMario.mp3");

    export default defineComponent({
        props: ['game'],
        methods: {
            drawCard: function () {
                if (this.game.isGameOver) { return }
                fetch('https://localhost:7281/counter/DrawCard', {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },

                    body: JSON.stringify({
                        name: this.game.playerId
                    })
                })
                    .then(response => response.json())
                    .then(response => {
                        this.$emit('clicked', response);
                    })
                    .then(() => {
                        var sound = new Audio(drawSound);
                        sound.play();
                    });
            }
        }
    })
</script>

<style scoped>
    .buttonDiv {
        margin: 0 auto;
    }

    .DeckImage {
        cursor: pointer;
        width: 243px;
        height: auto;
    }

    .deckText {
        text-align: center;
    }
</style>