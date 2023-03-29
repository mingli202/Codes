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

void board(SDL_Renderer* renderer){
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
}

int main()
{
    
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

    int rx = 244;
    int ry = 244;
    SDL_Rect sq = {rx, ry, 163, 163};

    SDL_RenderClear(renderer);
    board(renderer);
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 100);
    SDL_RenderFillRect(renderer, &sq);
    SDL_RenderPresent(renderer);

    bool quit = false;

    while(!quit){
        SDL_Event event;
        while(SDL_PollEvent(&event)){
            if(event.type == SDL_QUIT){
                quit = true;
            }

            // Keyboard Event
            if(event.type == SDL_KEYDOWN){
                if(event.key.keysym.sym == SDLK_UP && ry > 70){
                    SDL_RenderClear(renderer);
                    board(renderer);
                    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 100);
                    ry -= 183;
                    sq = {rx, ry, 163, 163};
                    SDL_RenderFillRect(renderer, &sq);
                    SDL_RenderPresent(renderer);
                }
                if(event.key.keysym.sym == SDLK_DOWN && ry < 400){
                    SDL_RenderClear(renderer);
                    board(renderer);
                    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 100);
                    ry += 183;
                    sq = {rx, ry, 163, 163};
                    SDL_RenderFillRect(renderer, &sq);
                    SDL_RenderPresent(renderer);
                }
                if(event.key.keysym.sym == SDLK_LEFT && rx > 70){
                    SDL_RenderClear(renderer);
                    board(renderer);
                    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 100);
                    rx -= 183;
                    sq = {rx, ry, 163, 163};
                    SDL_RenderFillRect(renderer, &sq);
                    SDL_RenderPresent(renderer);
                }
                if(event.key.keysym.sym == SDLK_RIGHT && rx < 400){
                    SDL_RenderClear(renderer);
                    board(renderer);
                    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 100);
                    rx += 183;
                    sq = {rx, ry, 163, 163};
                    SDL_RenderFillRect(renderer, &sq);
                    SDL_RenderPresent(renderer);
                }
            }
        }
    }

    return 0;
}