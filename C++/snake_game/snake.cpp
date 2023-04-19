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

typedef struct Food{
    int x;
    int y;
    std::string type;
}Food;

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

    std::vector<info> snake = {};
    info tmp;

    int red = 255;
    int green = 255;
    int blue = 255;

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
        SDL_SetRenderDrawColor(renderer, red, green, blue, 255);

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

    void on_food_collision(std::vector<info>& snake, std::vector<Food>& food){

        for (Food& i : food){
            if (snake[0].x == i.x && snake[0].y == i.y){
                int LAST = snake.size() - 1;

                srand((unsigned) time(NULL));
                i.x = rand() % 35 * 20;
                i.y = rand() % 35 * 20;

                tmp.x = snake[LAST].x_previous;
                tmp.y = snake[LAST].y_previous;
                snake.push_back(tmp);

                break;
            }
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

    int player_collision_on_other(std::vector<info>& p1_snake, std::vector<info>& p2_snake){
        if (p1_snake[0].x == p2_snake[0].x && p1_snake[0].y == p2_snake[0].y){
            return 2;
        }

        for (int i = 0; i < p2_snake.size() - 1; i++){
            if (p1_snake[0].x == p2_snake[i].x_previous && p1_snake[0].y == p2_snake[i].y_previous){
                return 1;
            }
        }
        return 0;
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

        surface = TTF_RenderText_Solid(font, score.c_str(), WHITE); // white text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        int texW = 0;
        int texH = 0;
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        SDL_Rect dstrect = {10, 10, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }

    void display_second_player_score(SDL_Renderer* renderer, TTF_Font* font, SDL_Surface* surface, SDL_Texture* texture, std::vector<info>& snake){
        std::string score = "Score: " + std::to_string(snake.size() - 1);

        surface = TTF_RenderText_Solid(font, score.c_str(), WHITE); // white text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        int texW = 0;
        int texH = 0;
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        SDL_Rect dstrect = {600, 10, texW, texH};
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
        int center_x = (700 - texW) / 2;
        int center_y = (700 - texH) / 2;
        SDL_Rect dstrect = {center_x, center_y, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }

    void display_winner_screen(SDL_Renderer* renderer, TTF_Font* font, SDL_Surface* surface, SDL_Texture* texture, int win){
        std::string winner;
        std::string win_message;
        if (win == -1){
            win_message = "Draw";
        }
        else{
            winner = (win == 1) ? "Green" : "Blue";
            win_message = winner + " wins!";
        }
        int texW = 0;
        int texH = 0;
        int center_x;
        int center_y;
        SDL_Rect dstrect;

        surface = TTF_RenderText_Solid(font, win_message.c_str(), BLACK); // black text, winner
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        center_x = (700 - texW) / 2;
        center_y = (700 - texH) / 2;
        dstrect = {center_x, center_y, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);

        surface = TTF_RenderText_Solid(font, "Press Space to play again", BLACK); // black text
        texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        SDL_QueryTexture(texture, NULL, NULL, &texW, &texH);
        center_x = (700 - texW) / 2;
        center_y = (700 + texH) / 2;
        dstrect = {center_x, center_y, texW, texH};
        SDL_RenderCopy(renderer, texture, NULL, &dstrect);
    }
};


void bg(SDL_Renderer* renderer, const int bg_color){
    const int WINDOW_SIZE = 700;
    SDL_SetRenderDrawColor(renderer, bg_color, bg_color, bg_color, 255); // black
    SDL_Rect backgound = {0, 0, WINDOW_SIZE, WINDOW_SIZE};
    SDL_RenderFillRect(renderer, &backgound);
}

void draw_food(SDL_Renderer* renderer, const std::vector<Food> food){
    // food
    SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255); // red
    for (Food i : food){
        SDL_Rect food_rect = {i.x, i.y, 20, 20};
        SDL_RenderFillRect(renderer, &food_rect);
    }
}

void pause_screen(int& progress, int& previous){
    bool unpause = false;
    while (!unpause){
        SDL_Event Pause;
        while (SDL_PollEvent(&Pause)){
            switch(Pause.type){
                case (SDL_KEYDOWN):
                    if (Pause.key.keysym.sym == SDLK_ESCAPE){
                        progress = previous;
                        unpause = true;
                        break;
                    }
            }
        }
    }
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
    int win = 0;
    int previous;

    // food
    srand((unsigned) time(NULL));
    std::vector<Food> food;

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

                        p1.tmp = {340, 340, 0, 0, 0, 0};
                        p1.snake.push_back(p1.tmp);
                        bg_color = 0;

                        food = {{rand() % 35 * 20, rand() % 35 * 20}};
                    }
                    else if (p2_text.color.r == p2_text.RED.r && progress == 0){
                        progress = 2;

                        p1.red = 200;
                        p1.blue = 200;
                        p1.tmp = {480, 340, 0, 0, 0, 0};
                        p1.snake.push_back(p1.tmp);

                        p2.red = 200;
                        p2.green = 200;
                        p2.tmp = {200, 340, 0, 0, 0, 0};
                        p2.snake.push_back(p2.tmp);
                        bg_color = 0;

                        food = {{rand() % 35 * 20, rand() % 35 * 20}, {rand() % 35 * 20, rand() % 35 * 20}};
        
                    }
                    break;
                
                case (SDL_KEYDOWN):
                    if (event.key.keysym.sym == SDLK_ESCAPE && progress != 3){
                        previous = progress;
                        progress = 3;
                        break;
                    }

                    if (progress == 1 || progress == 2){
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

                    if (progress == 2){
                        if (event.key.keysym.sym == SDLK_a){
                            p2.snake[0].dx = -1;
                            p2.snake[0].dy = 0;
                            break;
                        }
                        else if (event.key.keysym.sym == SDLK_d){
                            p2.snake[0].dx = 1;
                            p2.snake[0].dy = 0; 
                            break;
                        }
                        else if (event.key.keysym.sym == SDLK_w){
                            p2.snake[0].dx = 0;
                            p2.snake[0].dy = -1;
                            break;
                        }
                        else if (event.key.keysym.sym == SDLK_s){
                            p2.snake[0].dx = 0;
                            p2.snake[0].dy = 1;
                            break;
                        }
                    }
                        
                    if (progress == -1 || progress == -2){
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
                
            // single player
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

            // two players
            case 2:
                p1.display_player(renderer, p1.snake);
                p1.on_food_collision(p1.snake, food);

                p2.display_player(renderer, p2.snake);
                p2.on_food_collision(p2.snake, food);
                
                draw_food(renderer, food);

                // if (p1.on_self_collision(p1.snake)){
                //     progress = -2;
                //     bg_color = 255;
                //     win = 2;
                //     break;
                // }
                // if (p2.on_self_collision(p2.snake)){
                //     progress = -2;
                //     bg_color = 255;
                //     win = 1;
                //     break;
                // }

                // Draw
                if (p1.player_collision_on_other(p1.snake, p2.snake) == 2){
                    progress = -2;
                    bg_color = 255;
                    win = -1;
                    break;
                }

                // p2 wins
                if (p1.player_collision_on_other(p1.snake, p2.snake) == 1){
                    progress = -2;
                    bg_color = 255;
                    win = 2;
                    break;
                }

                // p1 wins
                if (p2.player_collision_on_other(p2.snake, p1.snake) == 1){
                    progress = -2;
                    bg_color = 255;
                    win = 1;
                    break;
                }


                break;

            case -1:
                p1_text.display_death_screen(renderer, font, surface, texture, p1.snake);
                break;

            case -2:
                p1_text.display_winner_screen(renderer, font, surface, texture, win);
                break;

            // pause screen
            case 3:
                pause_screen(progress, previous);
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