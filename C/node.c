#include <stdio.h>
#include <stdlib.h>

struct ListNode {
  int val;
  struct ListNode *next;
};

struct ListNode *toListNode(int nums[], int length) {
  struct ListNode head = {.next = NULL};
  struct ListNode *tmp = &head;

  for (int i = 0; i < length; i++) {
    tmp->next = malloc(sizeof(struct ListNode));
    tmp = tmp->next;
    tmp->val = nums[i];
    tmp->next = NULL;
  }

  return head.next;
}

void printListNode(struct ListNode *list) {
  for (struct ListNode *tmp = list; tmp != NULL; tmp = tmp->next) {
    printf("%i ", tmp->val);
  }
  printf("\n");
}

void freeListNode(struct ListNode *list) {
  while (list != NULL) {
    struct ListNode *tmp = list->next;
    free(list);
    list = tmp;
  }
}

struct ListNode *mergeTwoLists(struct ListNode *list1, struct ListNode *list2) {
  struct ListNode *current = list1;
  struct ListNode *last;

  while (current != NULL) {
    if (current->val > list2->val) {
      struct ListNode *t = list2;
    } else {
      current = current->next;
    }
  }

  while (list2 != NULL) {
    current->next = list2;
  }

  return list1;
}

void test1() {
  int nums[10] = {3, 3, 4, 5, 5, 6, 8, 9, 10, 12};
  int nums2[10] = {1, 3, 5, 6, 6, 7, 8, 9, 10, 11};

  struct ListNode *list1 = toListNode(nums, 10);
  struct ListNode *list2 = toListNode(nums2, 10);
  struct ListNode *list = mergeTwoLists(list1, list2);
  printListNode(list);
  freeListNode(list);
}

void test2() {
  int nums[0] = {};
  int nums2[0] = {};

  struct ListNode *list1 = toListNode(nums, 0);
  struct ListNode *list2 = toListNode(nums2, 0);
  struct ListNode *list = mergeTwoLists(list1, list2);
  printListNode(list);
  freeListNode(list);
}
void test3() {
  int nums[0] = {};
  int nums2[1] = {0};

  struct ListNode *list1 = toListNode(nums, 0);
  struct ListNode *list2 = toListNode(nums2, 1);
  struct ListNode *list = mergeTwoLists(list1, list2);
  printListNode(list);
  freeListNode(list);
}

int main(void) {
  test1();
  test2();
  test3();
  printf("\n");
}
