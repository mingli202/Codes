import math
from dataclasses import dataclass
from typing import Self


@dataclass
class Node:
    val: int
    left: Self | None = None
    right: Self | None = None


class BST:
    root: Node | None = None

    def __init__(self, arr: list[int] = []):
        for val in arr:
            self.add(val)

    def add(self, val: int):
        node = self.root

        if node is None:
            self.root = Node(val)
            return

        while True:
            if val < node.val:
                if node.left is None:
                    node.left = Node(val)
                    return
                else:
                    node = node.left

            else:
                if node.right is None:
                    node.right = Node(val)
                    return
                else:
                    node = node.right

    def to_arr(self) -> list[int]:
        def traverse(node: Node | None) -> list[int]:
            if node is None:
                return []

            return traverse(node.left) + [node.val] + traverse(node.right)

        return traverse(self.root)


if __name__ == "__main__":
    tree = BST([1, 5, 4, 3, 2, 6, 7, 8, 9, 10])
    print(tree.to_arr())
