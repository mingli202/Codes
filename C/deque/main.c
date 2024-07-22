#include "./linkedList.c"

#include <stdio.h>

int main(void) {
  int arr[10] = {5, 4, 9, 8, 5, 4, 32, 1, 5, 9};
  LinkedList list = construct(arr, 10);

  insertNode(&list, 9);
  pushNode(&list, -90);

  printList(list);
  freeList(list);

  printf("\n");
  return 0;
}
