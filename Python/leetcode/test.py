OKGREEN = "\033[92m"
FAIL = "\033[91m"
ENDC = "\033[0m"


def test(lhs, rhs):
    if lhs == rhs:
        print(f"{OKGREEN}Ok{ENDC}")

    else:
        print(f"{FAIL}Fail{ENDC}")
        print("lhs:", lhs)
        print("rhs:", rhs)
