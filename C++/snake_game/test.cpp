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

typedef struct myStruct{
    int x;
    int y;
    std::string type;
}myStruct;

void fibonacci_sequence(int size_of_sequence){
    int a = 0;
    int b = 1;
    int c;

    for(int i = 0; i < size_of_sequence; i++){
        c = a + b;
        a = b;
        b = c;
        std::cout << c << " ";
    }
}

void collatz_sequence(int starting_number){
    int number = starting_number;

    while(number != 1){
        if(number % 2 == 0){
            number = number / 2;
        }
    }
}

int main(){

    std::vector<int> myVect = {1, 2, 3, 4, 6, 7, 8};
    std::cout << myVect[myVect.size() - 1];

    std::cout << "\n";

    std::vector<myStruct> Vector_of_Struct = {
        {1, 2, "hello"},
        {5, 3, "this is a test"}
    };

    int my_x = 5;
    int my_y = 3;

    auto lamdba_test = [my_x, my_y](myStruct s){
        return (s.x == my_x &&  s.y == my_y);
    };

    std::cout << std::any_of(Vector_of_Struct.begin(), Vector_of_Struct.end(), lamdba_test);

    std::cout << "\n";    

    return 0;
}