async function fetchCallPost(link: string, counter: number, pile: any){
    fetch('https://localhost:7281/counter', {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },

        body: JSON.stringify({
            counter: counter,
            pressingPlayer: pile.owner//.nextPlayer
        })
    })
        .then(response => response.json());
}