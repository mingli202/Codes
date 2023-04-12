// Finished on Mar 24 2023

#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <algorithm>
#include <vector>
#include <array>

class Player{
    private:
    array<array<int, 10>, 10> map;

    public:
    Player(){
        for (int i  = 0; i < 10; i++){
            for (int j = 0; j < 10; j++){
                map[i][j] = 0;
            }
        }
    }

    void see_map(void){
        cout << endl;
        int ind = 0;
        for (array<int, 10> i : map){
            for (int j : i){
                cout << j << " ";
            }
        cout << 10 - ind << " ";
        ind++;
        cout << endl;
        }
        cout << "a b c d e f g h i j\n\n";
    }

    void set_ship(void){
        char xi;
        char xf;
        int yi;
        int yf;

        cout << " starting coodinates (xy): ";
        cin >> xi >> yi;
        if (xi < 97 || xi > 106 || yi < 1 || yi > 10){
            cout << " Please type a valid coordinates (form \'LetterNumber\', eg. \'a1\'" << endl;
            return;
        }

        cout << " ending coordinates (xy): ";
        cin >> xf >> yf;
        if (xf < 97 || xf > 106 || yf < 1 || yf > 10){
            cout << " Please type a valid coordinates (form \'LetterNumber\', eg. \'a1\'" << endl;
            return;
        }

        if (xi == xf && yi == yf){
            cout << " The ship have to be at least 2 square long!" << endl;
            return;
        }

        if (xi == xf){
            for (int i = 10 - max(yi, yf); i < 11 - min(yi, yf); i++){
                map[i][xi - 97] = 1;
            }
        }
        else if (yi == yf){
            for (int i = min(xi, xf) - 97; i < max(xi, xf) - 96; i++){
                map[yi - 1][i] = 1;
            }
        }
        else{
            cout << " Ships can\'t go diagonally!" << endl;
            return;
        }
    }

    void attack(int x, int y){
        if (map[x][y] == 0){
            cout << "miss!" << endl;
        }
        else{
            cout <<"Hit!" << endl;
            map[x][y] == 0;
        }

    }
};

class Ai : public Player{

};

int main(){

    Player player1;
    Ai ai;

    cout << "Enter \'help\' for commands" << endl;
    string input;    
    while (getline(cin , input)){

        if (input == "help"){
            cout << "Enter:\n";
            cout << "\'quit\' to exit the game\n";
            cout << "\'set ship\' to set your ship\n";
            cout << "\'see map\' to see your current map\n";
            cout << "\'ready\' if you are ready to fight the enemy(once ready, there is no going back)\n";
        }

        if (input == "set ship"){
            player1.set_ship();
        }
        if (input == "see map"){
            player1.see_map();
        }

        if (input == "ready"){
            string atk;
            while (getline(cin, atk)){
                break;
            }
            
            break;
        }
    }


    cout << endl;
    return 0;
}
