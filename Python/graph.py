from matplotlib import pyplot as plt
from matplotlib.widgets import Slider
import numpy as np


def intensityD(angle, d, wavelength, n):
    iMax = n**2
    xi = 1
    yi = 0

    # using the phasor diagram technique to add up vectors
    for i in range(1, n):
        phi = 2 * np.pi * d * np.sin(i * angle) / wavelength
        xi += np.cos(phi)
        yi += np.sin(phi)

    return (xi**2 + yi**2) / iMax


def intensityA(angle, a, wavelength):
    beta = 2 * np.pi * a * np.sin(angle) / wavelength
    return (np.sin(beta / 2) / (beta / 2)) ** 2


def main():
    # initial values
    n = 2
    d = 63.14e-6
    a = 25.3e-6
    wavelength = 589.3e-9

    # range of angle
    x = np.linspace(-np.pi / 70, np.pi / 70, 1000)

    diffraction = [intensityA(i, a, wavelength) for i in x]
    interference = [
        intensityD(i, d, wavelength, n) * intensityA(i, a, wavelength) for i in x
    ]

    # graph
    fig, ax = plt.subplots()

    ax.set_xlabel("Angular Position, θ (rad)")
    ax.set_ylabel("Intensity ratio, I/Imax")
    ax.set_title(
        "Figure 1: Diffraction grating interference pattern example (d = 63.14µm, a = 25.3µm, λ = 589.3nm)"
    )

    ax.plot(x, diffraction, "--")

    (line,) = ax.plot(x, interference)
    fig.subplots_adjust(left=0.25, bottom=0.25)

    iAxis = fig.add_axes([0.25, 0.1, 0.65, 0.03])

    iSlider = Slider(iAxis, "n slits", 2, 25, 2, valstep=1)

    def update(val):
        line.set_ydata(
            [
                intensityD(i, d, wavelength, val) *
                intensityA(i, a, wavelength)
                for i in x
            ]
        )
        fig.canvas.draw_idle()

    iSlider.on_changed(update)

    plt.show()


if __name__ == "__main__":
    main()
