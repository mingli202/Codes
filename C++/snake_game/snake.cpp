// Improved snake game with variations and multiplayers

// Created on April 10, 2023
// In-progress

#include <iostream>
#include <string>
#include <cmath>
#include <algorithm>
#include <vector>
#include <array>
#include <limits>
#include <random>
#include <SDL2/SDL.h>
#include <SDL2_ttf/SDL_ttf.h>

class Player{
    public:
    typedef struct info{
        int x;
        int y;
        int x_previous;
        int y_previous;
        int dx;
        int dy;
    }info;

    std::vector<info> snake = {{340, 340, 0, 0, 0, 0}};
    info tmp;

    int RECT_SIZE = 20;


    void display_player(SDL_Renderer* renderer, std::vector<info>& snake){
        snake[0].x_previous = snake[0].x;
        snake[0].y_previous = snake[0].y;
        snake[0].x += RECT_SIZE * snake[0].dx;
        snake[0].y += RECT_SIZE * snake[0].dy;

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

        SDL_Rect rect;
        SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

        rect = {snake[0].x, snake[0].y, RECT_SIZE, RECT_SIZE};
        SDL_RenderFillRect(renderer, &rect);

        for (int i = 1; i < snake.size(); i++){
            snake[i].x_previous = snake[i].x;
            snake[i].y_previous = snake[i].y;
            snake[i].x = snake[i - 1].x_previous;
            snake[i].y = snake[i - 1].y_previous;
            rect = {snake[i].x, snake[i].y, RECT_SIZE, RECT_SIZE};
            SDL_RenderFillRect(renderer, &rect);
        }
    }

    void on_food_collision(std::vector<info>& snake, SDL_Point& food){

        if (snake[0].x == food.x && snake[0].y == food.y){
            srand((unsigned) time(NULL));
            food.x = rand() % 35 * 20;
            food.y = rand() % 35 * 20;

            tmp.x = snake[-1].x_previous;
            tmp.y = snake[-1].y_previous;
            snake.push_back(tmp);
        }
    }

    bool on_self_collision(std::vector<info>& snake){
        for (int i = 1; i < snake.size() - 1; i++){
            if (snake[0].x == snake[i].x_previous && snake[0].y == snake[i].y_previous){
                return true;
            }
        }
        return false;
    }

};

class Text : public Player{
    public:
    const SDL_Color BLACK = {0, 0, 0, 255}; // black
    const SDL_Color WHITE = {255, 255, 255, 255}; // white
    const SDL_Color RED = {255, 0, 0, 255}; // red
    std::array<int, 2> p_coordinates = {300, 340};
    SDL_Color color = BLACK;

    void display_player_text(SDL_Renderer* renderer, TTF_Font* font, SDL_Surface* surface, SDL_Texture* texture, SDL_Point MyMouse, int player){
        int texW;
        int texH;
        SDL_Rect dstrect;
        std::string text;

        switch (player){
            case 0:
                text = std::to_string(player + 1) + " Player";
                break;
            case 1:
                text = std::to_string(player + 1) + " Players";
                break;
        }
        
        surface = TTF_RenderText_Solid(font, text.c_str(), color); // black text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        texW = 0;
        texH = 0;
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        dstrect = {300, p_coordinates[player], texW, texH};

        if (SDL_PointInRect(&MyMouse, &dstrect)){
            color = RED;
        }
        else{
            color = BLACK;
        }

        surface = TTF_RenderText_Solid(font, text.c_str(), color); // black text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);

        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }

