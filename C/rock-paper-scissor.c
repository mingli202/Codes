#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

int main(int argc, char *argv[]) {

  char playerMove;

  while (1) {
    printf("Input your move: \n");
    printf("r/p/s (rock/paper/scissors): ");
    scanf("%c", &playerMove);

    if (playerMove == 'r' || playerMove == 'p' || playerMove == 's') {
      break;
    }
    printf("Invalid input!\n\n");
  }

  srand(time(NULL));
  int r = rand() % 3;
  char *computerMove = "rps";
  computerMove += r;

  switch (*computerMove) {
  case 'r':
    printf("You lost!\n");
    break;

  case 'p':
    printf("You won!\n");
    break;

  case 's':
    printf("Draw :\\\n");
    break;
  }

  return EXIT_SUCCESS;
}
