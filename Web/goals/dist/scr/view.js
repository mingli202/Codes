var __classPrivateFieldGet = (this && this.__classPrivateFieldGet) || function (receiver, state, kind, f) {
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a getter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
    return kind === "m" ? f : kind === "a" ? f.call(receiver) : f ? f.value : state.get(receiver);
};
var _View_instances, _View_qs, _View_qsAll;
class View {
    constructor() {
        _View_instances.add(this);
        this.$ = {};
        this.$$ = {};
        this.$.navBtn = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="nav-btn"]');
        this.$.homeSection = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="home"]');
        this.$.body = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, "body");
        this.$.root = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, ":root");
        this.$.goalSection = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="goal-section"]');
        this.$.newGoal = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="new-goal"]');
        this.$.navIcon = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, ".nav-btn > i ");
        this.$.newGoalSection = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="new-goal-section"]');
        this.$.closeBtn = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="close-btn"]');
        this.$.form = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="form"]');
        this.$$.goals = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="goals"]');
        this.$$.goalDetails = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="goal-details"]');
        this.$$.edit = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="edit"]');
        this.$$.delete = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="delete"]');
    }
    //* key bindings
    bindScrollButtonEvent(handler) {
        this.$.navBtn.addEventListener("click", handler);
    }
    bindGoalsShowEvent(handler) {
        this.$$.goals.forEach((goal) => goal.addEventListener("click", () => handler(goal)));
    }
    bindAddNewGoalEvent(handler) {
        this.$.newGoal.addEventListener("click", handler);
    }
    bindCloseGoalEvent(handler) {
        this.$.closeBtn.addEventListener("click", handler);
    }
    removeCloseGoalEvent(handler) {
        this.$.closeBtn.removeEventListener("click", handler);
    }
    bindOnSubmitEvent(handler) {
        this.$.form.addEventListener("submit", handler);
    }
    removeOnSubmitEvent(handler) {
        this.$.form.removeEventListener("submit", handler);
    }
    bindEditGoalEvent(handler) {
        this.$$.edit.forEach((ed) => ed.addEventListener("click", (e) => handler(e, ed)));
    }
    bindDeleteGoalEvent(handler) {
        this.$$.delete.forEach((de) => de.addEventListener("click", (e) => handler(e, de)));
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
    updateGoalSection(goalDatas) {
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
        this.$.newGoalSection = __classPrivateFieldGet(this, _View_instances, "m", _View_qs).call(this, '[data-id="new-goal-section"]');
        this.$$.goals = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="goals"]');
        this.$$.goalDetails = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="goal-details"]');
        this.$$.edit = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="edit"]');
        this.$$.delete = __classPrivateFieldGet(this, _View_instances, "m", _View_qsAll).call(this, '[data-id="delete"]');
    }
}
_View_instances = new WeakSet(), _View_qs = function _View_qs(element) {
    const el = document.querySelector(element);
    // check if element exists
    if (!el) {
        throw new Error(`Element ${element} does not exist`);
    }
    return el;
}, _View_qsAll = function _View_qsAll(element) {
    const elList = document.querySelectorAll(element);
    // check if element exists
    if (!elList) {
        throw new Error(`Element ${element} does not exist`);
    }
    return elList;
};
export default View;
