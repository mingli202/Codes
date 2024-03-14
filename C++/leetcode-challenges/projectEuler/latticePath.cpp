#include <map>
#include <stdbool.h>
#include <string>
#include <vector>

// tabulation
long long latticePath(int m, int n) {
  std::vector<std::vector<long long>> table;

  for (int i = 0; i <= m; i++) {
    std::vector<long long> tmp;
    for (int k = 0; k <= n; k++) {
      tmp.push_back(0);
    }
    table.push_back(tmp);
  }

  table[1][1] = 1;

  for (int i = 0; i <= m; i++) {
    for (int k = 0; k <= n; k++) {
      if (i + 1 <= m)
        table[i + 1][k] += table[i][k];
      if (k + 1 <= n)
        table[i][k + 1] += table[i][k];
    }
  }

  return table[m][n];
}

// memoization
long long recursion(const int m, const int n,
                    std::map<std::string, long long> &memo) {
  if (m == 1 || n == 1)
    return (long long)1;

  const std::string key = std::to_string(m) + "," + std::to_string(n);
  const std::map<std::string, long long>::iterator it = memo.find(key);

  if (it != memo.end())
    return memo[key];

  memo[key] = recursion(m - 1, n, memo) + recursion(m, n - 1, memo);

  return memo[key];
};

long long latticePathMemo(const int m, const int n) {
  std::map<std::string, long long> memo = {};
  return recursion(m, n, memo);
}

int main() {

  // tests
  printf("%lli\n", latticePathMemo(3, 3));   // 6
  printf("%lli\n", latticePathMemo(1, 1));   // 1
  printf("%lli\n", latticePathMemo(10, 15)); // 817190

  // problem
  long long answer = latticePathMemo(21, 21);
  std::string test = answer == 137846528820 ? "passed" : "failed";
  printf("%lli\n%s", answer, test.c_str()); // passed

  printf("\n");
  return 0;
}
