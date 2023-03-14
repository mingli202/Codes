#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <stdbool.h>

typedef struct node{
    int number;
    struct node* next;
}node;

void divisors(num){
    node* cursor;
    node* list = malloc(sizeof(node));
    list->number = 1;
    list->next = NULL;

    for (int i = 2; i <= num; i++){
        if (num % i == 0){
            node* n = malloc(sizeof(node));
            if (n == NULL){
                printf("No more memory\n");
                return;
            }
            if (list->next == NULL){
                list = n;
            }
            else{
                cursor = list;
                while (cursor->next != NULL){
                    cursor = cursor->next;
                }
                cursor->next = n;
            }

            free(n);
        }
    }
    printf("The divisors are: ");
    for (node* tmp = list; tmp->next != NULL; tmp = tmp->next){
        printf("%i ", tmp->number);
    }
}

int main(void){

    // divisors(48);

    printf("\n");
}