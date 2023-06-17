var __classPrivateFieldGet = (this && this.__classPrivateFieldGet) || function (receiver, state, kind, f) {
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a getter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
    return kind === "m" ? f : kind === "a" ? f.call(receiver) : f ? f.value : state.get(receiver);
};
var _Store_instances, _Store_saveState, _Store_getState;
import View from "./view.js";
const view = new View();
const initialState = {
    allGoals: [],
    button: "down",
    newGoalSection: {},
    newGoal: false,
    goalDatas: [],
};
class Store {
    constructor(goalDetailsList, newGoalInitialState, key) {
        _Store_instances.add(this);
        this.goalDetailsList = goalDetailsList;
        this.newGoalInitialState = newGoalInitialState;
        this.key = key;
        this.state = window.localStorage.getItem(this.key)
            ? JSON.parse(window.localStorage.getItem(this.key))
            : initialState;
        this.initialNewGoalState = newGoalInitialState.cloneNode(true);
        this.state.button = "down";
        this.state.newGoal = false;
    }
    get goalData() {
        return this.state.goalDatas;
    }
    setInitialData(nodeList, newGoalSection) {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
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
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
    elementDataChange(targetId) {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        const oldState = stateCopy.allGoals[targetId];
        const newState = {
            initialHeight: oldState.finalHeight,
            finalHeight: oldState.initialHeight,
            initialMargin: oldState.finalMargin,
            finalMargin: oldState.initialMargin,
            state: oldState.state === "closed" ? "open" : "closed",
        };
        stateCopy.allGoals[targetId] = newState;
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
    navBtnStateChange() {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        stateCopy.button = this.state.button === "down" ? "up" : "down";
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
    newGoalStateChange() {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        stateCopy.newGoal = this.state.newGoal === true ? false : true;
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
    setNewGoalData(title, specific, plan, dueDate) {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        stateCopy.goalDatas.push({
            title,
            specific,
            plan,
            dueDate,
        });
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
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
        const form = view.$.form;
        const formElements = form.querySelectorAll("input, p, textarea");
        formElements.forEach((el) => {
            el.style.transform = "";
            el.style.opacity = "0";
            el.style.display = "";
        });
        // put back the ani element into its original state
        const ani = document.querySelector(".ani");
        ani.style.opacity = "";
        ani.style.zIndex = "";
        ani.style.width = "";
        ani.style.height = "";
        ani.style.borderRadius = "";
        // puting the check icon back
        const icon = document.getElementById("icon");
        icon.style.fontSize = "0";
        // chinging back the new goal open state
        this.newGoalStateChange();
    }
    deleteGoal(id) {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        stateCopy.goalDatas.splice(id, 1);
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
    editGoal(id, newData) {
        const stateCopy = structuredClone(__classPrivateFieldGet(this, _Store_instances, "m", _Store_getState).call(this));
        stateCopy.goalDatas[id] = newData;
        __classPrivateFieldGet(this, _Store_instances, "m", _Store_saveState).call(this, stateCopy);
    }
}
_Store_instances = new WeakSet(), _Store_saveState = function _Store_saveState(newState) {
    this.state = newState;
    window.localStorage.setItem(this.key, JSON.stringify(this.state));
}, _Store_getState = function _Store_getState() {
    return this.state;
};
export default Store;
