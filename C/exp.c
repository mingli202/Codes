#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <stdbool.h>

typedef struct node{
    int number;
    struct node* next;
}node;

void free_list(node* list){
    while (list != NULL){
        node* tmp = list->next;
        free(list);
        list = tmp;
    }
}

int main(void){

    node* ptr_list = NULL;

    // Add number
    node* ptr_n;
    node* ptr_cursor;

    // Add 1 to 10
    for (int i = 1; i <= 10; i++){
        ptr_n = malloc(sizeof(node));
        ptr_n->number = i;
        ptr_n->next = NULL;

        ptr_cursor = ptr_n;
    }

    printf("%i", ptr_list->next->number);
    
    free_list(ptr_list);

    printf("\n");
}