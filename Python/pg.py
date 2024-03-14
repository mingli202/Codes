import pygame

pygame.init()
screen = pygame.display.set_mode((800, 600))
clock = pygame.time.Clock()
font = pygame.font.SysFont("Arial", 30)

run = True


class player:
    x = screen.get_width() / 2
    y = screen.get_height() / 2
    SIZE = 50
    COLOR = (255, 255, 255)


class block:
    x = 600
    y = screen.get_height() / 2
    SIZE = 50
    COLOR = (255, 0, 0)


scene = 1

while run:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False

    keys = pygame.key.get_pressed()
    if keys[pygame.K_LEFT]:
        player.x -= 5

    elif keys[pygame.K_RIGHT]:

        if player.x + player.SIZE < block.x or scene != 2:
            player.x += 5
        else:
            player.x = block.x - player.SIZE

    elif keys[pygame.K_UP]:
        player.y -= 5
    elif keys[pygame.K_DOWN]:
        player.y += 5

    if player.x > screen.get_width() - 50:
        player.x = 0
        scene += 1

    screen.fill((0, 0, 0))

    if scene == 2:
        pygame.draw.rect(
            screen,
            [
                (c, c * (1 - (block.x - player.x) / block.x))[scene == 2]
                for c in block.COLOR
            ],
            (block.x, block.y, block.SIZE, block.SIZE),
        )

    pygame.draw.rect(
        screen,
        player.COLOR,
        (player.x, player.y, player.SIZE, player.SIZE),
    )

    txt = font.render(f"scene: {scene}", True, (255, 255, 255))
    screen.blit(txt, (10, 10))

    pygame.display.update()
    clock.tick(60)
