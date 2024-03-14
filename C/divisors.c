#include <stdio.h>
#include <stdlib.h>

typedef struct Node {
  int val;
  struct Node *next;
} Node;

Node *divisors(int n);
void printList(Node *head);
void freeList(Node *head);

int main(void) {
  int n;
  printf("Divisors for: ");
  scanf("%i", &n);

  Node *divs = divisors(n);
  printList(divs);
  freeList(divs);

  printf("\n");
}

Node *divisors(int n) {
  Node *head = (Node *)malloc(sizeof(Node));
  head->val = 1;
  head->next = NULL;
  Node *tmp = head;

  for (int i = 2; i <= n; i++) {
    if (n % i == 0) {
      tmp->next = (Node *)malloc(sizeof(Node));
      tmp = tmp->next;
      tmp->val = i;
      tmp->next = NULL;
    }
  }

  return head;
}

void printList(Node *head) {
  Node *tmp = head;
  while (tmp != NULL) {
    printf("%i ", tmp->val);
    tmp = tmp->next;
  }
}

void freeList(Node *head) {
  Node *tmp = head;
  while (tmp != NULL) {
    Node *next = tmp->next;
    free(tmp);
    tmp = next;
  }
}
