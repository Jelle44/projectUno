<template>
    <div>
        <form class="d-flex">
            <div class="buttonDiv">
                <div class="counterButton"
                    @click.prevent="drawCard()">
                    <img src="../assets/UnoCardBack.png" />
                </div>
                <div class="deckText">Draw a card ({{ game.counter }} remaining)</div>
            </div>
            <div class="pileTopCard"
                @click.prevent="">
                <img class="pileImage" :src="game.pileTopCard.path" />
            </div>
        </form>
        <form class="d-flex">
            <div v-if="game.player[0].hand" class="handDiv">
                <img v-for="(card, index) in game.player[0].hand" :key="index" class="handImage" :src="game.player[0].hand[index].path" @click.prevent="playCard(index)" />
            </div>
        </form>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import * as signalR from "@microsoft/signalr";
    //import  *  from "./FetchcallPost.ts";

    let connection: signalR.HubConnection | undefined = undefined;

    interface Card {
        path: string,
        activeColour: string,
        activeValue: string
    }

    interface Player {
        name: string,
        hand: Card[]
    }

    interface Data {
        playerId: string,
        counter: number;
        pileTopCard: Card;
        player: Player[]
    }

    export default defineComponent({
        data() {
            return {
                game: {
                    counter: 108,
                    pileTopCard: {
                        path: "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/fed3bb24-454f-4bdf-a721-6aa8f23e7cef/d9gnihf-ec16caeb-ec9c-4870-9480-57c7711d844f.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2ZlZDNiYjI0LTQ1NGYtNGJkZi1hNzIxLTZhYThmMjNlN2NlZlwvZDlnbmloZi1lYzE2Y2FlYi1lYzljLTQ4NzAtOTQ4MC01N2M3NzExZDg0NGYucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.kp5EommPFQl1sDPPtC-p8JloXDTm3CyNUgoievwh8Kw"
                    },
                    player: [{}]
                } as Data
            }
        },
        mounted() {
            connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7281/hub", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            }).build();
            connection.start();

            connection.on("requestReceived", (newNumCards: number, updatedPile: Card) => {
                this.game.counter = newNumCards;
                this.game.pileTopCard = updatedPile;
            });


            window.addEventListener("load", () => fetch('https://localhost:7281/counter/NewGame', {
                method: "get",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            }
            ).then(response => response.json())
                .then(game => this.game = game))
        },
        methods: { 
            drawCard: function() {
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
                    //if (!this.game.player[0].hand) {
                    //    this.game.player[0].hand = []
                    //}
                    this.game = response;
                    //this.game.player[0].hand = count.player[0].hand;
                    this.send();
                });
            },
            send: function () {
                connection!.send("DrawCard", this.game.counter, this.game.pileTopCard);
            },
            playCard: function (index: number) {
                fetch('https://localhost:7281/counter/PlayCard', {
                    method: "post",
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },

                    body: JSON.stringify({
                        name: this.game.playerId,
                        cardIndex: index
                    })
                })
                .then(response => response.json())
                    .then(updatedGameState => {
                        this.game = updatedGameState;
                        this.send();
                    }
                );
            }
        }
    })
</script>

<style scoped>
    .buttonDiv {
        margin: 0 auto;
    }

    .counterButton {
        cursor: pointer;
    }

    .pileTopCard {
        margin: 0 auto;
    }

    .pileImage {
        width: 243px;
        height: auto;
        border: 2px solid black;
        border-radius: 33px;
    }

    .handDiv {
        margin: 0 auto;
    }

    .handImage {
        width: 243px;
        height: auto;
        border: 2px solid black;
        border-radius: 33px;
        cursor: pointer;
    }

    .deckText {
        text-align: center;
    }
</style>
