#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

// Fibonacci problem (problem 2)
int fibo(void){
    long a = 0;
    long b = 1;
    long c = 1;
    long sum = 0;

    for (int i = 0; i < 100; i++){
        a = b + c;
        if (a % 2 == 0){
            sum += a;
        }
        b = a + c;
        if (b % 2 == 0){
            sum += a;
        }
        c = a + b;
        if (c % 2 == 0){
            sum += a;
        }


        if (a > 4000000 || b > 4000000 || c > 4000000){
            printf("%li\n", sum);
            return 0;
        }
    }
    return 0;
}

// Largest prime factor (P3)
void prime_divisors(long n){
    long i = 2;
    long max = 0;
    while (n > 2){
        if (n % i == 0){
            printf("%li ", i);
            n = n / i;
            if (i > max){
                max = i;
            }
            i = 2;
        }
        else{
            i += 1;
        }
    }
    printf("The highest is %li", max);
}

// Largest palindrome product (P4)
// void Largest_palindrome_product(void){
//     int z;
//     for (int i = 999; i >= 100; i--){
//         for (int j = 999; j >= 100; j--){
//             z = i * j;

//             char zstr[7];
//             strfromf(zstr, 7, "%f", z);
//             printf("%s\n", zstr);

//         }
//     }
    
// }


void nth_prime(int num){
    int *prime = malloc(num * sizeof(int));
    prime[0] = 2;
    prime[1] = 3;
    prime[num - 1] = 0;

    int n = 3;
    while (prime[num-1] == 0){
        for (int i = 0; i < num; i++){
            if (n % prime[i] == 0){
                break;
            }
            if (prime[i] == 0){
                prime[i] = n;
                break;
            }
        }
        n++;
    }
    printf("%i", prime[num-1]);
    free(prime);
}


int main(void){

    nth_prime(10001);
    printf("\n");

}