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

typedef struct mystruct{
    int x;
    int y;
}mystruct;

int main(){

    // vector<vector<int>> map = {
    //     {0, 0, 0},
    //     {0, 0, 0},
    //     {0, 0, 0}
    // };

    // vector<mystruct> hello;
    // mystruct tmp;

    // for (int i = 0; i < 3; i++){
    //     for (int j = 0; j < 3; j++){
    //         if (map[i][j] == 0){
    //             tmp.x = j;
    //             tmp.y = i;
    //             hello.push_back(tmp);
    //         }
    //     }
    // }
    // srand((unsigned) time(NULL));

    // int i = rand() % (hello.size() - 1);
    // cout << hello[i].x << endl << hello[i].y;

    // vector<mystruct> hel;
    // mystruct tmp;
    // tmp.x = 10;
    // tmp.y = 40;
    // hel.push_back(tmp);

    // for (mystruct i : hel){
    //     cout << i.x << " " << i.y << endl;
    // }

    // for (mystruct& j : hel){
    //     j.x += 10;
    //     j.y += 20;
    // }

    // for (mystruct i : hel){
    //     cout << i.x << " " << i.y << endl;
    // }

    SDL_Init(SDL_INIT_VIDEO);
    TTF_Init();

    SDL_Window* window = SDL_CreateWindow("SDL TTF Example", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 640, 480, 0);
    SDL_Renderer* renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED);


    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    SDL_Rect backgound = {0, 0, 700, 700};
    SDL_RenderFillRect(renderer, &backgound);


    TTF_Font* font = TTF_OpenFont("/Users/vincentliu/Codes/open-sans/OpenSans-Bold.ttf", 24);
    SDL_Surface* surface = TTF_RenderText_Solid(font, "hello", {255, 255, 255, 255}); // white text
    SDL_Texture* texture = SDL_CreateTextureFromSurface(renderer, surface);
    SDL_FreeSurface(surface);
    int texW = 0;
    int texH = 0;
    SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
    SDL_Rect dstrect = {10, 10, texW, texH};
    SDL_RenderCopy(renderer, texture, NULL, &dstrect);




    SDL_RenderPresent(renderer);

    bool quit = false;
    while (!quit){
        SDL_Event e;
        while (SDL_PollEvent(&e)){
            if (e.type == SDL_QUIT){
                quit = true;
            }
        }
    }

    // SDL_DestroyTexture(texture);
    // SDL_FreeSurface(surface);
    TTF_CloseFont(font);
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);

    TTF_Quit();
    SDL_Quit();
    return 0;

}
