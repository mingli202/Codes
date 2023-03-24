#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <stdbool.h>

typedef struct node{
    int number;
    struct node* next;
}node;

int example_list(void){
    /*List of size*/
    node* list = NULL;

    /*Add number 1 to list*/
    node* n;
    n = malloc(sizeof(node));
    if (n == NULL){
        return 1;
    }
    n->number = 1;  // instead of writing (*n).number = 1 we write n->number = 1;
    n->next = NULL;

    /*Update list to point to new node*/
    list = n;

    /*Add nummber 2 to list*/
    n = malloc(sizeof(node));
    if (n == NULL){
        free(list);
        return 1;
    }
    n->number = 2;
    n->next = NULL;
    list->next = n;

    /*Add number 3 to list*/
    n = malloc(sizeof(node));
    if (n == NULL){
        free(list->next);
        free(list);
        return 1;
    }
    n->number = 3;
    n->next = NULL;
    list->next->next = n;

    /*Print the numbers*/
    for (node* tmp = list; tmp != NULL; tmp = tmp->next){
        printf("%i\n", tmp->number);
    }

    /*Free list*/
    while (list != NULL){
        node* tmp = list->next;
        free(list);
        list = tmp;
    }
    return 0;
}

int main(void){

    float start;
    float end;

    do{
        printf("Start size: ");
        scanf("%f", &start);

        printf("End size: ");
        scanf("%f", &end);

    }while (start > end);


    int i;
    for (i = 0; start < end; i++){
        float death = start / 4;
        float born = start / 3;
        start = start - death + born;
    }
    printf("%i\n", i);

}