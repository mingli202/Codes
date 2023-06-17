import { GameState, Player } from "./types";

const initialValue: GameState = {
    currentGameMoves: [],
    history: {
        currentRoundGames: [],
        allGames: [],
    },
};

export default class Store {
    constructor(
        private readonly storageKey: string,
        private readonly players: Player[]
    ) {}

    get stats() {
        const state = this.#getState();

        return {
            playerWithStats: this.players.map((player) => {
                const wins = state.history.currentRoundGames.filter((game) => {
                    return game.status.winner?.id === player.id;
                }).length;
                return {
                    ...player,
                    wins,
                };
            }),
            ties: state.history.currentRoundGames.filter((game) => {
                return game.status.winner === null;
            }).length,
        };
    }

    get game() {
        const state = this.#getState();
        const currentPlayer = this.players[state.currentGameMoves.length % 2];

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
        let winner;

        for (const player of this.players) {
            const selectedSquareIds = state.currentGameMoves
                .filter((move) => move.player.id === player.id)
                .map((move) => move.squareId);

            for (const pattern of winningPattern) {
                if (pattern.every((v) => selectedSquareIds.includes(v))) {
                    winner = player;
                    break;
                }
            }
        }

        return {
            currentPlayer,
            moves: state.currentGameMoves,
            status: {
                isComplete:
                    typeof winner !== "undefined" ||
                    state.currentGameMoves.length === 9,
                winner,
            },
        };
    }

    playerMove(squareId: number) {
        const stateClone = structuredClone(this.#getState());

        stateClone.currentGameMoves.push({
            squareId,
            player: this.game.currentPlayer,
        });

        this.#saveState(stateClone);
    }

    reset() {
        const stateClone = structuredClone(this.#getState());

        const { status, moves } = this.game;
        if (status.isComplete) {
            stateClone.history.currentRoundGames.push({
                moves,
                status,
            });
        }

        stateClone.currentGameMoves = [];

        this.#saveState(stateClone);
    }

    newRound() {
        this.reset();

        const stateClone = structuredClone(this.#getState()) as GameState;
        stateClone.history.allGames.push(
            ...stateClone.history.currentRoundGames
        );
        stateClone.history.currentRoundGames = [];

        this.#saveState(stateClone);
    }

    #getState() {
        const item = window.localStorage.getItem(this.storageKey);
        return item ? (JSON.parse(item) as GameState) : initialValue;
    }

    #saveState(
        stateOrFunction: GameState | ((prevState: GameState) => GameState)
    ) {
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

        window.localStorage.setItem(this.storageKey, JSON.stringify(newState));
    }
}
