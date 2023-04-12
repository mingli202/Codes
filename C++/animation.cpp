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
#include <SDL2_ttf/SDL_ttf.h>

typedef struct epicenter{
    float x;
    float y;
    int dx;
    int dy;
    int time;
}epicenter;

void bg(SDL_Renderer* renderer){
    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    SDL_Rect backgound = {0, 0, 1200, 750};
    SDL_RenderFillRect(renderer, &backgound);
}

void draw(SDL_Renderer* renderer){
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

    // parabola
    // for (int i = 0; i < 1201; i++){
    //     SDL_RenderDrawPointF(renderer, i, -0.00052 * pow(i, 2) + 750);
    // }

    // circle
    // for (float x = 0, y; x < 1201; x+=0.01){
    //     y = sqrt(200 * 200 - pow(x - 600, 2)) + 210;
    //     SDL_RenderDrawPointF(renderer, x, y);
    //     y = -sqrt(200 * 200 - pow(x - 600, 2)) + 210;
    //     SDL_RenderDrawPointF(renderer, x, y);
    // }

}

void animation_on_press(SDL_Renderer* renderer, vector<epicenter>& epicenters){
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    int speed = 5;

    // animation
    for (epicenter& i : epicenters){
        /* expanding diagonal squares */
        // SDL_RenderDrawLineF(renderer, i.x, i.y + i.dy * speed, i.x + i.dx * speed, i.y);
        // SDL_RenderDrawLineF(renderer, i.x, i.y - i.dy * speed, i.x + i.dx * speed, i.y);
        // SDL_RenderDrawLineF(renderer, i.x, i.y + i.dy * speed, i.x - i.dx * speed, i.y);
        // SDL_RenderDrawLineF(renderer, i.x, i.y - i.dy * speed, i.x - i.dx * speed, i.y);

        /* expanding circles */
        // for (float x = 0, y; x < 1201; x+=1){
        //     y = sqrt(pow(i.dx * speed, 2) - pow(x - i.x, 2)) + i.y;
        //     SDL_RenderDrawPoint(renderer, x, y);
        //     y = -sqrt(pow(i.dx * speed, 2) - pow(x - i.x, 2)) + i.y;
        //     SDL_RenderDrawPoint(renderer, x, y);
        // }
        // if (i.dx * speed > 2000){
        //     epicenters.erase(epicenters.begin());
        // }

        /* falling blocks */
        // float y = 2 * (float) pow(i.dy, 2) + i.y;
        // if (y >= 730 && i.dy >= 0){
        //     i.dy = -i.dy;
        //     i.y += (731 - i.y) / 5;
        // }
        // SDL_FRect falling_sq = {i.x - 10, y, 20, 20};
        // SDL_RenderFillRectF(renderer, &falling_sq);
        // if (i.y >= 730){
        //     epicenters.erase(epicenters.begin());
        // }

        /* Expanding and retracting squares */
        float peak = 50 / pow(2, i.time);
        if (i.dx >= peak){
            i.dx = -i.dx;
            i.time += 1;
        }
        SDL_RenderDrawLineF(renderer, i.x, i.y + i.dx * speed, i.x + i.dx * speed, i.y);
        SDL_RenderDrawLineF(renderer, i.x, i.y - i.dx * speed, i.x + i.dx * speed, i.y);
        SDL_RenderDrawLineF(renderer, i.x, i.y + i.dx * speed, i.x - i.dx * speed, i.y);
        SDL_RenderDrawLineF(renderer, i.x, i.y - i.dx * speed, i.x - i.dx * speed, i.y);
        if (peak <= 1){
            epicenters.erase(epicenters.begin());
        }


        i.dx+=1;
        i.dy+=1;
    }
}

int main(){

    SDL_Init(SDL_INIT_EVERYTHING);
    TTF_Init();

    SDL_Window* window = SDL_CreateWindow("Animation", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1200, 750, SDL_WINDOW_SHOWN);
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

    // Variables
    bool pressed = false;
    vector<epicenter> epicenters;
    epicenter tmp;
    int time = 0;
    int delay = 0;
    bool moved = false;

    // main loop
    bool quit = false;
    while (!quit){
        SDL_Event event;
        while(SDL_PollEvent(&event)){
            if (event.type == SDL_QUIT){
                quit = true;
            }

            if (event.type == SDL_MOUSEBUTTONDOWN || event.type == SDL_MOUSEMOTION){
                tmp.x = event.motion.x;
                tmp.y = event.motion.y;
                moved = true;
            }

            if (event.key.keysym.sym == SDLK_k){
                epicenters.clear();
            }
        }
        
        if (time >= delay && moved){
            tmp.dx = 0;
            tmp.dy = 0;
            tmp.time = 1;
            epicenters.push_back(tmp);
            time = 0;
            moved = false;
        }


        SDL_RenderClear(renderer);
        bg(renderer);

        draw(renderer);
        animation_on_press(renderer, epicenters);


        SDL_RenderPresent(renderer);
        if (time <= delay){
            time++;
        }
    }

    TTF_Quit();
    SDL_Quit();
    return 0;
}