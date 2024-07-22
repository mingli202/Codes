all: compile run

compile:
	mkdir -p bin && clang -std=c2x main.c -o bin/main `pkgconf --libs --cflags ncurses`

run:
	./bin/main
