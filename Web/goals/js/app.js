import View from "./view.js";
import Store from "./store.js";

function setGoalsHeights(view, store) {
    store.setInitialData(view.$.goalDetails, view.$.newGoalSection);
    view.collapseHeights();
}

function init() {
    const view = new View();
    const store = new Store(view.$.goalDetails);

    setGoalsHeights(view, store);

    function f() {
        //* scroll one 100vh down
        console.log("clicked");

        const vh = window.innerHeight;
        const homeSection = view.$.homeSection;
        const body = view.$.body;
        const goalsList = view.$.goals;
        const btnState = store.state.button;
        const navBtn = view.$.navBtn;

        let b = (vIfTrue, vIfFalse) => {
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
        }, b(5000, 0));

        const scroll = anime({
            targets: homeSection,
            translateY: b(-vh, 0),
            duration: 2000,
            easing: b("easeInCubic", "easeOutCubic"),
            delay: b(0, 3000),
        });
        scroll.play();

        const bgScroll = anime({
            targets: "body",
            backgroundPosition: b(["0% 0%", "0% 100%"], ["0% 100%", "0% 0%"]),
            duration: 5000,
            easing: "easeInOutCubic",
        });
        bgScroll.play();

        const goalScroll = anime({
            targets: goalsList,
            translateY: b(-vh, 0),
            delay: anime.stagger(200, {
                start: b(2000, 0),
                direction: b("normal", "reverse"),
            }),
            easing: b("easeOutCubic", "easeInCubic"),
            duration: 3000,
        });
        goalScroll.play();

        const rotateNavBtn = anime({
            targets: navBtn,
            duration: 3000,
            delay: 5000,
            rotate: b(180, 0),
            easing: "easeOutElastic",
        });
        rotateNavBtn.play();

        store.navBtnStateChange();
    }
    view.bindScrollButtonEvent(f);

    view.bindGoalsShowEvent((goal) => {
        const target = goal.querySelector('[data-id="goal-details"]');
        const targetId = +goal.id;

        const initialHeight = store.state.allGoals[targetId].initialHeight;
        const finalHeight = store.state.allGoals[targetId].finalHeight;
        const initialMargin = store.state.allGoals[targetId].initialMargin;
        const finalMargin = store.state.allGoals[targetId].finalMargin;
        const state = store.state.allGoals[targetId].state;

        const show = anime({
            targets: target,
            margin: {
                value: [initialMargin, finalMargin],
                duration: 1000,
                easing: "easeOutQuart",
                delay: state === "closed" ? 0 : 500,
            },
            height: {
                value: [initialHeight, finalHeight],
                duration: 1000,
                easing: "easeOutQuart",
                delay: state === "closed" ? 0 : 500,
            },
            opacity: {
                value: state === "closed" ? 1 : 0,
                duration: 1000,
                easing: "easeOutQuart",
                delay: state === "closed" ? 500 : 0,
            },
        });
        show.play();

        store.elementDataChange(targetId);
    });

    view.bingAddNewGoalEvent(() => {
        const navBtn = view.$.navBtn;
        const navIcon = view.$.navIcon;
        const newGoalSection = view.$.newGoalSection;
        const newGoalHeight = store.state.newGoalSection.height;
        const newGoalWidth = store.state.newGoalSection.width;
        const newGoal = store.state.newGoal;

        let b = (vIfTrue, vIfFalse) => {
            const value = newGoal === false ? vIfTrue : vIfFalse;
            return value;
        };

        const shrink = anime({
            targets: navBtn,
            duration: 500,
            width: b(0, 100),
            height: b(0, 100),
            easing: "spring",
        });
        shrink.play();

        const shrink1 = anime({
            targets: navIcon,
            duration: 500,
            fontSize: b(0, 75),
            easing: "spring",
        });
        shrink1.play();

        const exp = anime({
            targets: newGoalSection,
            duration: 1000,
            easing: "spring(1, 100, 17, 0)",
            height: b(newGoalHeight, 0),
            width: b(newGoalWidth, 0),
        });
        exp.play();

        store.newGoalStateChange();
    });
}

window.onload = init;
