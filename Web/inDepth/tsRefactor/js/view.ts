import { Move, Player } from "./types";

export default class View {
    $: Record<string, HTMLElement> = {};
    $$: Record<string, NodeListOf<HTMLElement>> = {};
    constructor() {
        this.$.resetBtn = this.#qs('[data-id="reset-btn"]');
        this.$.newRoundbtn = this.#qs('[data-id="new-round-btn"]');
        this.$$.square = this.#qsAll('[data-id="square"]');
        this.$.winDialog = this.#qs('[data-id="win"]');
        this.$.winnerTxt = this.#qs('[data-id="winner-txt"]');
        this.$.againBtn = this.#qs('[data-id="again-btn"]');
        this.$.playerIcon = this.#qs('[data-id="player-icon"]');
        this.$.innerTurn = this.#qs(".inner-turn");
        this.$.actionIcon = this.#qs('[data-id="action-icon"]');
        this.$.action = this.#qs(".action");
        this.$.player1Score = this.#qs('[data-id="player1-score"]');
        this.$.player2Score = this.#qs('[data-id="player2-score"]');
        this.$.tiesScore = this.#qs('[data-id="ties-score"]');

        this.$.action.addEventListener("mouseover", () => {
            this.#menuIn();
        });
        this.$.action.addEventListener("mouseout", () => {
            this.#menuOut();
        });
    }
    /**
     * Register all event listeners
     */
    bindGameResetEvent(handler: EventListener) {
        this.$.resetBtn.addEventListener("click", handler);
        this.$.againBtn.addEventListener("click", handler);
    }

    bindNewRoundEvent(handler: EventListener) {
        this.$.newRoundbtn.addEventListener("click", handler);
    }

    bindPlayerMoveEvent(handler: (el: Element) => void) {
        this.$$.square.forEach((square) => {
            square.addEventListener("click", () => handler(square));
        });
    }

    /**
     * DOM Helper Methods
     */
    updateScore(p1Wins: number, p2Wins: number, ties: number) {
        this.$.player1Score.textContent = `${p1Wins} wins`;
        this.$.tiesScore.textContent = `${ties}`;
        this.$.player2Score.textContent = `${p2Wins} wins`;
    }

    openWinDialog(messsage: string) {
        this.$.winDialog.style.display = "flex";
        this.$.winnerTxt.textContent = messsage;
    }

    closeWinDialog() {
        this.$.winDialog.style.display = "none";
    }

    clearMoves() {
        this.$$.square.forEach((square) => {
            square.replaceChildren();
        });
    }

    #menuIn() {
        this.$.actionIcon.classList.remove("fa-angle-down");
        this.$.actionIcon.classList.add("fa-angle-up");
    }
    #menuOut() {
        this.$.actionIcon.classList.remove("fa-angle-up");
        this.$.actionIcon.classList.add("fa-angle-down");
    }

    handlePlayerMove(squareEl: Element, player: Player) {
        const icon = document.createElement("i");
        icon.classList.add(player.iconClass, "fa");
        squareEl.replaceChildren(icon);
    }

    inititializeMoves(moves: Move[]) {
        this.$$.square.forEach((square) => {
            const existingMove = moves.find(
                (move) => move.squareId === +square.id
            );
            if (existingMove) {
                this.handlePlayerMove(square, existingMove.player);
            }
        });
    }

    setTurnIndicator(player: Player) {
        const icon = document.createElement("i");
        const label = document.createElement("p");

        icon.classList.add(player.iconClass, "fa");
        this.$.innerTurn.style.color =
            player.id === 1 ? "var(--color4)" : "var(--color5)";
        label.innerText = `${player.name} Go!`;

        this.$.innerTurn.replaceChildren(icon, label);
    }

    #qsAll(selector: string) {
        const elList = document.querySelectorAll<HTMLElement>(selector);
        if (!elList) {
            throw new Error(`Cannot find element: ${selector}`);
        }
        return elList;
    }

    #qs(selector: string, parent?: Element) {
        const el = parent
            ? parent.querySelector<HTMLElement>(selector)
            : document.querySelector<HTMLElement>(selector);
        if (!el) {
            throw new Error(`Cannot find element: ${selector}`);
        }
        return el;
    }
}
