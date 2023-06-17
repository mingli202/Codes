import View from "./view.js";
import Store from "./store.js";
import { Player } from "./types";

/*

Questions to ask when you are making the javaApp
What actions can a user take in my app?

view <-> app <-> storage
view:
    -add event listener
    -handle Ui-only events
    -Manipulate the DOM
app:
    -control flow of view and logic state
    -initialize application
storage:
    -get state
    -save state
    -emit state change events

When making javaApp files, you have to be wary of global scope variables.
You can lose track of your variables a1nd reinitialize them.
So we make a variable containing all our variables for this file

Best practices when developing user interfaces:
    -Global scope and namespace
    -Stable selectors (data-* attributes)
    -Seperate logic by responsibility ("separation of concerns")

What is state?
    -Client state: state that does not persist through sessions
    -Server state: snapshot in time, database

Introduction to the MVC pattern

*/

const players: Player[] = [
    {
        id: 1,
        name: "Player 1",
        iconClass: "fa-x",
    },
    {
        id: 2,
        name: "Player 2",
        iconClass: "fa-o",
    },
];

function init() {
    const view = new View();
    const store = new Store("tic-tac-toe-live", players);

    function initView() {
        view.closeWinDialog();
        view.clearMoves();
        view.setTurnIndicator(store.game.currentPlayer);
        view.updateScore(
            store.stats.playerWithStats[0].wins,
            store.stats.playerWithStats[1].wins,
            store.stats.ties
        );
        view.inititializeMoves(store.game.moves);
    }

    window.addEventListener("storage", () => {
        console.log("state changed from another tab");
        initView();
    });

    initView();

    view.bindGameResetEvent(() => {
        store.reset();
        initView();
    });

    view.bindNewRoundEvent(() => {
        store.newRound();
        initView();
    });

    view.bindPlayerMoveEvent((square) => {
        const existingMove = store.game.moves.find(
            (move) => move.squareId === +square.id
        );
        if (existingMove) {
            return;
        }

        view.handlePlayerMove(square, store.game.currentPlayer);

        store.playerMove(+square.id);

        if (store.game.status.isComplete) {
            const winTxt = store.game.status.winner
                ? `Player ${store.game.status.winner.id} wins!`
                : "Tie!";
            view.openWinDialog(winTxt);
            return;
        }

        view.setTurnIndicator(store.game.currentPlayer);
    });
}

window.addEventListener("load", init);
