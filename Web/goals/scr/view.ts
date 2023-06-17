import { GoalData } from "./types";

export default class View {
    $: Record<string, HTMLElement> = {};
    $$: Record<string, NodeListOf<HTMLElement>> = {};

    constructor() {
        this.$.navBtn = this.#qs('[data-id="nav-btn"]');
        this.$.homeSection = this.#qs('[data-id="home"]');
        this.$.body = this.#qs("body");
        this.$.root = this.#qs(":root");
        this.$.goalSection = this.#qs('[data-id="goal-section"]');
        this.$.newGoal = this.#qs('[data-id="new-goal"]');
        this.$.navIcon = this.#qs(".nav-btn > i ");
        this.$.newGoalSection = this.#qs('[data-id="new-goal-section"]');
        this.$.closeBtn = this.#qs('[data-id="close-btn"]');
        this.$.form = this.#qs('[data-id="form"]');

        this.$$.goals = this.#qsAll('[data-id="goals"]');
        this.$$.goalDetails = this.#qsAll('[data-id="goal-details"]');
        this.$$.edit = this.#qsAll('[data-id="edit"]');
        this.$$.delete = this.#qsAll('[data-id="delete"]');
    }

    //* key bindings
    bindScrollButtonEvent(handler: EventListener) {
        this.$.navBtn.addEventListener("click", handler);
    }

    bindGoalsShowEvent(handler: (el: HTMLElement) => void) {
        this.$$.goals.forEach((goal) =>
            goal.addEventListener("click", () => handler(goal))
        );
    }

    bindAddNewGoalEvent(handler: EventListener) {
        this.$.newGoal.addEventListener("click", handler);
    }

    bindCloseGoalEvent(handler: EventListener) {
        this.$.closeBtn.addEventListener("click", handler);
    }

    removeCloseGoalEvent(handler: EventListener) {
        this.$.closeBtn.removeEventListener("click", handler);
    }

    bindOnSubmitEvent(handler: EventListener) {
        this.$.form.addEventListener("submit", handler);
    }

    removeOnSubmitEvent(handler: EventListener) {
        this.$.form.removeEventListener("submit", handler);
    }

    bindEditGoalEvent(handler: (e: Event, el: HTMLElement) => void) {
        this.$$.edit.forEach((ed) =>
            ed.addEventListener("click", (e) => handler(e, ed))
        );
    }

    bindDeleteGoalEvent(handler: (e:Event, el: HTMLElement) => void) {
        this.$$.delete.forEach((de) =>
            de.addEventListener("click", (e) => handler(e, de))
        );
    }

    //* Ui functions
    collapseHeights() {
        this.$$.goalDetails.forEach((goal) => {
            goal.style.height = "0";
            goal.style.margin = "0";
        });
        this.$.newGoalSection.style.height = "0";
        this.$.newGoalSection.style.width = "0";
        this.$.newGoalSection.style.visibility = "visible";
        this.$.newGoalSection.style.margin = "0";
    }

    updateGoalSection(goalDatas: GoalData[]) {
        this.$.goalSection.replaceChildren();
        for (let i = 0; i < goalDatas.length; i++) {
            const newGoal = document.createElement("div");
            newGoal.className = "goal";
            newGoal.dataset.id = "goals";
            newGoal.id = `${i}`;

            newGoal.innerHTML = `
                <div class="goal-title">
                    <i class="fa fa-square check-icon"></i>
                    <span>${goalDatas[i].title}</span>
                    <span class="goal-date yellow">${goalDatas[i].dueDate}</span>
                </div>
                <div class="goal-details" data-id="goal-details">
                    <div class="bold">Specific Goal:</div>
                    <div>${goalDatas[i].specific}</div>
                    <div class="bold">Action plan:</div>
                    <div>${goalDatas[i].plan}</div>
                    <div class="goal-action">
                        <span data-id="edit" data-num="${i}">Edit</span>
                        <span data-id="delete" data-num="${i}">Delete</span>
                    </div>
                </div>
            `;

            this.$.goalSection.appendChild(newGoal);
        }

        // update the node list
        this.$.newGoalSection = this.#qs('[data-id="new-goal-section"]');
        this.$$.goals = this.#qsAll('[data-id="goals"]');
        this.$$.goalDetails = this.#qsAll('[data-id="goal-details"]');
        this.$$.edit = this.#qsAll('[data-id="edit"]');
        this.$$.delete = this.#qsAll('[data-id="delete"]');
    }

    /**
     * Helper functions
     */
    #qs(element: string) {
        const el: HTMLElement | null = document.querySelector(element);
        // check if element exists
        if (!el) {
            throw new Error(`Element ${element} does not exist`);
        }
        return el;
    }

    #qsAll(element: string) {
        const elList: NodeListOf<HTMLElement> | null =
            document.querySelectorAll(element);
        // check if element exists
        if (!elList) {
            throw new Error(`Element ${element} does not exist`);
        }
        return elList;
    }
}
