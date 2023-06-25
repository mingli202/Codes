import "./App.css";
import Modal from "./components/Modal";
import LeftMenu from "./components/LeftMenu";
import { useState } from "react";
import { GameState, Player } from "./types";
import classNames from "classnames";

const players: Player[] = [
  {
    id: 1,
    name: "Player 1",
    iconClass: "fa-x",
    colorClass: "color4",
  },
  {
    id: 2,
    name: "Player 2",
    iconClass: "fa-o",
    colorClass: "color5",
  },
];

function deriveGame(state: GameState) {
  const currentPlayer = players[state.currentGameMoves.length % 2];

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

  for (const player of players) {
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
        typeof winner !== "undefined" || state.currentGameMoves.length === 9,
      winner,
    },
  };
}

function deriveStats(state: GameState) {
  return {
    playerWithStats: players.map((player) => {
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

export default function App() {
  const [state, setState] = useState<GameState>({
    currentGameMoves: [],
    history: {
      currentRoundGames: [],
      allGames: [],
    },
  });

  const game = deriveGame(state);
  const stats = deriveStats(state);

  function handlePlayerMoves(squareId: number, player: Player) {
    setState((prev) => {
      const stateClone = structuredClone(prev);

      stateClone.currentGameMoves.push({
        squareId,
        player,
      });

      return stateClone;
    });
  }

  function resetGame(isNewRound: boolean) {
    setState((prev) => {
      const stateClone = structuredClone(prev);

      const { status, moves } = game;
      if (status.isComplete) {
        stateClone.history.currentRoundGames.push({
          moves,
          status,
        });
      }

      stateClone.currentGameMoves = [];

      if (isNewRound) {
        stateClone.history.allGames.push(
          ...stateClone.history.currentRoundGames
        );
        stateClone.history.currentRoundGames = [];
      }

      return stateClone;
    });
  }

  return (
    <>
      <header>Tic Tac Toe</header>
      <main>
        {/* <!-- Left side --> */}
        <LeftMenu
          onAction={(action) => resetGame(action === "new-round")}
          player={game.currentPlayer}
        />

        {/* main board */}
        <div className="grid-container">
          {[1, 2, 3, 4, 5, 6, 7, 8, 9].map((squareId) => {
            const existingMoves = game.moves.find(
              (move) => move.squareId === squareId
            );

            return (
              <div
                key={squareId}
                className="board"
                onClick={() => {
                  if (existingMoves) return;

                  handlePlayerMoves(squareId, game.currentPlayer);
                }}
              >
                {existingMoves && (
                  <i
                    className={classNames("fa", existingMoves.player.iconClass)}
                  ></i>
                )}
              </div>
            );
          })}
        </div>

        {/* <!-- right side --> */}
        <div className="right">
          <div className="score" style={{ backgroundColor: "var(--color4)" }}>
            <p>Player 1:</p>
            <p>{stats.playerWithStats[0].wins} wins</p>
          </div>
          <div className="score" style={{ backgroundColor: "var(--color3)" }}>
            <p>Ties:</p>
            <p>{stats.ties}</p>
          </div>
          <div className="score" style={{ backgroundColor: "var(--color5)" }}>
            <p>Player 2:</p>
            <p>{stats.playerWithStats[1].wins} wins</p>
          </div>
        </div>
      </main>

      {game.status.isComplete && (
        <Modal
          message={
            game.status.winner ? `${game.status.winner.name} wins!` : "Tie! "
          }
          onClick={() => {
            resetGame(false);
          }}
        />
      )}
    </>
  );
}
