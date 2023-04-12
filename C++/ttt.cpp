// Finished on April 4 2023

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

typedef struct win_coor{
    string player;
    string orientation;
    int coor;
}win_coor;

typedef struct x_y{
    int x;
    int y;
}x_y;

void update_board(SDL_Renderer* renderer, int x, int y, vector<vector<int>> m, int turn, bool& pressed, int dir, bool& enter, win_coor win){
    SDL_RenderClear(renderer);

    // Bg color
    SDL_Rect rect;
    // if presed, to set direction
    if (pressed){
        SDL_SetRenderDrawColor(renderer, 150, 150, 150, 255);
        rect = {0, 0, 700, 750};
        SDL_RenderFillRect(renderer, &rect);
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    }
    else{
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
        rect = {0, 0, 700, 750};
        SDL_RenderFillRect(renderer, &rect);
    }

    // Board
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    SDL_RenderDrawLine(renderer, 250, 50, 250, 650);
    SDL_RenderDrawLine(renderer, 450, 50, 450, 650);
    SDL_RenderDrawLine(renderer, 50, 250, 650, 250);
    SDL_RenderDrawLine(renderer, 50, 450, 650, 450);

    // Draw player mouse mvt
    auto draw_X = [](SDL_Renderer* renderer, int x, int y){
        SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 70, 200 * x + 230, 200 * y + 230);
        SDL_RenderDrawLine(renderer, 200 * x + 230, 200 * y + 70, 200 * x + 70, 200 * y + 230);
    };
    auto draw_O = [](SDL_Renderer* renderer, int x, int y){
        SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 70, 200 * x + 70, 200 * y + 230);
        SDL_RenderDrawLine(renderer, 200 * x + 230, 200 * y + 70, 200 * x + 230, 200 * y + 230);
        SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 70, 200 * x + 230, 200 * y + 70);
        SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 230, 200 * x + 230, 200 * y + 230);
    };

    SDL_SetRenderDrawColor(renderer, 100, 100, 100, 255);
    if (turn == 1 && win.coor == -1){
        draw_X(renderer, x, y);
    }
    else if (turn == 2 && win.coor == -1){
        draw_O(renderer, x, y);
    }
    
    // Draw player - board
    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
    for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
            // Draw X
            if (m[i][j] == 1){
                draw_X(renderer, j, i);
            }
                
            // Draw O
            if (m[i][j] == 2){
                draw_O(renderer, j, i);
            }
        }
    
    }

    // Draw directions
    if (pressed){
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
        switch (dir){
            case 0:
                SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 150, 200 * x + 230, 200 * y + 150);
                SDL_RenderDrawLine(renderer, 200 * x + 230, 200 * y + 150, 200 * x + 200, 200 * y + 120);
                SDL_RenderDrawLine(renderer, 200 * x + 230, 200 * y + 150, 200 * x + 200, 200 * y + 180);
                break;
            case 1:
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 92, 200 * x + 208, 200 * y + 208);
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 208, 200 * x + 162, 200 * y + 208);
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 208, 200 * x + 208, 200 * y + 162);
                break;
            case 2:
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 70, 200 * x + 150, 200 * y + 230);
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 230, 200 * x + 120, 200 * y + 200);
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 230, 200 * x + 180, 200 * y + 200);
                break;
            case 3:
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 92, 200 * x + 92, 200 * y + 208);
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 208, 200 * x + 92, 200 * y + 162);
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 208, 200 * x + 138, 200 * y + 208);
                break;
            case 4:
                SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 150, 200 * x + 230, 200 * y + 150);
                SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 150, 200 * x + 100, 200 * y + 120);
                SDL_RenderDrawLine(renderer, 200 * x + 70, 200 * y + 150, 200 * x + 100, 200 * y + 180);
                break;
            case 5:
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 92, 200 * x + 208, 200 * y + 208);
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 92, 200 * x + 138, 200 * y + 92);
                SDL_RenderDrawLine(renderer, 200 * x + 92, 200 * y + 92, 200 * x + 92, 200 * y + 138);
                break;
            case 6:
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 70, 200 * x + 150, 200 * y + 230);
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 70, 200 * x + 120, 200 * y + 100);
                SDL_RenderDrawLine(renderer, 200 * x + 150, 200 * y + 70, 200 * x + 180, 200 * y + 100);
                break;
            case 7:
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 92, 200 * x + 92, 200 * y + 208);
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 92, 200 * x + 162, 200 * y + 92);
                SDL_RenderDrawLine(renderer, 200 * x + 208, 200 * y + 92, 200 * x + 208, 200 * y + 138);
                break;
        }
    }
    // Draw moving player
    if (enter){
        SDL_SetRenderDrawColor(renderer, 100, 100, 100, 255);
        SDL_Rect square = {200 * x + 60, 200 * y + 60, 180, 180};
        SDL_RenderFillRect(renderer, &square);
        SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
        if (turn == 1){
            draw_X(renderer, x, y);
        }
        else{
            draw_O(renderer, x, y);
        }
    }

    if (win.coor != -1){
        SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);

        if (win.player != "draw"){
            // draw P
            SDL_RenderDrawLine(renderer, 200, 675, 200, 725);
            SDL_RenderDrawLine(renderer, 200, 675, 225, 675);
            SDL_RenderDrawLine(renderer, 200, 700, 225, 700);
            SDL_RenderDrawLine(renderer, 225, 675, 225, 700);

            // Draw W
            SDL_RenderDrawLine(renderer, 320, 675, 335, 725);
            SDL_RenderDrawLine(renderer, 335, 725, 350, 675);
            SDL_RenderDrawLine(renderer, 350, 675, 365, 725);
            SDL_RenderDrawLine(renderer, 365, 725, 380, 675);

            // draw I
            SDL_RenderDrawLine(renderer, 390, 675, 390, 725);

            // draw N
            SDL_RenderDrawLine(renderer, 400, 675, 400, 725);
            SDL_RenderDrawLine(renderer, 400, 675, 425, 725);
            SDL_RenderDrawLine(renderer, 425, 675, 425, 725);

            // draw S
            SDL_RenderDrawLine(renderer, 435, 675, 460, 675);
            SDL_RenderDrawLine(renderer, 435, 700, 460, 700);
            SDL_RenderDrawLine(renderer, 435, 725, 460, 725);
            SDL_RenderDrawLine(renderer, 435, 675, 435, 700);
            SDL_RenderDrawLine(renderer, 460, 700, 460, 725);    
        }
        

        // Draw 1 or 2
        if (win.player == "p1"){
            SDL_RenderDrawLine(renderer, 260, 675, 260, 725);
        }
        else if (win.player == "p2"){
            SDL_RenderDrawLine(renderer, 235, 675, 260, 675);
            SDL_RenderDrawLine(renderer, 235, 700, 260, 700);
            SDL_RenderDrawLine(renderer, 235, 725, 260, 725);
            SDL_RenderDrawLine(renderer, 260, 675, 260, 700);
            SDL_RenderDrawLine(renderer, 235, 700, 235, 725);
        }

        // Draw winning line
        SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
        if (win.orientation == "horizontal"){
            SDL_RenderDrawLine(renderer, 50, 200 * win.coor + 150, 650, 200 * win.coor + 150);
        }
        else if (win.orientation == "vertical"){
            SDL_RenderDrawLine(renderer, 200 * win.coor + 150, 50, 200 * win.coor + 150, 650);
        }
        else if (win.orientation == "diagonal1"){
            SDL_RenderDrawLine(renderer, 50, 50, 650, 650);
        }
        else if (win.orientation == "diagonal2"){
            SDL_RenderDrawLine(renderer, 50, 650, 650, 50);
        }

        SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
        if (win.player == "draw"){
            // Draw D
            SDL_RenderDrawLine(renderer, 250, 675, 250, 725);
            SDL_RenderDrawLine(renderer, 275, 675, 275, 725);
            SDL_RenderDrawLine(renderer, 250, 675, 275, 675);
            SDL_RenderDrawLine(renderer, 250, 725, 275, 725);

            // Draw R
            SDL_RenderDrawLine(renderer, 285, 675, 285, 725);
            SDL_RenderDrawLine(renderer, 285, 675, 310, 675);
            SDL_RenderDrawLine(renderer, 310, 675, 310, 700);
            SDL_RenderDrawLine(renderer, 285, 700, 310, 700);
            SDL_RenderDrawLine(renderer, 285, 700, 310, 725);

            // Draw A
            SDL_RenderDrawLine(renderer, 320, 725, 330, 675);
            SDL_RenderDrawLine(renderer, 340, 725, 330, 675);
            SDL_RenderDrawLine(renderer, 325, 700, 335, 700);

            // Draw W
            SDL_RenderDrawLine(renderer, 345, 675, 360, 725);
            SDL_RenderDrawLine(renderer, 360, 725, 375, 675);
            SDL_RenderDrawLine(renderer, 375, 675, 390, 725);
            SDL_RenderDrawLine(renderer, 390, 725, 405, 675);
        }
    }

    SDL_RenderPresent(renderer);
}

