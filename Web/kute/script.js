const box = document.querySelectorAll(".box");
// box.forEach((i) => i.addEventListener("click", clickAnimation));

const _delay = 1000;
const _options = {
    easing: "easingExponentialInOut",
    duration: 2000,
    offset: _delay,
    yoyo: true,
    repeat: 1,
};

function clickAnimation() {
    console.log("clicked!");
    // coolMvt();
    // simpleRotate.start();
    // perpectiveAnimation.start();
    // borderAnimation.start();
    svgAnimate.start();
}

let svgAnimate = KUTE.fromTo(
    "#wave1",
    { path: "#wave1"},
    { path: "#wave2" },
    _options
)

let borderAnimation = KUTE.allFromTo(
    box,
    { borderRadius: "0%" },
    { borderRadius: "50%" },
    _options
);

let perpectiveAnimation = KUTE.allFromTo(
    box,
    { translate3d: [0, 0, 0] },
    { translate3d: [0, 0, 400] },
    _options
);

let simpleRotate = KUTE.allFromTo(
    box,
    { rotate3d: [0, 0, 0], translate3d: [0, 0, 0] },
    { rotate3d: [0, 360, 0], translate3d: [0, 0, -500] },
    {
        easing: "easingExponentialOut",
        duration: 4000,
        offset: _delay,
        yoyo: true,
        repeat: 1,
    }
);

let coolAnime1 = KUTE.allFromTo(
    box,
    { rotate3d: [0, 0, 0] },
    { rotate3d: [0, 120, 720] },
    { easing: "easingExponentialOut", duration: 5000, offset: _delay }
);

let coolAnime2 = KUTE.allFromTo(
    box,
    { rotate3d: [0, 120, 720], translate3d: [0, 0, 0] },
    { rotate3d: [360, 240, 60], translate3d: [100, 100, 100] },
    { easing: "easingQuarticInOut", duration: 3000, offset: _delay }
);

let coolAnime3 = KUTE.allFromTo(
    box,
    { rotate3d: [360, 240, 60], translate3d: [100, 100, 100] },
    { rotate3d: [0, 0, 0], translate3d: [0, 0, 0] },
    { easing: "easingCubicInOut", duration: 4000, offset: _delay }
);

function coolMvt() {
    coolAnime1.chain(coolAnime2);
    coolAnime2.chain(coolAnime3);
    coolAnime1.start();
}
