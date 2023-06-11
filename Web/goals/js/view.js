export default class View {
    $ = {};

    constructor() {
        this.$.navBtn = this.#qs('[data-id="nav-btn"]');
        this.$.homeSection = this.#qs('[data-id="home"]');
        this.$.body = this.#qs("body");
        this.$.goals = this.#qsAll('[data-id="goals"]');
        this.$.goalDetails = this.#qsAll('[data-id="goal-details"]');
        this.$.root = this.#qs(":root");
        this.$.goalSection = this.#qs('[data-id="goal-section"]');
        this.$.newGoal = this.#qs('[data-id="new-goal"]');
        this.$.navIcon = this.#qs(".nav-btn > i ");
        this.$.newGoalSection = this.#qs('[data-id="new-goal-section"]');
    }

    //* key bindings
    bindScrollButtonEvent(handler) {
        this.$.navBtn.addEventListener("click", handler);
    }

    bindGoalsShowEvent(handler) {
        this.$.goals.forEach((goal) =>
            goal.addEventListener("click", () => handler(goal))
        );
    }

    bingAddNewGoalEvent(handler) {
        this.$.newGoal.addEventListener("click", handler);
    }

    //* Ui functions

    collapseHeights() {
        this.$.goalDetails.forEach((goal) => {
            goal.style.height = 0;
            goal.style.margin = 0;
        });
        this.$.newGoalSection.style.height = 0;
        this.$.newGoalSection.style.width = 0;
        this.$.newGoalSection.style.visibility = "visible";
    }

    /**
     * Helper functions
     */
    #qs(element) {
        const el = document.querySelector(element);
        // check if element exists
        if (!el) {
            throw new Error(`Element ${element} does not exist`);
        }
        return el;
    }

    #qsAll(element) {
        const elList = document.querySelectorAll(element);
        // check if element exists
        if (!elList) {
            throw new Error(`Element ${element} does not exist`);
        }
        return elList;
    }
}
