// Project finished on Mar 29, 2023

#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <algorithm>
#include <vector>
#include <array>
#include <limits>
#include <random>

class Player{
    public:

    string direction;
    int orientation;
    char x;
    int i_x;
    int y;
    int i_y;
    int steps;
    
    int turn(vector<vector<int>>& m, int p){
        cout << "Choose your starting square (form is \'a1\'): ";
        cin >> x >> y;
        cin.clear();
        cin.ignore();
        i_x = x - 97;
        i_y = y - 1;
        if (x < 97 || x > 99 || y < 1 || y > 3){
            cout << "Please type a valid answer\n\n";
            return 0;
        }
        if (m[i_y][i_x] != 0){
            cout << "\n  You have to start in an empty square!\n\n";
            return 0;
        }

        cout << "Orientation: 1 (horizontal); 2 (Vertical); 3 (Diagonal)\n";
        cin >> orientation;
        cin.clear();
        cin.ignore();
        if (orientation == 1){
            cout << "Direction: l (left); r (right)\n";
            cin >> direction;
            cin.clear();
            cin.ignore();
            if (direction != "l" && direction != "r"){
                cout << "\n  Please type a valid answer\n\n";
                return 0;
            }
        }
        else if(orientation == 2){
            cout << "Direction: u (up); d (down)\n";
            cin >> direction;
            cin.clear();
            cin.ignore();
            if (direction != "u" && direction != "d"){
                cout << "\n  Please type a valid answer\n\n";
                return 0;
            }
        }
        else if(orientation == 3){
            cout << "Direction: ul (up-left); ur (up-right); dl (down-left); dr (down-right)\n";
            cin >> direction;
            cin.clear();
            cin.ignore();
            if (direction != "ul" && direction != "ur" && direction != "dl" && direction != "dr"){
                cout << "\n  Please type a valid answer\n\n";
                return 0;
            }
        }
        else{
            cout << "\n  Please type a valid answer\n\n";
            return 0;
        }

        srand((unsigned) time(NULL));
        steps = rand() % 3 + 1;

        cout << "Moved " << steps << " steps in your chosen direction.\nPress Enter to continue.";
        cin.ignore(numeric_limits<streamsize>::max(),'\n');

        while (steps > 0){
            if (direction == "l" || direction == "ul" || direction == "dl"){
                i_x--;
                if (i_x < 0){
                    i_x = 2;
                }
            }
            if (direction == "r" || direction == "ur" || direction == "dr"){
                i_x++;
                if (i_x > 2){
                    i_x = 0;
                }
            }
            if (direction == "u" || direction == "ul" || direction == "ur"){
                i_y--;
                if (i_y < 0){
                    i_y = 2;
                }
            }
            if (direction == "d" || direction == "dl" || direction == "dr"){
                i_y++;
                if (i_y > 2){
                    i_y = 0;
                }
            }
            steps--;
            
        }
        m[i_y][i_x] = p;
        return 1;
    }
};


class AI : public Player{
    public:

    void easy(vector<vector<int>>& m, int p, int xx, int yy){
        m[yy][xx] = p;

        cout << "Placed at " << (char)(xx + 97) << yy + 1 << endl;
        cout << "Press Enter to continue.";
        cin.ignore(numeric_limits<streamsize>::max(),'\n');

    }
};


void see_map(vector<vector<int>> m){
    int k = 1;
    cout << endl;
    cout << "  a   b   c\n";
    for (vector<int> i : m){
        for (int j : i){
            if (j == 0){
                cout << "|   ";
            }
            else if (j == 1){
                cout << "| X ";
            }
            else{
                cout << "| O ";
            }

        }
        cout << "| " << k << endl;
        k++;
    }
}

int check_win(vector<vector<int>> m){
    vector<int> win;
    auto check_w = [](vector<int> v){
        if (all_of(v.begin(), v.end(), [](int x){return x == 1;})){
            cout << endl << "Player One Wins!" << endl;
            return true;
        }
        else if(all_of(v.begin(), v.end(), [](int x){return x == 2;})){
            cout << endl << "Player Two Wins!" << endl;
            return true;
        }
        return false;
    };

    for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
            win.push_back(m[i][j]);
        }
        if (check_w(win)){
            return 1;
        }
        win.clear();
    }
    win.clear();

    for (int i = 0; i < 3; i++){
        for (int j = 0; j < 3; j++){
            win.push_back(m[j][i]);
        }
        if (check_w(win)){
            return 1;
        }
        win.clear();
    }    

    win.clear();
    
    for (int i = 0; i < 3; i++){
        win.push_back(m[i][i]);
    }
    if (check_w(win)){
        return 1;
    }
    win.clear();

    for (int i = 0; i < 3; i++){
        win.push_back(m[2 - i][i]);
    }
    if (check_w(win)){
        return 1;
    }
    win.clear();


    auto check = [](vector<int> v){
        return all_of(v.begin(), v.end(), [](int x){return x != 0;});
    };
    if (all_of(m.begin(), m.end(), check)){
        cout << "Draw\n";
        return -1;
    }

    return 0;
}

int main(){

    vector<vector<int>> map{
        {0, 0, 0},
        {0, 0, 0},
        {0, 0, 0}
    };

    Player player1;
    Player player2;
    AI enemy;


    int turn = 0;
    char ai = false;
    do{
        cout << "Play against player? (y/n): ";
        cin >> ai;
        cin.clear();
        cin.ignore();
    }while(ai != 'y' && ai != 'n');


    see_map(map);
    if (ai == 'y'){
        while (true){
            if (turn == 0){
                cout << "\nPlayer One (X)\n\n";
                if(player1.turn(map, 1) == 1){
                    turn = 1;
                    see_map(map);
                    if(check_win(map) == 1){
                        break;
                    }
                    if(check_win(map) == -1){
                        break;
                    }
                }
            }
            else if(turn == 1){
                cout << "\nPlayer Two (O)\n\n";
                if(player2.turn(map, 2) == 1){
                    turn = 0;
                    see_map(map);
                    if(check_win(map) == 1){
                        break;
                    }
                    if(check_win(map) == -1){
                        break;
                    }
                }
            }
        }
    }
    else{
        int x;
        int y;
        while (true){
            srand((unsigned) time(NULL));
            x = rand() % 3;
            y = rand() % 3;

            if (turn == 0){
                cout << "\nPlayer One (X)\n\n";
                if(player1.turn(map, 1) == 1){
                    turn = 1;
                    see_map(map);
                    if(check_win(map) == 1){
                        break;
                    }
                    if(check_win(map) == -1){
                        break;
                    }
                }
            }
            else if(turn == 1){

                cout << "\nPlayer Two - Ai (O)\n\n";

                enemy.easy(map, 2, x, y);
                see_map(map);
                turn = 0;
                if(check_win(map) == 1){
                    break;
                }
                if(check_win(map) == -1){
                    break;
                }
            }
        }
    }
    cout << endl;
    return 0;
}
