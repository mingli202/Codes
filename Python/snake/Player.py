import pygame
import numpy as np


class Player:
    SIZE = 25
    speed = 25

    def __init__(self, x: int, y: int, color):
        self.x = x
        self.y = y
        self.dx = 0
        self.dy = 0
        self.body = np.array([np.array([x, y])])
        self.color = color
        self.score = 0

    def draw(self, screen: pygame.Surface):
        self.x += self.dx
        self.y += self.dy

        # check if out of bounds
        if self.x > screen.get_width() - self.SIZE:
            self.x = 0
        if self.y > screen.get_height() - self.SIZE:
            self.y = 0

        if self.x < 0:
            self.x = screen.get_width() - self.SIZE
        if self.y < 0:
            self.y = screen.get_height() - self.SIZE

        # update body
        self.add(self.x, self.y)
        self.body = self.body[1:]

        # draw body
        for part in self.body:
            pygame.draw.rect(
                screen, self.color, (part[0], part[1], self.SIZE, self.SIZE)
            )

    # expand body
    def add(self, x: int, y: int):
        self.body = np.append(self.body, np.array([np.array([x, y])]), 0)

    def checkSelfCollision(self):
        return any(part[0] == self.x and part[1] == self.y for part in self.body[:-1])
