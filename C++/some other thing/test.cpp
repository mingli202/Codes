#include <iostream>
#include <string>
#include <cmath>
#include <algorithm>
#include <vector>
#include <array>
#include <limits>
#include <random>

double angle(int x1, int y1, int x2, int y2) {
    double ang = atan((y2 - y1) / (x2 - x1));
    if (x2 < x1 && y2 > y1 || x2 < x1 && y2 < y1) {
        ang += M_PI;
    }
    return ang;
}

int main(int argc, char* argv[]){

    std::array<int, 2> p1 = {0, 0};
    std::array<int, 2> p2 = {-3, -3};

    double ratio = (double) 3 / (double) 4;
    double a = angle(p1[0], p1[1], p2[0], p2[1]);
    std::cout << a << "\n";
    std::cout << cos(a) << "\n";
    std::cout << sin(a) << "\n";


    return 0;
}