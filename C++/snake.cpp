// Finished on April 4 2023
// Update April 11 2023: optimized key press

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

// some constant
const int RECT_SIZE = 20;

typedef struct info{
    int x;
    int y;
    int x_p;
    int y_p;
    int vx;
    int vy;
}info;

void bg(SDL_Renderer* renderer){
    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    SDL_Rect backgound = {0, 0, 700, 700};
    SDL_RenderFillRect(renderer, &backgound);
}

void display_player(SDL_Renderer* renderer, vector<info>& snake){
    SDL_Rect rect;
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

    rect = {snake[0].x, snake[0].y, RECT_SIZE, RECT_SIZE};
    SDL_RenderFillRect(renderer, &rect);

    for (int i = 1; i < snake.size(); i++){
        snake[i].x_p = snake[i].x;
        snake[i].y_p = snake[i].y;
        snake[i].x = snake[i - 1].x_p;
        snake[i].y = snake[i - 1].y_p;
        rect = {snake[i].x, snake[i].y, RECT_SIZE, RECT_SIZE};
        SDL_RenderFillRect(renderer, &rect);
    }
}

void draw_food(SDL_Renderer* renderer, int food_x, int food_y){
    // food
    SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
    SDL_Rect food = {food_x, food_y, RECT_SIZE, RECT_SIZE};
    SDL_RenderFillRect(renderer, &food);
}

bool check_collision(vector<info> snake){
    for (int i = 1; i < snake.size() - 1; i++){
        if (snake[0].x == snake[i].x_p && snake[0].y == snake[i].y_p){
            return true;
        }
    }
    return false;
}

void display_score(SDL_Renderer* renderer, TTF_Font* font, SDL_Texture* texture){
    int texW = 0;
    int texH = 0;
    SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
    SDL_Rect dstrect = {10, 10, texW, texH};
    SDL_RenderCopy(renderer, texture, NULL, &dstrect);
}

void display_death_screen(SDL_Renderer* renderer, TTF_Font* font, SDL_Texture* texture){
    int texW = 0;
    int texH = 0;
    SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
    SDL_Rect dstrect = {200, 300, texW, texH};
    SDL_RenderCopy(renderer, texture, NULL, &dstrect);
}

int main(){

    // SDL init
    SDL_Init(SDL_INIT_EVERYTHING);
    TTF_Init();
    
    // Screen
    SDL_Window* window = SDL_CreateWindow("Snake Game", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 700, 700, SDL_WINDOW_SHOWN);
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

    // font
    TTF_Font* font = TTF_OpenFont("/Users/vincentliu/Codes/open-sans/OpenSans-Bold.ttf", 24);
    
    SDL_Surface* surface = TTF_RenderText_Solid(font, "Score: 0", {255, 255, 255, 255}); // white text
    SDL_Texture* texture = SDL_CreateTextureFromSurface(renderer, surface);
    SDL_FreeSurface(surface);

    // Variables
    info head;
    head.x = 340;
    head.y = 340;
    head.vx = 0;
    head.vy = 0;

    int size = 0;
    string score = "Score: " + to_string(size);
    string death_msg;

    vector<info> snake;
    snake.clear();
    snake.push_back(head);
    info tmp;

    // food
    srand((unsigned) time(NULL));
    int food_x = rand() % 35 * 20;
    int food_y = rand() % 35 * 20;


    // Main loop
    bool quit = false;
    while (!quit){
        SDL_RenderClear(renderer);
        bg(renderer);

        display_score(renderer, font, texture);

        // input
        SDL_Event e;
        while (SDL_PollEvent(&e)){
            switch (e.type){
                case (SDL_QUIT):
                    quit = true;
                    break;
                case (SDL_KEYDOWN):
                    if (e.key.keysym.sym == SDLK_LEFT || e.key.keysym.sym == SDLK_a){
                        snake[0].vx = -1;
                        snake[0].vy = 0;
                        break;
                    }
                    else if (e.key.keysym.sym == SDLK_RIGHT || e.key.keysym.sym == SDLK_d){
                        snake[0].vx = 1;
                        snake[0].vy = 0; 
                        break;  
                    }
                    else if (e.key.keysym.sym == SDLK_UP || e.key.keysym.sym == SDLK_w){
                        snake[0].vx = 0;
                        snake[0].vy = -1;
                        break;
                    }
                    else if (e.key.keysym.sym == SDLK_DOWN || e.key.keysym.sym == SDLK_s){
                        snake[0].vx = 0;
                        snake[0].vy = 1;
                        break;
                    }
            }
        }


        snake[0].x_p = snake[0].x;
        snake[0].y_p = snake[0].y;
        snake[0].x += RECT_SIZE * snake[0].vx;
        snake[0].y += RECT_SIZE * snake[0].vy;
        if (snake[0].x < 0){
            snake[0].x = 680;
        }
        if (snake[0].x > 680){
            snake[0].x = 0;
        }
        if (snake[0].y < 0){
            snake[0].y = 680;
        }
        if (snake[0].y > 680){
            snake[0].y = 0;
        }

        // food
        if (snake[0].x == food_x && snake[0].y == food_y){
            srand((unsigned) time(NULL));
            food_x = rand() % 35 * 20;
            food_y = rand() % 35 * 20;

            tmp.x = snake[size].x_p;
            tmp.y = snake[size].y_p;
            snake.push_back(tmp);
            size++;

            // score update
            score = "Score: " + to_string(size);
            surface = TTF_RenderText_Solid(font, score.c_str(), {255, 255, 255, 255}); // white text
            texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_FreeSurface(surface);
        }

        display_player(renderer, snake);
        draw_food(renderer, food_x, food_y);
        SDL_RenderPresent(renderer);
        SDL_Delay(50);

        if (check_collision(snake)){
            bool quit2 = false;
            death_msg = score.append("\nPress Space to play again");
            while (!quit2){
                SDL_RenderClear(renderer);
                SDL_Event e2;
                while (SDL_PollEvent(&e2)){
                    if (e2.type == SDL_QUIT){
                        SDL_DestroyRenderer(renderer);
                        SDL_DestroyWindow(window);
                        SDL_Quit();
                        TTF_CloseFont(font);
                        SDL_DestroyTexture(texture);
                        TTF_Quit();
                        return 0;
                    }
                    // Play again
                    if (e2.key.keysym.sym == SDLK_SPACE){
                        SDL_DestroyRenderer(renderer);
                        SDL_DestroyWindow(window);
                        SDL_Quit();
                        TTF_CloseFont(font);
                        SDL_DestroyTexture(texture);
                        TTF_Quit();
                        main();
                        return 0;
                    }
                }

                SDL_Rect death;
                SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                death = {0, 0, 700, 700};
                SDL_RenderFillRect(renderer, &death);

                // Display death message
                surface = TTF_RenderText_Blended_Wrapped(font, death_msg.c_str(), {0, 0, 0, 255}, 0); // white text
                texture = SDL_CreateTextureFromSurface(renderer, surface);
                SDL_FreeSurface(surface);
                display_death_screen(renderer, font, texture);

                SDL_RenderPresent(renderer);
            }
        }
    }
    
    TTF_CloseFont(font);
    SDL_DestroyTexture(texture);
    TTF_Quit();
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();
    return 0;
}
