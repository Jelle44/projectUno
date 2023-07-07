# ProjectUno

## Name
Project Uno (card game).

## Description
The aim of this project is for me to learn how to use the following:
    - C#;
    - Vue.js (using TypeScript);
    - SignalR (in order to use websockets, ergo: showing changes on one screen on multiple screens at the same time);
    - Eventually, the entity framework might be added to this project as well;
    
These techniques will all be integrated in this card game, meaning programming the card game to actually work according to the official rules, is only a secondary aim of this project.

## Installation
You'll have to install dotnet, node and posibly vue.js (for editing). When installing vue, also add the typescript extension.

To start the project:
    - open command line in the vue folder and enter the command: npm run serve;
    - open the command line in the webapi folder and enter the command: dotnet run;

## Roadmap
The roadmap of this project, in a MoSCoW format:

Must have:
    - a starting hand containing 7 cards;
    - players must be able to see the cards in their hands;
    - a player must be allowed to play cards;
    - the game should end when there is a winner;

Should have:
    - the game should have a deck, preferably with a finite amount of cards (randomised);
    - a player should be allowed to draw a card;
    - a player should be able to see the discard pile;
    - it would be nice if the cards with special effects do what the rules say they do;
    - a player would like to see the number of cards in other players' hands;
    - a winscreen at the end of the game, to show the game is over;

Could have:
    - entity framework + database to save the game halfway through;
    - would be nice if the game checks which cards are allowed to be played, and which are not;
    - would be nice if the game checks which player is allowed to play a card;
    - a button to press when a player only has one card remaining in his hand;
    - it would be nice if more than two players could play the game;

Would have:
    - it would be nice to automatically reshuffle the deck when there are no cards left;
    - it would be nice to be able to drag and drop the cards;
    - it would be nice to see the cardback of the cards in your opponents' hands, instead of just a counter;
    - it would be nice to add visual effects to drawing/dealing cards;
    - it would be nice to be able to toggle custom rules;

## Contributing
For now, this is an individual project, so no contributions will be accepted.

## Project status
If you have run out of energy or time for your project, put a note at the top of the README saying that development has slowed down or stopped completely. Someone may choose to fork your project or volunteer to step in as a maintainer or owner, allowing your project to keep going. You can also make an explicit request for maintainers.
