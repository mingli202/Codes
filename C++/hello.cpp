#include <iostream>
#include <string>
#include <cmath>
#include <array>
#include <algorithm>
#include <vector>
#include <limits>
#include <map>

typedef struct myStruct{
    int a;
    int b;
}myStruct;

void hello(void){
    int x;
    std::cout << "Input your first number: ";  
    std::cin >> x;

    int y;
    std::cout << "Input your second number: ";
    std::cin >>  y;

    int choice;
    std::cout << "Choose what to do with your numbers:\n";
    std::cout << "1.Addition\n2.Substraction\n3.Multiplication\n4.Division\n";
    std::cout << "5.Modulus\n6.Find max\n7.Find min\n";
    std::cin >> choice;

    if (choice == 1){
        std::cout << x << " + " << y << " = " << x + y;
    }
    else if (choice == 2){
        std::cout << x << " - " << y << " = " << x - y;
    }
    else if (choice == 3){
        std::cout << x << " x " << y << " = " << x * y;
    }
    else if (choice == 4){
        if (y == 0){
            std::cout << "Division by zero is undefined.";
        }
        else{
            std::cout << x << " / " << y << " = " << x / y;
        }
    }
    else if (choice == 5){
        std::cout << x << " mod " << y << " = " << x % y;
    }
    else if (choice == 6 || choice == 7){
        if (x == y){
            std::cout << "Both numbers are equal";
        }
        if (choice == 6){
            std::cout << "The maximum is " << std::max(x, y);
        }
        else{
            std::cout << "The minimum is " << std::min(x, y);
        }
    }
    else{
        std::cout << "PLease type a number";
    }
}

void ternary_operator(void){
    int k;
    std::cout << "k: ";
    std::cin >> k;
    std::string l = (k < 10) ? "k is smaller than 10" : "k is bigger than 10";
    std::cout << l;
}

void the_switch_operator(void){
    int a;
    std::cout << "Enter the day of the week: ";
    std::cin >> a;

    switch(a){
        case 1:
            std::cout << "Monday";
            break;
        case 2: 
            std::cout << "Tuesday";
            break;
        case 3:
            std::cout << "Wednesday";
            break;
        case 4:
            std::cout << "Thursday";
            break;
        case 5:
            std::cout << "Friday";
            break;
        case 6:
            std::cout << "Saterday";
            break;
        case 7:
            std::cout << "Sunday";
            break;
        default:
            std::cout << "There is no match";
            break;
    }
}

void sum_of_primes(void){
    int list[] = {3, 5, 3, 6, 2, 5, 5, 6, 7};
    for (int i = 0; i < 0;){
        std::cout << i;
    }
}

void for_each_function(void){
    std::string arr[] = {"Hello", "I", "Am", "Gary"};
    
    for (std::string i : arr){
        std::cout << i << std::endl;
    }
}

void TwoD_array(void){
    int k[2][6] = {
        {1, 2, 3, 4, 5, 6},
        {1, 2, 3, 4, 5, 6}
    };

    for (int i = 0; i < 6; i++){
        std::cout << k[0][i] << std::endl;
    }
}

void average(void){
    int score;
    std::cout << "What are your scores? ";
    int num_of_scores = 0;
    int total = 0;
    while(std::cin >> score){
        std::cout << "(Enter a letter to stop) What are your scores? ";
        total = total + score;
        num_of_scores++;
    }
    float avg = (float)total/num_of_scores;
    std::cout << "Your average is: " << avg;
}

void wtf(void){
    std::array <int, 6> myArray = {1, 2, 3, 4, 5, 0};
    for (const auto& i : myArray){
        std::cout << i << " ";
    }

    // lambda shit
    auto check = [](int x){
        return (x == 0);
    };
    // algorithm shit
    if (std::any_of(myArray.begin(), myArray.end(), check)){
        std::cout << "There is at least one zero\n";
    }
    else{
        std::cout << "There is no zero\n";
    }

}

void vector_shit(void){
    std::vector<int> myVector = {5432, 3456, 6543};
    myVector.push_back(12345672);
    myVector.push_back(50);
    std::cout << myVector[0] << std::endl;
    std::cout << myVector.size() << std::endl << myVector.capacity();

    for (int i : myVector){
        std::cout << i << std::endl;
    }

    std::vector<std::vector<int>> the_vector{
        {1, 2, 3, 4, 5},
        {2, 3, 4, 5, 6}
    };

    std::cout << std::endl;

    std::array<std::array<int, 5>, 5> myArray;
    
    for (std::array<int, 5> i : myArray){
        for (int j : i){
            std::cout << j << " ";
        }
    }
    std::cout << std::endl;

    for (std::vector<int> i : the_vector){
        for (int j : i){
            std::cout << j << " ";
        }
    }

    std::cout << "Enter to continue";
    std::cin.ignore(std::numeric_limits<std::streamsize>::max(),'\n');

    std::vector<std::vector<int>> aVector{
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 9}
    };
    std::cout << aVector[0][-1];


    std::vector<myStruct> p;

    myStruct tmp;

    auto print_vector = [](std::vector<myStruct> p){
        for (myStruct i : p){
            std::cout << i.a << i.b << " ";
        }
        std::cout << std::endl;
    };


    for (int i = 1; i < 9; i++){
        tmp.a = i;
        tmp.b = i;
        p.push_back(tmp);
    }

    print_vector(p);

    p.erase(p.begin());

    print_vector(p);
    std::cout << p[0].a;

    tmp.a = 123;
    tmp.b = 456;
    p.push_back(tmp);

    print_vector(p);


    std::cout << std::endl;
}

void map_shit(){
    std::map<std::string, int> myMap = {
        {"one", 1},
        {"three", 3}
    };
    myMap["two"] = 2;
    std::cout << myMap["two"] << "\n";
    std::cout << myMap["one"] << "\n";
    std::cout << myMap["three"] << "\n";

    std::map<std::string, myStruct> map2 = {
        {"Point A", {100, 4}}
    };

    std::cout << map2["Point A"].a;
}

typedef struct{
    int food;
    int water;
}storage;

typedef struct{
    int stamina;
    int hydration;
    storage provision;
}player_stats;

// main
int main(){

    std::map<std::string, player_stats> players{
        {"Player 1", {
            100, 100, {10, 20}
        }},
        {"Player 2", {
            100, 100, {10, 20}
        }}
    };

    std::cout << players["Player 1"].provision.food << "\n";
    players["Player 1"].provision.food -= 5;
    std::cout << players["Player 1"].provision.food << "\n";

    std::cout << "\n";
}