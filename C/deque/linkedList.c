#include <stdio.h>
#include <stdlib.h>

typedef struct Node {
  int value;
  struct Node *next;
} Node;

typedef struct LinkedList {
  Node *head;
  Node *last;
} LinkedList;

LinkedList construct(int arr[], int size) {
  Node head = {.next = NULL};
  Node *tmp = &head;

  for (int i = 0; i < size; i++) {
    tmp->next = (Node *)malloc(sizeof(Node));
    tmp = tmp->next;

    tmp->value = arr[i];
    tmp->next = NULL;
  }

  LinkedList list = {
      .head = head.next,
      .last = tmp,
  };

  return list;
}

void printList(LinkedList list) {
  for (Node *tmp = list.head; tmp != NULL; tmp = tmp->next) {
    printf("%i ", tmp->value);
  }
}

void freeList(LinkedList list) {
  for (Node *head = list.head; head != NULL;) {
    Node *tmp = head->next;
    free(head);
    head = tmp;
  }
}

void insertNode(LinkedList *list, int val) {
  Node *node = malloc(sizeof(Node));
  node->value = val;
  node->next = list->head;

  list->head = node;
}

void pushNode(LinkedList *list, int val) {
  Node *node = malloc(sizeof(Node));
  node->value = val;
  node->next = NULL;

  list->last->next = node;
  list->last = list->last->next;
}
