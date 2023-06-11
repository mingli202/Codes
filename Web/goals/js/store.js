const initialState = {
    allGoals: [], //* array of objects containing the state of all goals
    button: "down",
    newGoalSection: {},
    newGoal: false,
};

/**
 *  Object state {
 *      Object allGoals: [
 *          {
 *              initialHeight,
 *              finalHeight,
 *              initialMargin,
 *              finalMargin,
 *              state, // closed or open
 *          },
 *          ...
 *      ],
 *      button: "down"
 * }
 */

export default class Store {
    state = initialState;

    constructor(goalDetailsList) {
        this.goalDetailsList = goalDetailsList; //* NodeList
    }

    get state() {
        return this.state.allGoals;
    }

    setInitialData(nodeList, newGoalSection) {
        let stateCopy = structuredClone(this.state);
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

        stateCopy.newGoalSection = {
            height: newGoalSection.clientHeight,
            width: newGoalSection.clientWidth,
        };

        this.state = stateCopy;
        console.log(stateCopy);
    }

    elementDataChange(targetId) {
        const oldState = this.state.allGoals[targetId];
        // console.log(oldState);
        const newState = {
            initialHeight: oldState.finalHeight,
            finalHeight: oldState.initialHeight,
            initialMargin: oldState.finalMargin,
            finalMargin: oldState.initialMargin,
            state: oldState.state === "closed" ? "open" : "closed",
        };
        this.state.allGoals[targetId] = newState;
    }

    navBtnStateChange() {
        const stateCopy = structuredClone(this.state);
        stateCopy.button = this.state.button === "down" ? "up" : "down";
        this.state = stateCopy;
        // console.log(this.state);
    }

    newGoalStateChange() {
        const stateCopy = structuredClone(this.state);
        stateCopy.newGoal = this.state.newGoal === true ? false : true;
        this.state = stateCopy;
    }
}
