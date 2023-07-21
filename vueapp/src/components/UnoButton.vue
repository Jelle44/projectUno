<template>
    <div class="UnoButtonDiv">
        <div class="UnoButtonDiv">
            <button class="UnoButton"
                    @click.prevent="signalLastCard()">
                Uno!
            </button>
        </div>
        <div v-if="isActive" class="UnoMessage">
            The Uno-button has been pressed!
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    const unoSound = require("@/assets/livin_a_lie_livin_a_lie_timmah.mp3");

    export default defineComponent({
        props: ['isActive', 'game'],
        methods: {
            signalLastCard: function () {
                fetch('https://localhost:7281/counter/SignalUno', {
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
                    .then(updatedGameState => {
                        this.$emit('clicked', updatedGameState);
                    })
                    .then(() => {
                        var sound = new Audio(unoSound);
                        sound.play();
                    });
            },
        }
    })
</script>

<style scoped>
    .UnoButtonDiv {
        margin: 0 auto;
    }

    .UnoButton {
        text-align: center;
        width: 17rem;
        height: auto;
        font-weight: 800;
        animation: colorRotate 6s linear 0s infinite;
        border-radius: 1rem;
    }

    .UnoMessage {
        text-align: center;
    }

    @keyframes colorRotate {
        from {
            color: #6666ff;
        }

        10% {
            color: #0099ff;
        }

        50% {
            color: #00ff00;
        }

        75% {
            color: #ff3399;
        }

        100% {
            color: #6666ff;
        }
    }
</style>