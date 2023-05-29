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

const int WINDOW_WIDTH = 800;
const int WINDOW_HEIGHT = 600;

typedef struct {
    double xi;
    double yi;
    double xf;
    double yf;
    double dx;
    double dy;
} data;

//colors
const SDL_Color WHITE = {255, 255, 255, 255};
const SDL_Color BLACK = {0, 0, 0, 255};
const SDL_Color RED = {255, 0, 0, 255};


void background(SDL_Renderer* renderer){
    SDL_SetRenderDrawColor(renderer, BLACK.r, BLACK.g, BLACK.b, BLACK.a);
    SDL_Rect rect = {0, 0, WINDOW_WIDTH, WINDOW_HEIGHT};
    SDL_RenderFillRect(renderer, &rect);
}

void drawShape(SDL_Renderer* renderer, double x, double y, int radius, SDL_Color color, std::string shape) {
    SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a);

    // circle
    if (shape == "circle") {
        for (int i = -radius; i <= radius; i += 3) {
            for (int j = -radius; j <= radius; j += 3) {
                if (i * i + j * j <= radius * radius) {
                    SDL_RenderDrawPoint(renderer, x + i, y + j);
                }
            }
        }
    }

    // square
    else if (shape == "square") {
        SDL_Rect square = {(int) x - radius, (int) y - radius, 2 * radius, 2 * radius};
        SDL_RenderFillRect(renderer, &square);
    }
}

double distance(double x1, double y1, double x2, double y2) {
    return sqrt(pow(x2 - x1, 2) + pow(y2 - y1, 2));
}

void collision_with_others(SDL_Renderer* renderer, data& point, std::vector<data>& points, int radius) {
    SDL_SetRenderDrawColor(renderer, RED.r, RED.g, RED.b, RED.a);
    for (data& p : points) {
        SDL_RenderDrawLine(renderer, p.xi, p.yi, point.xi, point.yi);
        // check if same point
        if (p.xi == point.xi && p.yi == point.yi) {
            continue;
        }
        // check if collision
        if (distance(p.xi, p.yi, point.xi, point.yi) <= 2 * radius) {
            double tmpx = point.dx;
            double tmpy = point.dy;
            point.dx += p.dx - point.dx;
            point.dy += p.dy - point.dy;
            p.dx += tmpx - p.dx;
            p.dy += tmpy - p.dy;
        }
    }
}

int main() {

    SDL_Init(SDL_INIT_EVERYTHING);

    SDL_Window* window = SDL_CreateWindow("Physics Engine", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WINDOW_SHOWN);
    if (window == nullptr) {
        std::cout << "Failed to create window" << SDL_GetError() << "\n";
        return 1;
    }

    SDL_Renderer* renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
    if (renderer == nullptr) {
        std::cout << "Failed to create renderer" << SDL_GetError() << "\n";
        return 1;
    }

    // variables
    data mousePos;
    std::vector<data> points;
    bool pressed = false;
    const int RADIUS = 25;
    std::string shape = "square";


    //main loop
    bool running = true;
    while (running) {
        SDL_RenderClear(renderer);
        background(renderer);

        SDL_Event event;

        while (SDL_PollEvent(&event)) {
            if (event.type == SDL_QUIT) {
                running = false;
            }

            if (event.key.keysym.sym == SDLK_k) {
                points.clear();
            }

            else if (event.motion.state == SDL_PRESSED) {
                mousePos.xf = event.motion.x;
                mousePos.yf = event.motion.y;
                pressed = true;
            }
            else if (pressed == true) {
                mousePos.dx = - (mousePos.xf - mousePos.xi) / (double) 50;
                mousePos.dy = - (mousePos.yf - mousePos.yi) / (double) 50;
                points.push_back(mousePos);
                pressed = false;
            }

            else if (event.type == SDL_MOUSEMOTION) {
                mousePos.xi = event.motion.x;
                mousePos.yi = event.motion.y;
            }

        }

        if (pressed) {
            drawShape(renderer, mousePos.xi, mousePos.yi, RADIUS, WHITE, shape);
            SDL_SetRenderDrawColor(renderer, RED.r, RED.g, RED.b, RED.a);
            SDL_RenderDrawLine(renderer, mousePos.xi, mousePos.yi, mousePos.xf, mousePos.yf);
        }

        for (data& point : points) {
            drawShape(renderer, point.xi, point.yi, RADIUS, WHITE, shape);

            point.xi += point.dx;
            point.yi += point.dy;

            //check for collision
            if (point.xi + RADIUS > WINDOW_WIDTH || point.xi - RADIUS < 0) {
                point.dx = -point.dx;
            }
            if (point.yi + RADIUS > WINDOW_HEIGHT || point.yi - RADIUS < 0) {
                point.dy = -point.dy;
            }

            // check for collision with other balls
            collision_with_others(renderer, point, points, RADIUS);
            
        }

        SDL_RenderPresent(renderer);
    }

    //clean up 
    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}