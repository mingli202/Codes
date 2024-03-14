import pygame
from Player import Player
from Food import Food
import math

pygame.init()
screen = pygame.display.set_mode((1275, 725))
clock = pygame.time.Clock()
pygame.display.set_caption("Snake Game")

# initialize font
font = pygame.font.SysFont("Arial", 25)

singlePlayer = (
    math.floor(screen.get_width() / (2 * Player.SIZE)) * Player.SIZE,
    math.floor(screen.get_height() / (2 * Player.SIZE)) * Player.SIZE,
)


class State:
    DEATH = -1
    SINGLE_PLAYER = 0


def checkCollision(player: Player, food: Food):
    if player.x + player.SIZE > food.x and player.x < food.x + food.SIZE:
        if player.y + player.SIZE > food.y and player.y < food.y + food.SIZE:
            return True
    return False


def mainGame(player: Player, food: Food, state: int):
    # keypresses
    keys = pygame.key.get_pressed()
    if (keys[pygame.K_w] or keys[pygame.K_UP]) and not player.dy > 0:
        player.dy = -abs(player.speed)
        player.dx = 0
    elif (keys[pygame.K_s] or keys[pygame.K_DOWN]) and not player.dy < 0:
        player.dy = abs(player.speed)
        player.dx = 0
    elif (keys[pygame.K_a] or keys[pygame.K_LEFT]) and not player.dx > 0:
        player.dx = -abs(player.speed)
        player.dy = 0
    elif (keys[pygame.K_d] or keys[pygame.K_RIGHT]) and not player.dx < 0:
        player.dx = abs(player.speed)
        player.dy = 0

    # fill the screen with a color to wipe away anything from last frame
    screen.fill((0, 0, 0))

    # draw
    player.draw(screen)
    food.draw(screen)

    # check player death
    if player.checkSelfCollision():
        return State.DEATH

    # check collision with food
    if checkCollision(player, food):
        player.add(player.x, player.y)
        player.score += 1
        food.__init__(screen, player.body)

    # display score
    score_text = font.render(f"Score: {player.score}", True, (255, 255, 255))
    screen.blit(score_text, (10, 10))

    return state


def deathScreen(player: Player, food: Food, state: int):
    keys = pygame.key.get_pressed()
    if keys[pygame.K_n]:
        player.__init__(singlePlayer[0], singlePlayer[1], (255, 255, 255))
        food.__init__(screen, player.body)

        return State.SINGLE_PLAYER

    screen.fill((255, 255, 255))

    deathMsg = font.render(f"You died. Final Score: {player.score}", True, (255, 0, 0))
    screen.blit(
        deathMsg,
        (
            screen.get_width() / 2 - deathMsg.get_width() / 2,
            screen.get_height() / 2 - deathMsg.get_height(),
        ),
    )

    playAgainMsg = font.render("Press 'n' to play again", True, (255, 0, 0))
    screen.blit(
        playAgainMsg,
        (
            screen.get_width() / 2 - playAgainMsg.get_width() / 2,
            screen.get_height() / 2 + playAgainMsg.get_height(),
        ),
    )

    return state


def main():
    player = Player(singlePlayer[0], singlePlayer[1], (255, 255, 255))

    food = Food(screen, player.body)
    state = State.SINGLE_PLAYER

    running = True
    while running:
        # poll for events
        # pygame.QUIT event means the user clicked X to close your window
        for event in pygame.event.get():
            match event.type:
                case pygame.QUIT:
                    return pygame.quit()

        match state:
            case State.SINGLE_PLAYER:
                state = mainGame(player, food, state)
            case State.DEATH:
                state = deathScreen(player, food, state)

        # flip() the display to put your work on the screen
        pygame.display.update()

        # fps
        clock.tick(20)


if __name__ == "__main__":
    main()
