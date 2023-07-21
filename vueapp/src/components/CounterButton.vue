<template>
    <div v-if="!game.isGameOver">
        <form class="d-flex">
            <deck-component :game="game"
                            @clicked="onClickDeckComponent">
            </deck-component>
            <uno-button :game="game"
                        :isActive="isActive"
                        @clicked="onClickUnoButton">
            </uno-button>
            <pile-component :game="game">
            </pile-component>
        </form>

        <info-component
            :game="game">
        </info-component>

        <hand-component
            :game="game"
            @clicked="onClickHandComponent">
        </hand-component>
    </div>

    <div v-if="game.isGameOver" class="GameOver" readonly>
        The game has ended.
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    import * as signalR from "@microsoft/signalr";
    import UnoButton from "./UnoButton.vue";
    import DeckComponent from "./DeckComponent.vue";
    import InfoComponent from "./InfoComponent.vue";
    import PileComponent from "./PileComponent.vue";
    import HandComponent from "./HandComponent.vue";

    let connection: signalR.HubConnection | undefined = undefined;
    // eslint-disable-next-line
    let isActive: boolean;

    interface Card {
        name:string,
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
        handSizePlayerOne: number,
        handSizePlayerTwo: number,
        counter: number;
        pileTopCard: Card;
        player: Player[],
        isGameOver: boolean
    }

    export default defineComponent({
        components: {
            UnoButton,
            DeckComponent,
            InfoComponent,
            PileComponent,
            HandComponent
        },
        data() {
            return {
                game: {
                    counter: 108,
                    pileTopCard: {
                        path: "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/fed3bb24-454f-4bdf-a721-6aa8f23e7cef/d9gnihf-ec16caeb-ec9c-4870-9480-57c7711d844f.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2ZlZDNiYjI0LTQ1NGYtNGJkZi1hNzIxLTZhYThmMjNlN2NlZlwvZDlnbmloZi1lYzE2Y2FlYi1lYzljLTQ4NzAtOTQ4MC01N2M3NzExZDg0NGYucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.kp5EommPFQl1sDPPtC-p8JloXDTm3CyNUgoievwh8Kw"
                    },
                    player: [{}],
                    isGameOver: false
                } as Data,
                isActive: false
            }
        },
        mounted() {
            connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7281/hub", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            }).build();
            connection.start();

            connection.on("requestReceived", (updatedPile: Card, newNumCards: number, p1HandSize: number, p2HandSize: number, isGameOver: boolean) => {
                this.game.pileTopCard = updatedPile;
                this.game.counter = newNumCards;
                this.game.handSizePlayerOne = p1HandSize;
                this.game.handSizePlayerTwo = p2HandSize;
                this.game.isGameOver = isGameOver;
                this.reloadHand();
            });

            connection.on("unoButtonWasPressed", () => {
                this.isActive = true;
            })

            window.addEventListener("load", () => {
                    fetch('https://localhost:7281/counter/NewGame', {
                        method: "get",
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(response => response.json())
                        .then(game => this.game = game)
            })
        },
        methods: {
            send: function () {
                connection!.send("UpdateBoard", this.game.pileTopCard, this.game.counter, this.game.handSizePlayerOne, this.game.handSizePlayerTwo, this.game.isGameOver);
            },
            onClickUnoButton: function (value: Data) {
                this.game = value;
                this.isActive = true;
                this.send();
                this.sendUno();
            },
            onClickDeckComponent: function (value: Data) {
                this.game = value;
                this.isActive = false;
                this.send();
            },
            onClickHandComponent: function (value: Data) {
                this.game = value;
                this.send();
                this.isActive = false;
            },
            sendUno: function () {
                connection!.send("UnoButtonPressed");
            },
            reloadHand: function () {
                fetch('https://localhost:7281/counter/ReloadHand', {
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
                        this.game.player[0].hand = response.hand;
                    })
            }
        }
    })
</script>

<style scoped>
    .GameOver[readonly] {
        text-align: center;
        color: forestgreen;
        background-color: orangered;
        font-size: 50px;
    }
</style>
