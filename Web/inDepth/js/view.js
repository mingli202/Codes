export default class View {
    $ = {};
    constructor() {
        this.$.resetBtn = this.#qs('[data-id="reset-btn"]');
        this.$.newRoundbtn = this.#qs('[data-id="new-round-btn"]');
        this.$.square = this.#qsAll('[data-id="square"]');
        this.$.winDialog = this.#qs('[data-id="win"]');
        this.$.winnerTxt = this.#qs('[data-id="winner-txt"]');
        this.$.againBtn = this.#qs('[data-id="again-btn"]');
        this.$.playerIcon = this.#qs('[data-id="player-icon"]');
        this.$.innerTurn = this.#qs(".inner-turn");
        this.$.actionIcon = this.#qs('[data-id="action-icon"]');
        this.$.action = this.#qs(".action");

        this.$.action.onmouseover = () => {
            this.#menuIn();
        };
        this.$.action.onmouseout = () => {
            this.#menuOut();
        };
    }
    /**
     * Register all event listeners
     */
    bindGameResetEvent(handler) {
        this.$.resetBtn.addEventListener("click", handler);
        this.$.againBtn.addEventListener("click", handler);
    }

    bindNewRoundEvent(handler) {
        this.$.newRoundbtn.addEventListener("click", handler);
    }

    bindPlayerMoveEvent(handler) {
        this.$.square.forEach((square) => {
            square.addEventListener("click", () => handler(square));
        });
    }

    /**
     * DOM Helper Methods
     */
    openWinDialog(messsage) {
        this.$.winDialog.style.display = "flex";
        this.$.winnerTxt.innerText = messsage;
    }

    closeWinDialog() {
        this.$.winDialog.style.display = "none";
    }

    clearMoves() {
        this.$.square.forEach((square) => {
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

    handlePlayerMove(squareEl, player) {
        const icon = document.createElement("i");
        icon.classList.add(player.iconClass, "fa");
        squareEl.replaceChildren(icon);
    }

    setTurnIndicator(player) {
        const icon = document.createElement("i");
        const label = document.createElement("p");

        icon.classList.add(player.iconClass, "fa");
        this.$.innerTurn.style.color =
            player.id === 1 ? "var(--color4)" : "var(--color5)";
        label.innerText = `${player.name} Go!`;

        this.$.innerTurn.replaceChildren(icon, label);
    }

    #qsAll(selector) {
        const elList = document.querySelectorAll(selector);
        if (!elList) {
            throw new Error(`Cannot find element: ${selector}`);
        }
        return elList;
    }

    #qs(selector, parent) {
        const el = parent
            ? parent.querySelector(selector)
            : document.querySelector(selector);
        if (!el) {
            throw new Error(`Cannot find element: ${selector}`);
        }
        return el;
    }
}
