#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <algorithm>
#include <vector>
#include <array>
#include <limits>
#include <random>
#include <SDL2/SDL.h>
#include <OpenGL/gl.h>
#include <GLUT/glut.h>


typedef struct place{
    int player;
    int x;
    int y;
}place;


class Player{
    public:
    int px;
    int py;

    void turn(vector<vector<int>>& m){
        px = 1;
        py = 1;
    }
};

void update_board(SDL_Renderer* renderer, int x, int y, vector<place> p){
    SDL_RenderClear(renderer);

    // Bg color
    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    SDL_Rect rect = {0, 0, 650, 700};
    SDL_RenderFillRect(renderer, &rect);

    // Board
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    SDL_RenderDrawLine(renderer, 233, 50, 233, 600);
    SDL_RenderDrawLine(renderer, 417, 50, 417, 600);
    SDL_RenderDrawLine(renderer, 50, 233, 600, 233);
    SDL_RenderDrawLine(renderer, 50, 417, 600, 417);

    // Square
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    SDL_Rect sq = {x, y, 163, 163};
    SDL_RenderFillRect(renderer, &sq);

    // Draw square
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    SDL_Rect ds;
    for (place i : p){
        ds = {i.x, i.y, 163, 163};
        SDL_RenderFillRect(renderer, &ds);
    }

    // Draw X
    

    // Draw O
    

    SDL_RenderPresent(renderer);
}

int main()
{

    vector<vector<int>> map{
        {0, 0, 0},
        {0, 0, 0},
        {0, 0, 0}
    };

    // Array to keep track of where the rectangle is drawn
    vector<place> p;
    place tmp;


    // Player initialization
    Player p1;
    Player p2;
    int turn = 0;

    // SDL
    SDL_Init(SDL_INIT_EVERYTHING);

    // Screan
    SDL_Window* window = SDL_CreateWindow("Tic Tac Toe", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 650, 700, SDL_WINDOW_SHOWN);
    if (window == NULL){
        cout << "No window" << SDL_GetError();
        return 1;
    }

    // Renderer
    SDL_Renderer* renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
    if (renderer == NULL){
        cout << "No renderer" << SDL_GetError();
        return 1;
    }

    // Starting position of rectangle
    int rx = 244;
    int ry = 244;

    // Main loop
    bool quit = false;
    while(!quit){
        SDL_Event event;
        while(SDL_PollEvent(&event)){
            if(event.type == SDL_QUIT){
                quit = true;
            }

            if(event.type == SDL_MOUSEMOTION && event.motion.x < 600 && event.motion.y < 600){
                rx = 183 * floor((event.motion.x - 50) / 183) + 60;
                ry = 183 * floor((event.motion.y - 50) / 183) + 60;
            }

            if (event.type == SDL_MOUSEBUTTONDOWN){
                if (turn = 0){
                    tmp.player = 0;
                    turn = 1;
                }
                else if (turn = 1){
                    tmp.player = 1;
                    turn = 0;
                }

                tmp.x = rx;
                tmp.y = ry;
                p.push_back(tmp);
            }
        }
        update_board(renderer, rx, ry, p);
    }


    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();
    return 0;
}