    void display_score(SDL_Renderer* renderer, TTF_Font* font, SDL_Surface* surface, SDL_Texture* texture, std::vector<info>& snake){
        std::string score = "Score: " + std::to_string(snake.size() - 1);

        surface = TTF_RenderText_Blended_Wrapped(font, score.c_str(), WHITE, 0); // white text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        int texW = 0;
        int texH = 0;
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        SDL_Rect dstrect = {10, 10, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }

    void display_death_screen(SDL_Renderer* renderer, TTF_Font* font, SDL_Surface* surface, SDL_Texture* texture, std::vector<info>& snake){
        std::string death_message = "Score: " + std::to_string(snake.size() - 1) + "\nPress Space to play again";

        surface = TTF_RenderText_Blended_Wrapped(font, death_message.c_str(), BLACK, 0); // black text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        int texW = 0;
        int texH = 0;
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        SDL_Rect dstrect = {200, 300, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }
};


void bg(SDL_Renderer* renderer, int bg_color){
    const int WINDOW_SIZE = 700;
    SDL_SetRenderDrawColor(renderer, bg_color, bg_color, bg_color, 255); // black
    SDL_Rect backgound = {0, 0, WINDOW_SIZE, WINDOW_SIZE};
    SDL_RenderFillRect(renderer, &backgound);
}

void draw_food(SDL_Renderer* renderer, SDL_Point food){
    // food
    SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255); // red
    SDL_Rect food_rect = {food.x, food.y, 20, 20};
    SDL_RenderFillRect(renderer, &food_rect);
}


int main(){
    SDL_Init(SDL_INIT_EVERYTHING);
    TTF_Init();

    // Screen
    SDL_Window* window = SDL_CreateWindow("Snake Game", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 700, 700, SDL_WINDOW_SHOWN);
    if (window == NULL){
        std::cout << "No window" << SDL_GetError();
    }

    // Renderer
    SDL_Renderer* renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
    if (renderer == NULL){
        std::cout << "No renderer" << SDL_GetError();
    }

    // text
    TTF_Font* font = TTF_OpenFont("/Users/vincentliu/Codes/open-sans/OpenSans-Bold.ttf", 24);
    SDL_Surface* surface;
    SDL_Texture* texture;

    Text p1_text;
    Text p2_text;

    // Players
    Player p1;
    Player p2;

    // variables
    int bg_color = 255; // white
    int progress = 0;
    SDL_Point MyMouse;

    // food
    srand((unsigned) time(NULL));
    SDL_Point food = {rand() % 35 * 20, rand() % 35 * 20};

    // main loop
    bool quit = false;
    while (!quit){
        
        // Events
        SDL_Event event;
        while (SDL_PollEvent(&event)){
            switch (event.type){
                case (SDL_QUIT):
                    quit = true;
                    break;
                
                case (SDL_MOUSEMOTION):
                    MyMouse.x = event.motion.x;
                    MyMouse.y = event.motion.y;
                    break;
                
                case (SDL_MOUSEBUTTONDOWN):
                    if (p1_text.color.r == p1_text.RED.r && progress == 0){
                        progress = 1;
                    }
                    else if (p2_text.color.r == p2_text.RED.r && progress == 0){
                        progress = 2;
                    }
                    bg_color = 0;
                    break;
                
                case (SDL_KEYDOWN):
                    if (progress == 1){
                        if (event.key.keysym.sym == SDLK_LEFT){
                            p1.snake[0].dx = -1;
                            p1.snake[0].dy = 0;
                            break;
                        }
                        else if (event.key.keysym.sym == SDLK_RIGHT){
                            p1.snake[0].dx = 1;
                            p1.snake[0].dy = 0; 
                            break;  
                        }
                        else if (event.key.keysym.sym == SDLK_UP){
                            p1.snake[0].dx = 0;
                            p1.snake[0].dy = -1;
                            break;
                        }
                        else if (event.key.keysym.sym == SDLK_DOWN){
                            p1.snake[0].dx = 0;
                            p1.snake[0].dy = 1;
                            break;
                        }
                    }
                        
                    if (progress == -1){
                        if (event.key.keysym.sym == SDLK_SPACE){
                            TTF_CloseFont(font);
                            SDL_DestroyTexture(texture);
                            TTF_Quit();
                            SDL_DestroyRenderer(renderer);
                            SDL_DestroyWindow(window);
                            SDL_Quit();

                            main();
                            return 0;
                        }
                    }
                    break;
            }
        }

        // Display
        SDL_RenderClear(renderer);
        bg(renderer, bg_color);

        switch (progress){
            case 0:
                p1_text.display_player_text(renderer, font, surface, texture, MyMouse, 0);
                p2_text.display_player_text(renderer, font, surface, texture, MyMouse, 1);
                break;
                
            case 1:
                p1_text.display_score(renderer, font, surface, texture, p1.snake);
                p1.display_player(renderer, p1.snake);
                p1.on_food_collision(p1.snake, food);
                draw_food(renderer, food);

                if(p1.on_self_collision(p1.snake)){
                    progress = -1;
                    bg_color = 255;
                }
                break;

            case 2:
                break;
            
            case -1:
                p1_text.display_death_screen(renderer, font, surface, texture, p1.snake);
                break;

        }

        SDL_RenderPresent(renderer);
        SDL_Delay(50);
    }


    TTF_CloseFont(font);
    SDL_DestroyTexture(texture);
    TTF_Quit();
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();
    return 0;
}