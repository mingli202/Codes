import pygame
import random

# Initialize Pygame
pygame.init()

# Define some colors
BLACK = (0, 0, 0)
WHITE = (255, 255, 255)
GREEN = (0, 255, 0)
RED = (255, 0, 0)

# Set the width and height of the screen
SCREEN_WIDTH = 600
SCREEN_HEIGHT = 400
BLOCK_SIZE = 10

# Create the screen
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))

# Set the caption of the window
pygame.display.set_caption("Snake Game")

# Set the font for displaying text
font = pygame.font.SysFont(None, 25)

# Define the function to display the message on the screen
def message_to_screen(msg, color):
    screen_text = font.render(msg, True, color)
    screen.blit(screen_text, [SCREEN_WIDTH/6, SCREEN_HEIGHT/3])

# Define the main function for the game
def gameLoop():
    # Initialize the game variables
    game_over = False
    game_close = False

    # Initialize the starting position of the snake
    x1 = SCREEN_WIDTH/2
    y1 = SCREEN_HEIGHT/2

    # Initialize the change in position of the snake
    x1_change = 0
    y1_change = 0

    # Initialize the length of the snake
    snake_List = []
    Length_of_snake = 1

    # Initialize the food position
    foodx = round(random.randrange(0, SCREEN_WIDTH - BLOCK_SIZE) / 10.0) * 10.0
    foody = round(random.randrange(0, SCREEN_HEIGHT - BLOCK_SIZE) / 10.0) * 10.0

    # Game loop
    while not game_over:

        # Check for events
        while game_close == True:
            screen.fill(WHITE)
            message_to_screen("You Lost! Press Q-Quit or C-Play Again", RED)
            pygame.display.update()

            # Check for key events
            for event in pygame.event.get():
                if event.type == pygame.KEYDOWN:
                    if event.key == pygame.K_q:
                        game_over = True
                        game_close = False
                    if event.key == pygame.K_c:
                        gameLoop()

        # Check for key events
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                game_over = True
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_LEFT:
                    x1_change = -BLOCK_SIZE
                    y1_change = 0
                elif event.key == pygame.K_RIGHT:
                    x1_change = BLOCK_SIZE
                    y1_change = 0
                elif event.key == pygame.K_UP:
                    y1_change = -BLOCK_SIZE
                    x1_change = 0
                elif event.key == pygame.K_DOWN:
                    y1_change = BLOCK_SIZE
                    x1_change = 0

        # Check if the snake hits the boundaries
        if x1 >= SCREEN_WIDTH or x1 < 0 or y1 >= SCREEN_HEIGHT or y1 < 0:
            game_close = True

        # Update the position of the snake
        x1 += x1_change
        y1 += y1_change

        # Draw the screen
        screen.fill(WHITE)
        pygame.draw.rect(screen, GREEN, [foodx, foody, BLOCK_SIZE, BLOCK_SIZE])

        # Update the snake's length
        snake_Head = []
        snake_Head.append(x1)
        snake_Head
        snake_Head.append(y1)
        snake_List.append(snake_Head)
        if len(snake_List) > Length_of_snake:
            del snake_List[0]

        # Check if the snake hits itself
        for x in snake_List[:-1]:
            if x == snake_Head:
                game_close = True

        # Draw the snake
        for x in snake_List:
            pygame.draw.rect(screen, BLACK, [x[0], x[1], BLOCK_SIZE, BLOCK_SIZE])

        # Update the score
        score = Length_of_snake - 1
        score_text = font.render("Score: " + str(score), True, RED)
        screen.blit(score_text, [0, 0])

        # Update the display
        pygame.display.update()

        # Check if the snake eats the food
        if x1 == foodx and y1 == foody:
            foodx = round(random.randrange(0, SCREEN_WIDTH - BLOCK_SIZE) / 10.0) * 10.0
            foody = round(random.randrange(0, SCREEN_HEIGHT - BLOCK_SIZE) / 10.0) * 10.0
            Length_of_snake += 1

        # Set the game's FPS
        clock = pygame.time.Clock()
        clock.tick(20)

    # Quit Pygame
    pygame.quit()


if __name__ == "__main__":
    gameLoop()
