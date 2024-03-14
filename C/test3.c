#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct node {
  int number;
  struct node *next;
} node;

int example_list(void) {
  /*List of size*/
  node *list = NULL;

  /*Add number 1 to list*/
  node *n;
  n = malloc(sizeof(node));
  if (n == NULL) {
    return 1;
  }
  n->number = 1; // instead of writing (*n).number = 1 we write n->number = 1;
  n->next = NULL;

  /*Update list to point to new node*/
  list = n;

  /*Add nummber 2 to list*/
  n = malloc(sizeof(node));
  if (n == NULL) {
    free(list);
    return 1;
  }
  n->number = 2;
  n->next = NULL;
  list->next = n;

  /*Add number 3 to list*/
  n = malloc(sizeof(node));
  if (n == NULL) {
    free(list->next);
    free(list);
    return 1;
  }
  n->number = 3;
  n->next = NULL;
  list->next->next = n;

  /*Print the numbers*/
  for (node *tmp = list; tmp != NULL; tmp = tmp->next) {
    printf("%i\n", tmp->number);
  }

  /*Free list*/
  while (list != NULL) {
    node *tmp = list->next;
    free(list);
    list = tmp;
  }
  return 0;
}

node *makeListFromArray(const char *array, int size) {
  node list = {.next = NULL};
  node *tmp = &list;

  for (int i = 0; i < size; i++) {
    tmp->next = malloc(sizeof(node));
    tmp = tmp->next;
    tmp->number = array[i];
    tmp->next = NULL;
  }

  return list.next;
}

void printList(node *list) {
  for (node *it = list; it != NULL; it = it->next) {
    printf("%i\n", it->number);
  }
}

void freeList(node *list) {
  while (list != NULL) {
    node *it = list->next;
    free(list);
    list = it;
  }
}

node *addTowNumbers(node *l1, node *l2) {
  int sum = 0;
  node list = {.next = NULL};

  // tmp variable that will move and extend the node
  node *tmp = &list;

  while (l1 != NULL || l2 != NULL) {
    tmp->next = malloc(sizeof(node));
    tmp = tmp->next;
    tmp->next = NULL;

    // check which one ends first
    if (!l2) {
      sum = l1->number + sum;
    } else if (!l1) {
      sum = l2->number + sum;
    } else {
      sum = l1->number + l2->number + sum;
    }

    // check if sum is over 10 to see if need to carry the 1
    if (sum >= 10) {
      tmp->number = sum - 10;
      sum = 1;
    } else {
      tmp->number = sum;
      sum = 0;
    }

    // check of there is next
    l1 = l1 ? l1->next : NULL;
    l2 = l2 ? l2->next : NULL;
  }

  // check of last sum is was over 10 which makes sum carry 1
  if (sum == 1) {
    tmp->next = malloc(sizeof(node));
    tmp = tmp->next;
    tmp->number = sum;
    tmp->next = NULL;

    sum = 0;
  }

  return list.next;
}

int lengthOfLongestSubstring(char *s) {
  const int length = strlen(s);
  int max = 0;

  char *substring = "";

  for (int i = 0; i < length; i++) {
    char *has = strchr(substring, s[i]);

    if (has) {
      substring = has + 1;
    }

    int l = strlen(substring);
    char *newstring = malloc(l + 2);
    strcpy(newstring, substring);
    newstring[l] = s[i];
    newstring[l + 1] = '\0';
    substring = newstring;

    if (strlen(substring) > max) {
      max = strlen(substring);
    }
  }

  return max;
}

int checkIsPalindrom(char *s) {
  char *left = s;
  char *right = s + strlen(s) - 1;

  while (left < right) {
    if (*left != *right) {
      return 0;
    }
    left++;
    right--;
  }

  return 1;
}

char *longestPalindrome(char *s) {
  char *substring = "";

  for (; *s != '\0'; s++) {
    char *position = strrchr(s, *s);

    if (position == s)
      continue;

    char *sub = (char *)malloc(position - s + 2);
    strncpy(sub, s, position - s + 1);
    sub[strlen(sub)] = '\0';

    if (strlen(sub) <= strlen(substring))
      continue;

    int isPalindrom = 0;

    while (*sub != '\0') {
      isPalindrom = checkIsPalindrom(sub);

      if (isPalindrom) {
        if (strlen(substring) < strlen(sub)) {
          substring = sub;
        }
        break;
      };

      sub[strlen(sub) - 1] = '\0';
      position = strrchr(sub, *s);
      strncpy(sub, s, position - s);
    }
  }

  return substring;
}

bool isPalindrom(int x) {
  if (x < 0)
    return 0;

  if (x == 0)
    return 1;

  char s[(int)log10(x) + 2];
  sprintf(s, "%i", x);
  s[(int)log10(x) + 1] = '\0';

  int right = 0;
  int left = log10(x);

  while (left > right) {

    if (s[left] != s[right])
      return 0;

    right++;
    left--;
  }

  return 1;
}

int main(void) {

  // char *s = "babad";
  // char *s1 = "cbbd";
  // char *s2 = "abcdegf";

  // char *ans = longestPalindrome(s);
  // puts(ans);

  printf("%i\n", isPalindrom(121));
  printf("%i\n", isPalindrom(0));
  printf("%i\n", isPalindrom(1001));
  printf("%i\n", isPalindrom(2342));

  return 0;
}
