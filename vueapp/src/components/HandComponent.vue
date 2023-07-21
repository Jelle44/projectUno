<template>
    <form class="d-flex">
        <div v-if="game.player[0].hand" class="handDiv">
            <img v-for="(card, index) in game.player[0].hand" :key="index"
                 class="handImage"
                 :src="game.player[0].hand[index].path"
                 @click.prevent="playCard(card)" />
        </div>
    </form>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    const playSound = require("@/assets/WindowsXPErrorSound.mp3");

    interface Card {
        name: string,
        path: string,
        activeColour: string,
        activeValue: string
    }

    export default defineComponent({
        props: ['game'],
        methods: {
            playCard: async function (card: Card) {
                let cardColour;

                if (card.activeValue == "RECOLOUR" ||
                    card.activeValue == "DRAW_FOUR") {
                    cardColour = await this.newColourPopUp();
                }
                fetch('https://localhost:7281/counter/PlayCard', {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },

                    body: JSON.stringify({
                        name: this.game.playerId,
                        value: card.activeValue,
                        colour: card.activeColour,
                        newColour: cardColour
                    })
                })
                    .then(response => response.json())
                    .then(updatedGameState => {
                        this.$emit('clicked', updatedGameState);
                    })
                    .then(() => {
                        var sound = new Audio(playSound);
                        sound.play();
                    });
            },
            newColourPopUp: async function string() {
                let colour = await window.prompt("Please enter one of the following colours: 'BLUE', 'GREEN', 'RED', 'YELLOW'.");
                if (colour?.toUpperCase() === "BLUE" ||
                    colour?.toUpperCase() === "GREEN" ||
                    colour?.toUpperCase() === "RED" ||
                    colour?.toUpperCase() === "YELLOW") {

                    return colour;
                }
                return "BLUE";
            }
        }
    })
</script>

<style scoped>
    .handDiv {
        margin: 0 auto;
    }

    .handImage {
        width: 12rem;
        height: auto;
        border: 1px solid black;
        border-radius: 1rem;
        cursor: pointer;
    }
</style>