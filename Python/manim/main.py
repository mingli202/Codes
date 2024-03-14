import manim as mn


class CreateCircle(mn.Scene):
    def construct(self):
        circle = mn.Circle()  # create a circle
        # set the color and transparency
        circle.set_fill(mn.PINK, opacity=0.5)
        self.play(mn.Create(circle))  # show the circle on screen


class SquareToCircle(mn.Scene):
    def construct(self):
        circle = mn.Circle()  # create a circle
        circle.set_fill(mn.PINK, opacity=0.5)  # set color and transparency

        square = mn.Square()  # create a square
        square.rotate(mn.PI / 4)  # rotate a certain amount

        self.play(mn.Create(square))  # animate the creation of the square
        # interpolate the square into the circle
        self.play(mn.Transform(square, circle))
        self.play(mn.FadeOut(square))  # fade out animation


class SquareAndCircle(mn.Scene):
    def construct(self):
        circle = mn.Circle()
        circle.set_fill(mn.GREEN, 0.5)

        square = mn.Square()
        square.set_fill(mn.RED, 0.5)

        square.next_to(circle, mn.RIGHT, buff=0.5)
        self.play(mn.Create(circle), mn.Create(square))


class AnimateSquareToCircle(mn.Scene):
    def construct(self):
        square = mn.Square()
        circle = mn.Circle()

        self.play(mn.Create(square))
        self.play(mn.Rotate(square, mn.PI / 4))

        self.play(mn.ReplacementTransform(square, circle))
        self.play(circle.animate.set_fill(mn.PINK, 0.5))


class DifferentRotations(mn.Scene):
    def construct(self):
        left_square = mn.Square(
            color=mn.BLUE, fill_opacity=0.7).shift(2 * mn.LEFT)

        right_square = mn.Square(
            color=mn.GREEN, fill_opacity=0.7).shift(2 * mn.RIGHT)

        self.play(left_square.animate.rotate(mn.PI), mn.Rotate(
            right_square, angle=mn.PI), run_time=2)
        self.wait()
