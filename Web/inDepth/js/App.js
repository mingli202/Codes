import View from "./view.js";
import Store from "./store.js";

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

const App = {
    // All of our selected HTML elements. This $ sign is just common practice.
    $: {
        resetBtn: document.querySelector('[data-id="reset-btn"]'),
        newRoundbtn: document.querySelector('[data-id="new-round-btn"]'),
        square: document.querySelectorAll('[data-id="square"]'),
        winDialog: document.querySelector('[data-id="win"]'),
        winnerTxt: document.querySelector('[data-id="winner-txt"]'),
        againBtn: document.querySelector('[data-id="again-btn"]'),
        playerIcon: document.querySelector('[data-id="player-icon"]'),
        innerTurn: document.querySelector(".inner-turn"),
    },

    state: {
        player: 1,
        moves: [],
    },

    getGameState(moves) {
        const winningPattern = [
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9],
            [1, 4, 7],
            [2, 5, 8],
            [3, 6, 9],
            [1, 5, 9],
            [3, 5, 7],
        ];

        const p1Moves = moves
            .filter((move) => move.playerId === 1)
            .map((move) => +move.squareId);
        const p2Moves = moves
            .filter((move) => move.playerId === 2)
            .map((move) => +move.squareId);

        let winner = null;

        winningPattern.forEach((pattern) => {
            const p1Wins = pattern.every((move) => p1Moves.includes(move));
            const p2Wins = pattern.every((move) => p2Moves.includes(move));

            if (p1Wins) {
                winner = 1;
            }
            if (p2Wins) {
                winner = 2;
            }
        });

        return {
            status:
                moves.length === 9 || winner != null
                    ? "completed"
                    : "in-progress", // In progress or completed
            winner, // 1 or 2 or null
        };
    },

    init() {
        App.registerEvents();
    },

    // Event listerners.
    registerEvents() {
        // TODO
        App.$.resetBtn.addEventListener("click", () => {
            console.log("Reset button clicked");
        });

        // TODO
        App.$.newRoundbtn.addEventListener("click", () => {
            console.log("New round button clicked");
        });

        App.$.againBtn.addEventListener("click", (e) => {
            console.log("Again button clicked");
            App.$.winDialog.style.display = "none";
            App.state.moves = [];
            App.$.square.forEach((square) => {
                square.replaceChildren();
            });
            App.state.player = 1;
            App.$.innerTurn.innerHTML =
                '\
                <i class="fa fa-x icon" data-id="player-icon"></i>\
                <!-- Player state -->\
                <p data-id="player-go">Player 1 Go!</p>';
            App.$.innerTurn.style.color = "var(--color4)";
        });

        // TODO
        App.$.square.forEach((square) => {
            square.addEventListener("click", (e) => {
                //* check if the square is empty
                const hasMove = (squareId) => {
                    const existingMove = App.state.moves.find(
                        (move) => move.squareId === squareId
                    );
                    return existingMove !== undefined;
                };
                if (hasMove(+square.id)) {
                    return;
                }

                const player = App.state.player;
                const icon = document.createElement("i");

                if (player === 1) {
                    icon.classList.add("fa", "fa-x");
                    App.$.innerTurn.style.color = "var(--color5)";
                    App.$.innerTurn.innerHTML =
                        '\
                        <i class="fa fa-o icon" data-id="player-icon"></i>\
                        <!-- Player state -->\
                        <p data-id="player-go">Player 2 Go!</p>';
                } else {
                    icon.classList.add("fa", "fa-o");
                    App.$.innerTurn.style.color = "var(--color4)";
                    App.$.innerTurn.innerHTML =
                        '\
                        <i class="fa fa-x icon" data-id="player-icon"></i>\
                        <!-- Player state -->\
                        <p data-id="player-go">Player 1 Go!</p>';
                }

                //* check board state
                App.state.moves.push({
                    squareId: +square.id,
                    playerId: player,
                });

                App.state.player = player === 1 ? 2 : 1;
                square.replaceChildren(icon);

                //* check for win
                const game = App.getGameState(App.state.moves);
                if (game.status === "completed") {
                    App.$.winDialog.style.display = "flex";

                    if (game.winner) {
                        App.$.winnerTxt.innerHTML = `Player ${game.winner} wins!`;
                    } else {
                        App.$.winnerTxt.innerHTML = "Tie!";
                    }
                }
            });
        });
    },
};

const players = [
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
    const store = new Store(players);

    view.bindGameResetEvent(() => {
        view.closeWinDialog();
        store.reset();
        view.clearMoves();
        view.setTurnIndicator(store.game.currentPlayer);
    });

    // TODO
    view.bindNewRoundEvent((e) => {
        console.log("New Round Event");
        console.log(e);
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
                ? `Player ${store.game.status.winner} wins!`
                : "Tie!";
            view.openWinDialog(winTxt);
            return;
        }

        view.setTurnIndicator(store.game.currentPlayer);
    });
}

window.addEventListener("load", init);
