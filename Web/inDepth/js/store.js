const initialValue = {
    moves: [],
    history: {
        currentRoundGames: [],
        allgames: [],
    },
};

export default class Store {
    #state = initialValue;

    constructor(players) {
        this.players = players;
    }

    get game() {
        const state = this.#getState();
        const currentPlayer = this.players[state.moves.length % 2];

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
        let winner = null;

        for (const player of this.players) {
            const selectedSquareIds = state.moves
                .filter((move) => move.player.id === player.id)
                .map((move) => move.squareId);

            for (const pattern of winningPattern) {
                if (pattern.every((v) => selectedSquareIds.includes(v))) {
                    winner = player.id;
                    break;
                }
            }
        }

        return {
            currentPlayer,
            moves: state.moves,
            status: {
                isComplete: winner != null || state.moves.length === 9,
                winner,
            },
        };
    }

    playerMove(squareId) {
        const state = this.#getState();
        const stateClone = structuredClone(state);

        stateClone.moves.push({
            squareId,
            player: this.game.currentPlayer,
        });

        this.#saveState(stateClone);
    }

    reset() {
        this.#saveState(initialValue);
    }

    #getState() {
        return this.#state;
    }
    #saveState(stateOrFunction) {
        const prevState = this.#getState();
        let newState;

        switch (typeof stateOrFunction) {
            case "function":
                newState = stateOrFunction(prevState);
                break;
            case "object":
                newState = stateOrFunction;
                break;
            default:
                throw new Error(
                    `Invalid state type: ${typeof stateOrFunction}`
                );
        }

        this.#state = newState;
    }
}