int check_win(vector<vector<int>> m, win_coor& winner){
    vector<int> win;

    auto check_w = [&](vector<int> v){
        if (all_of(v.begin(), v.end(), [](int x){return x == 1;})){
            winner.player = "p1";
            return true;
        }
        else if(all_of(v.begin(), v.end(), [](int x){return x == 2;})){
            winner.player = "p2";
            return true;
        }
        return false;
    };

    for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
            win.push_back(m[i][j]);
        }
        if (check_w(win)){
            winner.coor = i;
            winner.orientation = "horizontal";
            return 1;
        }
        win.clear();
    }

    for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
            win.push_back(m[j][i]);
        }
        if (check_w(win)){
            winner.coor = i;
            winner.orientation = "vertical";
            return 1;
        }
        win.clear();
    }    
    
    for (int i = 0; i < 3; i++){
        win.push_back(m[i][i]);
    }
    if (check_w(win)){
        winner.coor = 1;
        winner.orientation = "diagonal1";
        return 1;
    }
    win.clear();

    for (int i = 0; i < 3; i++){
        win.push_back(m[2 - i][i]);
    }
    if (check_w(win)){
        winner.coor = 1;
        winner.orientation = "diagonal2";
        return 1;
    }
    win.clear();


    auto check = [](vector<int> v){
        return all_of(v.begin(), v.end(), [](int x){return x != 0;});
    };
    if (all_of(m.begin(), m.end(), check)){
        winner.coor = 1;
        winner.player = "draw";
        return 1;
    }

    return 0;
}

