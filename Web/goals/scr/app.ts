import View from "./view.js";
import Store from "./store.js";
import anime from "../anime-master/lib/anime.es.js";
import { GoalData } from "./types.js";

function initView(view: View, store: Store) {
    view.updateGoalSection(store.state.goalDatas);
    store.setInitialData(view.$$.goalDetails, view.$.newGoalSection);
    view.collapseHeights();
    const form = view.$.form;
    const formElements: NodeListOf<HTMLElement> =
        form.querySelectorAll("input, p, textarea");

    // make everything invisible first
    formElements.forEach((el) => {
        el.style.opacity = "0";
    });
}

function updateGoal(
    view: View,
    store: Store,
    f3: (el: HTMLElement) => void,
    f4: (e: Event, el: HTMLElement) => void,
    f5: (e: Event, el: HTMLElement) => void
) {
    view.updateGoalSection(store.state.goalDatas);
    store.setInitialData(view.$$.goalDetails);
    view.collapseHeights();
    view.bindGoalsShowEvent(f3);
    view.bindEditGoalEvent(f4);
    view.bindDeleteGoalEvent(f5);
}

function init() {
    const view = new View();
    const store = new Store(
        view.$$.goalDetails,
        view.$.newGoalSection,
        "goals"
    );

    initView(view, store);

    function f() {
        //* scroll one 100vh down
        const vh = window.innerHeight;
        const homeSection = view.$.homeSection;
        const body = view.$.body;
        const goalsList = view.$$.goals;
        const btnState = store.state.button;
        const navBtn = view.$.navBtn;

        let b = (vIfTrue: any, vIfFalse: any) => {
            const value = btnState === "down" ? vIfTrue : vIfFalse;
            return value;
        };

        navBtn.removeEventListener("click", f);

        setTimeout(() => {
            navBtn.addEventListener("click", f);
        }, 5000);
        setTimeout(() => {
            body.style.overflowY = b("scroll", "hidden");
            window.scroll(0, 0);
            body.style.backgroundAttachment = b("fixed", "");
        }, b(5000, 0));

        // home section scroll up
        anime({
            targets: homeSection,
            translateY: b(-vh, 0),
            duration: 2000,
            easing: b("easeInCubic", "easeOutCubic"),
            delay: b(0, 3000),
        });

        // bg image that scrolls up to give the illusion of depth
        anime({
            targets: "body",
            backgroundPosition: b(["0% 0%", "0% 100%"], ["0% 100%", "0% 0%"]),
            duration: 5000,
            easing: "easeInOutCubic",
        });

        // the goals that move up into view
        anime({
            targets: goalsList,
            translateY: b(-vh, 0),
            delay: anime.stagger(200, {
                start: b(2000, 0),
                direction: b("normal", "reverse"),
            }),
            easing: b("easeOutCubic", "easeInCubic"),
            duration: 3000,
        });

        // cool animation for the nav button
        anime({
            targets: navBtn,
            duration: 3000,
            delay: 5000,
            rotate: b(180, 0),
            easing: "easeOutElastic",
        });

        store.navBtnStateChange();
    }
    view.bindScrollButtonEvent(f);

    function f3(goal: Element) {
        const target = goal.querySelector('[data-id="goal-details"]');
        const targetId = +goal.id;

        const initialHeight = store.state.allGoals[targetId].initialHeight;
        const finalHeight = store.state.allGoals[targetId].finalHeight;
        const initialMargin = store.state.allGoals[targetId].initialMargin;
        const finalMargin = store.state.allGoals[targetId].finalMargin;
        const state = store.state.allGoals[targetId].state;

        // show the goals' details
        anime({
            targets: target,
            duration: 1000,
            easing: "easeOutQuart",
            delay: state === "closed" ? 0 : 500,
            margin: [initialMargin, finalMargin],
            height: [initialHeight, finalHeight],
            opacity: {
                value: state === "closed" ? 1 : 0,
                delay: state === "closed" ? 500 : 0,
            },
        });

        // animation of the goal box
        anime({
            targets: goal,
            duration: 1000,
            delay: state === "closed" ? 0 : 500,
            easing: "easeOutQuart",
            backgroundColor:
                state === "closed" ? "#001d3d" : "rgba(0, 0, 0, 0.3)",
            borderRadius: state === "closed" ? "1rem" : "0",
        });

        store.elementDataChange(targetId);
    }
    view.bindGoalsShowEvent(f3);

    function f1() {
        const navBtn = view.$.navBtn;
        const navIcon = view.$.navIcon;
        const newGoalSection = view.$.newGoalSection;
        const newGoalHeight = store.state.newGoalSection.height;
        const newGoalWidth = store.state.newGoalSection.width;
        const newGoal = store.state.newGoal;
        const form = view.$.form as HTMLFormElement;
        const formElements: NodeListOf<HTMLElement> =
            form.querySelectorAll("input, p, textarea");

        form.reset();

        let b = (vIfTrue: any, vIfFalse: any) => {
            const value = newGoal === false ? vIfTrue : vIfFalse;
            return value;
        };

        // nab nutton shrinking out of view
        anime({
            targets: navBtn,
            duration: 500,
            width: b(0, 100),
            height: b(0, 100),
            easing: "spring",
            delay: b(0, 500),
        });

        // nav button-icon shrinking out of view
        anime({
            targets: navIcon,
            duration: 500,
            fontSize: b(0, 75),
            easing: "spring",
            delay: b(0, 500),
        });

        //  new goal section expanding into view
        anime({
            targets: newGoalSection,
            duration: 1000,
            easing: "spring(1, 100, 17, 0)",
            height: b(newGoalHeight, 0),
            width: b(newGoalWidth, 0),
            margin: b("2rem", 0),
            delay: b(0, 500),
        });

        // each element fading in from opacity 0
        anime({
            targets: formElements,
            duration: 1000,
            easing: "easeOutQuint",
            delay: anime.stagger(50, {
                start: b(500, 0),
                direction: b("normal", "reverse"),
            }),
            opacity: b(1, 0),
        });

        store.newGoalStateChange();
    }
    view.bindAddNewGoalEvent(f1);

    view.bindCloseGoalEvent(f1);

    function f2() {
        const goalTitleForm: HTMLInputElement = document.querySelector(
            'textarea[name = "goalTitle"]'
        ) as HTMLInputElement;
        const goalSpecForm: HTMLInputElement = document.querySelector(
            'textarea[name = "goalSpecific"]'
        ) as HTMLInputElement;
        const goalPlanForm: HTMLInputElement = document.querySelector(
            'textarea[name = "goalPlan"]'
        ) as HTMLInputElement;
        const goalDateForm: HTMLInputElement = document.querySelector(
            'textarea[name = "goalDate"]'
        ) as HTMLInputElement;

        if (
            !goalTitleForm.value ||
            !goalSpecForm.value ||
            !goalPlanForm.value ||
            !goalDateForm.value
        ) {
            alert("There are sections unfilled");
            return;
        } else {
            store.setNewGoalData(
                goalTitleForm.value,
                goalSpecForm.value,
                goalPlanForm.value,
                goalDateForm.value
            );
        }

        const form = view.$.form;
        const formElements: NodeListOf<HTMLElement> =
            form.querySelectorAll("p, textarea");
        const ani: HTMLElement = document.querySelector(".ani") as HTMLElement;
        ani.style.zIndex = "1";
        const icon: HTMLElement = document.getElementById(
            "icon"
        ) as HTMLElement;
        const newGoal = document.querySelector(".new-goal") as HTMLElement;
        const navBtn = view.$.navBtn;
        const navIcon = view.$.navIcon;
        const formBtns: NodeListOf<HTMLElement> =
            form.querySelectorAll("input");

        // make the btns disappear so that they dont break something
        anime({
            targets: formBtns,
            opacity: 0,
            duration: 200,
            easing: "easeOutQuad",
            complete: () => {
                view.removeCloseGoalEvent(f1);
                view.removeOnSubmitEvent(f2);
            },
        });

        // elements disappear and translatingZ -100
        anime({
            targets: formElements,
            translateZ: {
                value: -100,
            },
            opacity: {
                value: 0,
                delay: 500,
            },
            duration: 1000,
            easing: "easeOutQuint",
        });

        // making the ani element shring into a ball
        anime({
            targets: ani,
            opacity: { value: 1, delay: 500 },
            height: 100,
            width: 100,
            borderRadius: "50%",
            delay: 700,
            duration: 1000,
            easing: "easeOutQuint",
        });

        // making the check icon grow into view
        anime({
            targets: icon,
            fontSize: "8rem",
            duration: 1000,
            easing: "spring",
            delay: 900,
        });

        // making everything fade out after the check icon has been displayed
        anime({
            targets: newGoal,
            opacity: 0,
            duration: 1000,
            easing: "easeOutQuad",
            delay: 1700,
            complete: () => {
                store.setBackToInitialState();
                // nab nutton grow in of view
                anime({
                    targets: navBtn,
                    duration: 500,
                    width: 100,
                    height: 100,
                    easing: "spring",
                });

                // nav button-icon grow in of view
                anime({
                    targets: navIcon,
                    duration: 500,
                    fontSize: 75,
                    easing: "spring",
                });

                // add back the event listeners
                view.bindCloseGoalEvent(f1);
                view.bindOnSubmitEvent(f2);
                updateGoal(view, store, f3, f4, f5);
            },
        });
    }
    view.bindOnSubmitEvent(f2);
    view.bindOnSubmitEvent((e: Event) => {
        e.preventDefault();
    });

    function f4(e: Event, el: HTMLElement) {
        e.stopImmediatePropagation();
        console.log("edit");

        if (el.dataset.num) {
            const id = +el.dataset.num;
            const data = store.goalData[id];

            const goalSection = view.$.goalSection;
            const oldGoal = document.getElementById(`${id}`) as Node;
            const editGoal: HTMLElement = oldGoal.cloneNode(
                true
            ) as HTMLElement;

            goalSection.replaceChild(editGoal, oldGoal);

            editGoal.innerHTML = `
                <form style="font-size: 7rem;">
                    <div class="goal-title">
                        <i class="fa fa-square check-icon"></i>
                        <span contenteditable class="edit" data-id="editGoalTitle">${data.title}</span>
                        <span class="goal-date edit" data-id="editGoalDate" contenteditable>${data.dueDate}</span>
                    </div>
                    <div class="goal-details" style="opacity: 1;" data-id="goal-details">
                        <div class="bold">Specific Goal:</div>
                        <div contenteditable class="edit" data-id="editGoalSpec">${data.specific}</div>
                        <div class="bold">Action plan:</div>
                        <div contenteditable class="edit" data-id="editGoalPlan">${data.plan}</div>
                        <div class="goal-action">
                            <span data-id="ok" data-num="${id}">Ok</span>
                            <span data-id="cancel" data-num="${id}">Cancel</span>
                        </div>
                    </div>
                </form>
            `;

            const ok: Element = document.querySelector(
                `[data-id="ok"]`
            ) as Element;

            ok.addEventListener("click", () => {
                const newGoalTitle: string = editGoal.querySelector(
                    '[data-id="editGoalTitle"]'
                )?.textContent as string;
                const editGoalDate: string = editGoal.querySelector(
                    '[data-id="editGoalDate"]'
                )?.textContent as string;
                const editGoalSpec: string = editGoal.querySelector(
                    '[data-id="editGoalSpec"]'
                )?.textContent as string;
                const editGoalPlan: string = editGoal.querySelector(
                    '[data-id="editGoalPlan"]'
                )?.textContent as string;

                const data: GoalData = {
                    title: newGoalTitle,
                    specific: editGoalSpec,
                    plan: editGoalPlan,
                    dueDate: editGoalDate,
                };
                store.editGoal(id, data);
                updateGoal(view, store, f3, f4, f5);

                // move them back up
                const vh = window.innerHeight;
                const goalsList = view.$$.goals;
                anime({
                    targets: goalsList,
                    translateY: -vh,
                    duration: 0,
                });
            });

            const cancel: Element = document.querySelector(
                `[data-id="cancel"]`
            ) as Element;

            cancel.addEventListener("click", () => {
                // goalSection.replaceChild(oldGoal, editGoal);
                updateGoal(view, store, f3, f4, f5);

                // move them back up
                const vh = window.innerHeight;
                const goalsList = view.$$.goals;
                anime({
                    targets: goalsList,
                    translateY: -vh,
                    duration: 0,
                });
            });
        } else {
            throw new Error("[data-num] does not exist");
        }
    }
    view.bindEditGoalEvent(f4);

    function f5(e: Event, el: HTMLElement) {
        e.stopImmediatePropagation();
        const vh = window.innerHeight;

        if (el.dataset.num) {
            const id = +el.dataset.num;
            store.deleteGoal(id);

            updateGoal(view, store, f3, f4, f5);

            // move them back up
            const goalsList = view.$$.goals;
            anime({
                targets: goalsList,
                translateY: -vh,
                duration: 0,
            });
        } else {
            throw new Error("[data-num] does not exist");
        }
    }
    view.bindDeleteGoalEvent(f5);
}

window.onload = init;
