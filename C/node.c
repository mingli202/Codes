#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <stdbool.h>

typedef struct node{
    int number;
    struct node* next;
}node;

node* create_node(num){
    node* n = malloc(sizeof(node));
    n->number = num;
    n->next = NULL;
    return n;
}

node* add_node(node* value){
    
}

int main(void){

    node* root;


    printf("\n");
}