int main()
{
    // Board
    vector<vector<int>> map{
        {0, 0, 0},
        {0, 0, 0},
        {0, 0, 0}
    };

    // Variable initialization
    int turn = 1;
    bool pressed = false;
    int dir = 0;
    bool enter = false;
    int win = 0;
    win_coor winner;
    winner.coor = -1;
    winner.player = "NULL";
    winner.orientation = "NULL";
    bool ai = false;
    vector<x_y> xy;

    // SDL
    SDL_Init(SDL_INIT_EVERYTHING);

    // Screan
    SDL_Window* window = SDL_CreateWindow("Tic Tac Toe", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 700, 750, SDL_WINDOW_SHOWN);
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

    // Starting position of mouse
    int rx = 260;
    int ry = 260;
    int i_x;
    int i_y;
    int steps;
    int steps_ai;

    // Main loop
    bool quit = false;
    bool exit = false;
    while(!quit){
        SDL_Event event;
        while(SDL_PollEvent(&event)){
            if(event.type == SDL_QUIT){
                quit = true;
                exit = true;
            }

            // Mouse Motion
            if(event.type == SDL_MOUSEMOTION && event.motion.x < 650 && event.motion.y < 650 && !pressed){
                i_x = floor((event.motion.x - 50) / 200);
                i_y = floor((event.motion.y - 50) / 200);
            }

            if (event.type == SDL_MOUSEBUTTONDOWN && map[i_y][i_x] == 0 && !pressed){
                pressed = true;
            }

            // ai
            if (event.key.keysym.sym == SDLK_a){
                if (event.type == SDL_KEYUP){
                    ai = true;
                    pressed = true;
                    x_y tmp;

                    for (int i = 0; i < 3; i++){
                        for (int j = 0; j < 3; j++){
                            if (map[i][j] == 0){
                                tmp.x = j;
                                tmp.y = i;
                                xy.push_back(tmp);
                            }
                        }
                    }

                    srand((unsigned) time(NULL));
                    steps_ai = rand() % (xy.size() - 1);

                    i_x = xy[steps_ai].x;
                    i_y = xy[steps_ai].y;
                    xy.clear();
                }
            }

            // Direction choice
            if (pressed){
                if (event.key.keysym.sym == SDLK_LEFT && !ai){
                    if (event.type == SDL_KEYUP){
                        dir--;
                    }
                }
                if (event.key.keysym.sym == SDLK_RIGHT && !ai){
                    if (event.type == SDL_KEYUP){
                        dir++;
                    }
                }
                if (ai){
                    srand((unsigned) time(NULL));
                    dir = rand() % 8;
                    update_board(renderer, i_x, i_y, map, turn, pressed, dir, enter, winner);
                    SDL_Delay(1000);
                }

                if (dir < 0){
                    dir = 7;
                }
                if (dir > 7){
                    dir = 0;
                }

                // Direction chosen, display steps
                if (event.key.keysym.sym == SDLK_SPACE || ai){
                    srand((unsigned) time(NULL));
                    steps = rand() % 3 + 1;
                    pressed = false;
                    enter = true;

                    for (; steps > 0; steps--){

                        update_board(renderer, i_x, i_y, map, turn, pressed, dir, enter, winner);
                        SDL_Delay(500);

                        if (dir == 3 || dir == 4 || dir == 5){
                            i_x--;
                            if (i_x < 0){
                                i_x = 2;
                            }
                        }
                        if (dir == 0 || dir == 1 || dir == 7){
                            i_x++;
                            if (i_x > 2){
                                i_x = 0;
                            }
                        }
                        if (dir == 7 || dir == 6 || dir == 5){
                            i_y--;
                            if (i_y < 0){
                                i_y = 2;
                            }
                        }
                        if (dir == 1 || dir == 2 || dir == 3){
                            i_y++;
                            if (i_y > 2){
                                i_y = 0;
                            }
                        }
                    }
                    map[i_y][i_x] = turn;
                    enter = false;

                    if (turn == 1){
                        turn = 2;
                    }
                    else if (turn == 2){
                        turn = 1;
                    }
                    ai = false;
                }
            }
        }

        update_board(renderer, i_x, i_y, map, turn, pressed, dir, enter, winner);
        if (check_win(map, winner) == 1){
            quit = true;
        }
    }

    while (!exit){
        SDL_Event e;
        while (SDL_PollEvent(&e)){
            if (e.type == SDL_QUIT){
                exit = true;
            }
        }
        update_board(renderer, i_x, i_y, map, turn, pressed, dir, enter, winner);
    }


    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();
    return 0;
}
