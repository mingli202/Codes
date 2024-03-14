import pygame
import random
import math
import numpy as np


class Food:
    color = (255, 0, 0)
    SIZE = 25

    def __init__(self, screen: pygame.Surface, playerBody: np.ndarray):
        self.x = (
            math.floor(random.randint(0, screen.get_width() - self.SIZE) / self.SIZE)
            * self.SIZE
        )
        self.y = (
            math.floor(random.randint(0, screen.get_height() - self.SIZE) / self.SIZE)
            * self.SIZE
        )

        # make sure food is not on player
        # while any(part[0] == self.x and part[1] == self.y for part in playerBody):
        #     self.x = (
        #         math.floor(
        #             random.randint(0, screen.get_width() - self.SIZE) / self.SIZE
        #         )
        #         * self.SIZE
        #     )
        #     self.y = (
        #         math.floor(
        #             random.randint(0, screen.get_height() - self.SIZE) / self.SIZE
        #         )
        #         * self.SIZE
        #     )

    def draw(self, screen: pygame.Surface):
        pygame.draw.rect(screen, self.color, (self.x, self.y, self.SIZE, self.SIZE))
