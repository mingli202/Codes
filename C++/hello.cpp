#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <array>
#include <algorithm>
#include <vector>
#include <limits>

void hello(void){
    int x;
    cout << "Input your first number: ";  
    cin >> x;

    int y;
    cout << "Input your second number: ";
    cin >>  y;

    int choice;
    cout << "Choose what to do with your numbers:\n";
    cout << "1.Addition\n2.Substraction\n3.Multiplication\n4.Division\n";
    cout << "5.Modulus\n6.Find max\n7.Find min\n";
    cin >> choice;

    if (choice == 1){
        cout << x << " + " << y << " = " << x + y;
    }
    else if (choice == 2){
        cout << x << " - " << y << " = " << x - y;
    }
    else if (choice == 3){
        cout << x << " x " << y << " = " << x * y;
    }
    else if (choice == 4){
        if (y == 0){
            cout << "Division by zero is undefined.";
        }
        else{
            cout << x << " / " << y << " = " << x / y;
        }
    }
    else if (choice == 5){
        cout << x << " mod " << y << " = " << x % y;
    }
    else if (choice == 6 || choice == 7){
        if (x == y){
            cout << "Both numbers are equal";
        }
        if (choice == 6){
            cout << "The maximum is " << max(x, y);
        }
        else{
            cout << "The minimum is " << min(x, y);
        }
    }
    else{
        cout << "PLease type a number";
    }
}

void ternary_operator(void){
    int k;
    cout << "k: ";
    cin >> k;
    string l = (k < 10) ? "k is smaller than 10" : "k is bigger than 10";
    cout << l;
}

void the_switch_operator(void){
    int a;
    cout << "Enter the day of the week: ";
    cin >> a;

    switch(a){
        case 1:
            cout << "Monday";
            break;
        case 2: 
            cout << "Tuesday";
            break;
        case 3:
            cout << "Wednesday";
            break;
        case 4:
            cout << "Thursday";
            break;
        case 5:
            cout << "Friday";
            break;
        case 6:
            cout << "Saterday";
            break;
        case 7:
            cout << "Sunday";
            break;
        default:
            cout << "There is no match";
            break;
    }
}

void sum_of_primes(void){
    int list[] = {3, 5, 3, 6, 2, 5, 5, 6, 7};
    for (int i = 0; i < 0;){
        cout << i;
    }
}

void for_each_function(void){
    string arr[] = {"Hello", "I", "Am", "Gary"};
    
    for (string i : arr){
        cout << i << endl;
    }
}

void TwoD_array(void){
    int k[2][6] = {
        {1, 2, 3, 4, 5, 6},
        {1, 2, 3, 4, 5, 6}
    };

    for (int i = 0; i < 6; i++){
        cout << k[0][i] << endl;
    }
}

void average(void){
    int score;
    cout << "What are your scores? ";
    int num_of_scores = 0;
    int total = 0;
    while(cin >> score){
        cout << "(Enter a letter to stop) What are your scores? ";
        total = total + score;
        num_of_scores++;
    }
    float avg = (float)total/num_of_scores;
    cout << "Your average is: " << avg;
}

void wtf(void){
    array <int, 6> myArray = {1, 2, 3, 4, 5, 0};
    for (const auto& i : myArray){
        cout << i << " ";
    }

    // lambda shit
    auto check = [](int x){
        return (x == 0);
    };
    // algorithm shit
    if (any_of(myArray.begin(), myArray.end(), check)){
        cout << "There is at least one zero\n";
    }
    else{
        cout << "There is no zero\n";
    }

}

void vector_shit(void){
    vector<int> myVector = {5432, 3456, 6543};
    myVector.push_back(12345672);
    myVector.push_back(50);
    cout << myVector[0] << endl;
    cout << myVector.size() << endl << myVector.capacity();

    for (int i : myVector){
        cout << i << endl;
    }

    vector<vector<int>> the_vector{
        {1, 2, 3, 4, 5},
        {2, 3, 4, 5, 6}
    };

    cout << endl;

    array<array<int, 5>, 5> myArray;
    
    for (array<int, 5> i : myArray){
        for (int j : i){
            cout << j << " ";
        }
    }
    cout << endl;

    for (vector<int> i : the_vector){
        for (int j : i){
            cout << j << " ";
        }
    }
}

typedef struct myStruct{
    int a;
    int b;
}myStruct;

// main
int main(){


    // cout << "Enter to continue";
    // cin.ignore(numeric_limits<streamsize>::max(),'\n');

    // vector<vector<int>> myVector{
    //     {1, 2, 3},
    //     {4, 5, 6},
    //     {7, 8, 9}
    // };
    // cout << myVector[0][-1];


    vector<myStruct> p;

    myStruct tmp;
    tmp.a = 12;
    tmp.b = 12;
    p.push_back(tmp);

    for (int i = 1; i < 9; i++){
        tmp.a = i;
        tmp.b = i;
        p.push_back(tmp);
    }

    for (myStruct i : p){
        cout << i.a << "" << i.b << endl;
    }


    cout << endl;
}