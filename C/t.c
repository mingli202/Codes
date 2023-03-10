#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <stdbool.h>

// Fibonacci problem (problem 2)
void fibo(void){
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
            return;
        }
    }
    return;
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

// Is prime
bool is_prime(num){
    for (int i = 2; i < num; i++){
        if (num % i == 0){
            return false;
        }
    }
    return true;
}

// 10001th prime (P5)
void nth_prime(int num){
    int *prime = malloc(num * sizeof(int));
    prime[0] = 2;
    prime[1] = 3;
    prime[num - 1] = 0;

    int n = 3;
    while (prime[num - 1] == 0){
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
    printf("The %ith prime number is %i", num, prime[num - 1]);
    free(prime);
}

// Special Pythagorean triplet (P9)
void special_pythagorean_triplet(void){
    float c = 0;
    float sum = 0;
    for (int a = 1; a < 999; a++){
        for (int b = 1; b < 999; b++){
            c = sqrtf(a*a + b*b);
            sum = a + b + c;
            if (sum == 1000){
                printf("(a, b, c) = (%i, %i, %f)\n", a, b, c);
                printf("Product = %f", a * b * c);
                return;
            }
        }
    }
    printf("There are not triplets that satisfy the condition\n");
    return;
}

void divisors(int num){
    int *div = malloc(0);
    int ind = 0;
    for (int i = 1; i <= num; i++){
        if (num % i == 0){
            div = (int*)realloc(div, (ind + 1) * sizeof(int));
            div[ind] = i;
            ind++;
        }
    }
    printf("Divisors of %i are: ", num);
    for (int k = 0; k < ind; k++){
        printf("%i ", div[k]);
    }

    free(div);
}

// Summation of primes (P10)
void sum_of_primes(n){
    int size = 2;
    long *primes = malloc(size * sizeof(long));
    primes[0] = 2;
    primes[1] = 3;
    long long sum;

    for (int i = 4; i < n; i++){
        for (int j = 0; j < size; j++){
            if (i % primes[j] == 0){
                break;
            }
            if (j == size - 1){
                primes = (long*)realloc(primes, (size + 1) * sizeof(long));
                primes[size] = i;
                size++;
                sum += i;
            }
        }
    }
    printf("%lli", sum);
    
}



int main(void){

    divisors(74378325);
    printf("\n");

}