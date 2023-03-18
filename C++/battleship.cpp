#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <algorithm>
#include <initializer_list>

class Player{
    private:
        int map[5][5] = {
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
        };

    public:
        void print_map(void){
            cout << "\nYour map:\n";
            for (int i = 0; i < 5; i++){
                cout << 5 - i << ": ";
                for (int j = 0; j< 5; j++){
                    cout << map[i][j] << " ";
                }
                cout << endl;
            }
            cout << "   a b c d e\n\n";
        }

        void set_ship(void){
            int orientation;

            cout << "Choose your orientation:\n1: Horizontal\n2: Vertical\n";
            cin >> orientation;
            if (orientation != 1 && orientation != 2){
                cout << "Please choose 1 or 2";
                return;
            }
            // For horizontal
            if (orientation == 1){
                int y;
                int y_index;
                cout << "y-coordinate: ";
                cin >> y;
                if (y == 0){
                    cout << "Please type a valid coodinate";
                    return;
                }
                y_index = 5 - y;

                char xi;
                char xf;
                cout << "x-coordinate start: ";
                cin >> xi;
                xi = xi - 97;
                cout << "y-coordinate end: ";
                cin >> xf;
                xf = xf - 97;

                for (int i = min(xf, xi); i <= max(xf, xi); i++){
                    map[y_index][i] = 1;
                }

                cout << "\nYour new battleship location:\n";
                for (int i = 0; i < 5; i++){
                    cout << 5 - i << ": ";
                    for (int j = 0; j< 5; j++){
                        cout << map[i][j] << " ";
                    }
                    cout << endl;
                }
                cout << "   a b c d e\n\n";
            }
            
            // For vertical
            if (orientation == 2){
                char x;
                int x_index;
                cout << "x-coordinate: ";
                cin >> x;
                if (x == 0){
                    cout << "Please type a valid coordinate";
                    return;
                }
                x_index = (int)x - 97;

                int yi;
                int yf;
                cout << "y-coordinate start: ";
                cin >> yi;
                cout << "y-coordinate end: ";
                cin >> yf;

                cout << "\nYour new battleship location:\n";
                for (int i = 5 - max(yi, yf); i <= 5 - min(yi, yf); i++){
                    map[i][x_index] = 1;
                }
                for (int i = 0; i < 5; i++){
                    cout << 5 - i << ": ";
                    for (int j = 0; j< 5; j++){
                        cout << map[i][j] << " ";
                    }
                    cout << endl;
                }
                cout << "   a b c d e\n\n";
            }

        }

        void attack(void){
            // bool found = false;
            // for (int i = 0; i < 5; i++){
            //     for (int j = 0; j < 5; j++){
            //         if (map[i][j] == 1){
            //             found = true;
            //             break;
            //         }
            //     }
            //     if (found){
            //         break;
            //     }
            // }
            // if (!found){
            //     cout << "You have set no battle ship\n";
            //     return;
            // }


            if (all_of(map[0][0], map[4][4], [](int x){return (x == 0);})){
                cout << "You have no battleship set";
                return;
            }

            srand((unsigned) time(NULL));
            int x_atk = rand() % 4;
            int y_atk = rand() % 4;

            cout << "Launching missile at " << (char)(x_atk + 97) << 5 - y_atk << endl;

            if (map[y_atk][x_atk] == 1){
                cout << "Hit!\n";
            }
            else{
                cout << "Miss!\n";
            }
        }
};



int main(){

    Player player_one;

    //main loop
    string input;
    cout << "Enter help for command help\n";
    while(getline(cin, input)){
        if (input == "help"){
            cout << "Type:\n";
            cout << "\'quit\' to exit\n";
            cout << "\'see map\' to see your map\n";
            cout << "\'set ship\' to set your ship\n";
            cout << "\'ready\' if you are ready for the attack\n";
        }
        if (input == "quit"){
            break;
        }
        if (input == "set ship"){
            player_one.set_ship();
        }
        if (input == "see map"){
            player_one.print_map();
        }
        if (input == "ready"){
            player_one.attack();
        }
    }

    cout << endl;
}