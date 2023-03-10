#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>


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