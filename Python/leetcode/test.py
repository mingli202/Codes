import random


def quick_sort(arr: list, start: int, end: int):
    if start >= end:
        return

    i = start
    target = arr[end]

    for k in range(start, end):
        if arr[k] < target:
            arr[k], arr[i] = arr[i], arr[k]
            i += 1

    arr[i], arr[end] = arr[end], arr[i]

    quick_sort(arr, start, i - 1)
    quick_sort(arr, i + 1, end)


def sample(arr: list) -> list:
    toreturn = []

    for _ in range(len(arr)):
        toreturn.append(arr.pop(random.randint(0, len(arr) - 1)))

    return toreturn


arr = sample([1, 2, 3, 4, 5, 6, 7, 8, 9, 10])
print(arr)
quick_sort(arr, 0, arr.__len__() - 1)
print(arr)
