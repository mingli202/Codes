#include <iostream>
using namespace std;
#include <string>
#include <cmath>
#include <array>
#include <algorithm>
#include <vector>
#include <set>
#include <iterator>


vector<int> climbingLeaderboard(vector<int> ranked, vector<int> player) {
    vector<int> answer;
    set<int> mySet(ranked.begin(), ranked.end());
    set<int>::iterator it = mySet.begin();
    int rank = 0;
    for (int i = 0; i < player.size();){
        if (player[i] > *it){
            if (it != mySet.end()){
                advance(it, 1);
                rank++;
            }
            else{
                answer.push_back(1);
                i++;
            }
        }
        else if (player[i] == *it){
            answer.push_back(mySet.size() - rank);
            i++;
        }
        else{
            answer.push_back(mySet.size() - rank + 1);
            i++;
        }
    }
    return answer;
}


int main(){
    vector<int> myVector{1, 1, 2, 2, 3, 4, 4, 12, 6, 8, 5, 4, 1};
    set<int> mySet(myVector.begin(), myVector.end());

    for (auto i : mySet){
        cout << i << endl;
    }

    vector<int> ranked{100, 100, 50, 40, 40, 20, 10};
    vector<int> player{5, 25, 50, 120};
    for (int i : climbingLeaderboard(ranked, player)){
        cout << i << " ";
    }
    

    cout << endl;
    return 0;
}