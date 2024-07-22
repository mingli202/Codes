#include <ncurses.h>
#include <stdbool.h>

void quit();

int main(int argc, char *argv[]) {
  initscr();
  cbreak();
  noecho();
  keypad(stdscr, TRUE);
  mousemask(ALL_MOUSE_EVENTS, NULL);

  clear();
  refresh();

  int width = 20, height = 10;
  int y_0 = (LINES - height) / 2, x_0 = (COLS - width) / 2;

  int ch;
  WINDOW *win;
  MEVENT event;

  while (ch != 27 && ch != 10 && ch != 113) {
    win = newwin(height, width, y_0, x_0);
    box(win, 0, 0);
    wrefresh(win);

    ch = getch();

    switch (ch) {
    case KEY_RIGHT:
      x_0 += 2;
      break;
    case KEY_LEFT:
      x_0 -= 2;
      break;
    case KEY_UP:
      y_0--;
      break;
    case KEY_DOWN:
      y_0++;
      break;
    case KEY_MOUSE:
      if (getmouse(&event) == OK) {
        printw("%lu\n", event.bstate);
      }
    }

    wborder(win, ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ');
    wrefresh(win);
    delwin(win);
  }

  start_color();
  if (has_colors() == false) {
    printw("Your terminal is trash");
    quit();
    return 1;
  }

  clear();
  init_pair(1, COLOR_RED, COLOR_BLACK);
  init_pair(2, COLOR_BLUE, COLOR_RED);

  attron(COLOR_PAIR(2));
  printw("Hello world but in red");
  attroff(COLOR_PAIR(2));
  getch();

  quit();
  return 0;
}

void quit() {
  clear();
  printw("Press any key to exit");
  getch();
  endwin();
}
