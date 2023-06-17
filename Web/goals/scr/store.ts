import { Heights, State, GoalData } from "./types";
import View from "./view.js";

const view = new View();

const initialState: State = {
    allGoals: [], //* array of objects containing the state of all goals
    button: "down",
    newGoalSection: {},
    newGoal: false,
    goalDatas: [],
};

export default class Store {
    state: State = window.localStorage.getItem(this.key)
        ? JSON.parse(window.localStorage.getItem(this.key) as string)
        : initialState;
    initialNewGoalState: HTMLElement;

    constructor(
        private readonly goalDetailsList: NodeListOf<Element>,
        private readonly newGoalInitialState: Node,
        private readonly key: string
    ) {
        this.initialNewGoalState = newGoalInitialState.cloneNode(
            true
        ) as HTMLElement;
        this.state.button = "down";
        this.state.newGoal = false;
    }

    get goalData() {
        return this.state.goalDatas;
    }

    #saveState(newState: State) {
        this.state = newState;
        window.localStorage.setItem(this.key, JSON.stringify(this.state));
    }

    #getState() {
        return this.state;
    }

    setInitialData(
        nodeList: NodeListOf<HTMLElement>,
        newGoalSection?: HTMLElement
    ) {
        const stateCopy: State = structuredClone(this.#getState());
        const list = Array.from(nodeList);
        const _3dvw = window.innerWidth * 0.03;

        stateCopy.allGoals = list.map((singleGoal) => {
            const elementData = {
                initialHeight: 0,
                finalHeight: singleGoal.clientHeight,
                initialMargin: 0,
                finalMargin: _3dvw,
                state: "closed",
            };
            return elementData;
        });

        if (newGoalSection) {
            stateCopy.newGoalSection = {
                height: newGoalSection.clientHeight,
                width: newGoalSection.clientWidth,
            };
        }

        this.#saveState(stateCopy);
    }

    elementDataChange(targetId: number) {
        const stateCopy: State = structuredClone(this.#getState());
        const oldState: Heights = stateCopy.allGoals[targetId];
        const newState: Heights = {
            initialHeight: oldState.finalHeight,
            finalHeight: oldState.initialHeight,
            initialMargin: oldState.finalMargin,
            finalMargin: oldState.initialMargin,
            state: oldState.state === "closed" ? "open" : "closed",
        };
        stateCopy.allGoals[targetId] = newState;
        this.#saveState(stateCopy);
    }

    navBtnStateChange() {
        const stateCopy = structuredClone(this.#getState());
        stateCopy.button = this.state.button === "down" ? "up" : "down";
        this.#saveState(stateCopy);
    }

    newGoalStateChange() {
        const stateCopy = structuredClone(this.#getState());
        stateCopy.newGoal = this.state.newGoal === true ? false : true;
        this.#saveState(stateCopy);
    }

    setNewGoalData(
        title: string,
        specific: string,
        plan: string,
        dueDate: string
    ) {
        const stateCopy = structuredClone(this.#getState());
        stateCopy.goalDatas.push({
            title,
            specific,
            plan,
            dueDate,
        });
        this.#saveState(stateCopy);
        console.log(stateCopy.goalDatas);
    }

    setBackToInitialState() {
        // put back the new goal section in the corner with zero width and zero height
        const goalSection = view.$.newGoalSection;
        goalSection.style.height = "0";
        goalSection.style.width = "0";
        goalSection.style.margin = "0";
        goalSection.style.opacity = "";

        // put back the form elements to its original state
        const form = view.$.form as HTMLFormElement;
        const formElements: NodeListOf<HTMLElement> =
            form.querySelectorAll("input, p, textarea");

        formElements.forEach((el) => {
            el.style.transform = "";
            el.style.opacity = "0";
            el.style.display = "";
        });

        // put back the ani element into its original state
        const ani: HTMLElement = document.querySelector(".ani") as HTMLElement;
        ani.style.opacity = "";
        ani.style.zIndex = "";
        ani.style.width = "";
        ani.style.height = "";
        ani.style.borderRadius = "";

        // puting the check icon back
        const icon: HTMLElement = document.getElementById(
            "icon"
        ) as HTMLElement;
        icon.style.fontSize = "0";

        // chinging back the new goal open state
        this.newGoalStateChange();
    }

    deleteGoal(id: number) {
        const stateCopy = structuredClone(this.#getState());
        stateCopy.goalDatas.splice(id, 1);
        this.#saveState(stateCopy);
    }

    editGoal(id: number, newData: GoalData) {
        const stateCopy = structuredClone(this.#getState());
        stateCopy.goalDatas[id] = newData
        this.#saveState(stateCopy);
    }
}
