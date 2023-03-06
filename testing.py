
def divisors(n):
    l = []
    for i in range(1, n+1):
        if n % i == 0:
            l.append(i)
    return l

def is_prime(n):
    if len(divisors(n)) == 2:
        return True
    else:
        return False
    
def prime_until(n):
    l = [1]
    for i in range(1, n+1):
        if is_prime(i):
            l.append(i)
    return l

if __name__ == "__main__":
    print(divisors(37))
    print(is_prime(37))
    print(prime_until(100))