<!-- OUTDATED, JUST AN EXAMPLE OF SIGNALR
    
    <template>
    <button @click="send()">Reset</button>
</template>

<script lang="ts">
    import * as signalR from "@microsoft/signalr";

    let connection: signalR.HubConnection | undefined = undefined;


    let Pile: {
        owner: null,
        activeColour: null,
        activeValue: null,
        isPlayed: boolean,
        path: string
    }

   
    export default {
        data() {
            return {
                connection: undefined,
                Pile: {
                    owner: null,
                    activeColour: null,
                    activeValue: null,
                    isPlayed: true,
                    path: "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/fed3bb24-454f-4bdf-a721-6aa8f23e7cef/d9gnihf-ec16caeb-ec9c-4870-9480-57c7711d844f.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2ZlZDNiYjI0LTQ1NGYtNGJkZi1hNzIxLTZhYThmMjNlN2NlZlwvZDlnbmloZi1lYzE2Y2FlYi1lYzljLTQ4NzAtOTQ4MC01N2M3NzExZDg0NGYucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.kp5EommPFQl1sDPPtC-p8JloXDTm3CyNUgoievwh8Kw"
                }
            }
        },
        mounted() {
            connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7281/hub", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            }).build();
            connection.start();
        },
        //TODO function below resets the deck, refactor to different function
        methods: {
            send: function () {
                connection!.send("drawCard", 108, Pile);
            }
        }
    };
</script>-->