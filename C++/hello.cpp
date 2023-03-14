#include <iostream>
using namespace std;
#include <string>
#include <cmath>

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
        case 1: cout << "Monday"; break;
        case 2: cout << "Tuesday"; break;
        case 3: cout << "Wednesday"; break;
        case 4: cout << "Thursday"; break;
        case 5: cout << "Friday"; break;
        case 6: cout << "Saterday"; break;
        case 7: cout << "Sunday"; break;
    }
}

void sum_of_primes(void){
    int list[] = {3, 5, 3, 6, 2, 5, 5, 6, 7};
    for (int i = 0; i < 0;){
        cout << i;
    }

}

int main() {
    int a[] = {1, 2, 3, 4};
    
    string s = "Hello";
    string t = "World";
    s.append(t);
    cout << s;
    
    cout << sizeof(a) / sizeof(a[0]);
    cout << "\n";
}