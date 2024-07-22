#include <ncurses.h>
#include <stdlib.h>

#define ITERMAX 10000

int main(void) {
  long iter;
  int yi, xi;
  int y[3], x[3];
  int index;
  int maxlines, maxcols;

  // initialize curses;

  initscr(); // determines termial type, including size and features.
  cbreak();  // disables line buffering.
  noecho();  // not echo back the the input back to the screen.
  clear();   // clears the screen.

  // initialize triangle

  // LINES and COLS are set by initscr()
  maxlines = LINES - 1;
  maxcols = COLS - 1;

  y[0] = 0;
  x[0] = 0;

  y[1] = maxlines;
  x[1] = maxcols / 2;

  y[2] = 0;
  x[2] = maxcols;

  // Two simple methods to draw text on the screen are the addch() and addstr()
  // functions.
  // To put text at a specific screen location, use the related mvaddch() and
  // mvaddstr() functions.
  mvaddch(y[0], x[0], '0');
  mvaddch(y[1], x[1], '1');
  mvaddch(y[2], x[2], '2');

  // initlize yi, xi with random value;

  yi = rand() % maxlines;
  xi = rand() % maxcols;
  mvaddch(yi, xi, '.');

  // iterate the triangle;

  for (iter = 0; iter < ITERMAX; iter++) {
    index = rand() % 3;

    yi = (yi + y[index]) / 2;
    xi = (xi + x[index]) / 2;

    mvaddch(yi, xi, '*');
    refresh();
  }

  // done;

  mvaddstr(maxlines, 0, "Press any key to quit");

  refresh();

  // wait for user to press a key;
  getch();

  endwin(); // exit curses environment;
  exit(0);
}
