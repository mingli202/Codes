#include <iostream>
using namespace std;
#include <algorithm>
#include <array>
#include <cmath>
#include <iterator>
#include <set>
#include <string>
#include <vector>

vector<int> climbingLeaderboard(vector<int> ranked, vector<int> player) {
  vector<int> answer;
  set<int> mySet(ranked.begin(), ranked.end());
  set<int>::iterator it = mySet.begin();
  int rank = 0;
  for (int i = 0; i < player.size();) {
    if (player[i] > *it) {
      if (it != mySet.end()) {
        advance(it, 1);
        rank++;
      } else {
        answer.push_back(1);
        i++;
      }
    } else if (player[i] == *it) {
      answer.push_back(mySet.size() - rank);
      i++;
    } else {
      answer.push_back(mySet.size() - rank + 1);
      i++;
    }
  }
  return answer;
}

// alternating harmonic seris
void alt_harmonic_series(long n) {
  double sum = 0;
  double a_n;
  for (double i = 1; i < n + 1; i++) {
    a_n = pow(-1, i + 1) * (1 / i);
    sum += a_n;
  }
  cout << sum;
}

void some_series(long n) {
  double sum;
  for (long i = 1; i < n + 1; i++) {
    sum += (double)cos(i) / i;
  }
  cout << sum;
}

int main() {
  // vector<int> myVector{1, 1, 2, 2, 3, 4, 4, 12, 6, 8, 5, 4, 1};
  // set<int> mySet(myVector.begin(), myVector.end());

  // for (auto i : mySet){
  //     cout << i << endl;
  // }

  // vector<int> ranked{100, 100, 50, 40, 40, 20, 10};
  // vector<int> player{5, 25, 50, 120};
  // for (int i : climbingLeaderboard(ranked, player)){
  //     cout << i << " ";
  // }

  // alt_harmonic_series(999999999);
  for (long i = 1; i < 1000000; i += 100000) {
    some_series(i);
    cout << "\n";
  }

  std::vector<int> h = {1, 2, 3, 4, 5};

  cout << endl;
  return 0;
}
