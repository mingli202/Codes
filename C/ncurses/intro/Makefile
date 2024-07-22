all: compile run

compile:
	mkdir -p bin && clang main.c -o bin/main `pkgconf --libs --cflags ncurses`

run:
	./bin/main

demo:
	mkdir -p bin && clang demo.c -o bin/demo `pkgconf --libs --cflags ncurses` && ./bin/demo
