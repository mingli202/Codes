#include <stdio.h>

extern int number;

void hello(void){
    printf("Hello, %i", number);
}