import unittest
from typing import Optional
from collections import deque
import json


class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right


def arr_to_tree(arr: list[int | None], node: int) -> Optional[TreeNode]:
    if node >= len(arr):
        return None

    val = arr[node]

    if val is None:
        return None

    return TreeNode(val, arr_to_tree(arr, 2 * node + 1), arr_to_tree(arr, 2 * node + 2))


class Solution(unittest.TestCase):
    def treeQueries(self, root: Optional[TreeNode], queries: list[int]) -> list[int]:
        lvls_below: dict[int, int] = {}
        tree: dict[int, int] = {}

        def dfs(
            r: Optional[TreeNode],
            node: int = 0,
            lv_b: dict[int, int] = lvls_below,
            t: dict[int, int] = tree,
        ) -> int:
            if not r:
                return 0

            tree[r.val] = node

            lv_below = (
                max(
                    dfs(r.left, node * 2 + 1, lv_b, t),
                    dfs(r.right, node * 2 + 2, lv_b, t),
                )
                + 1
            )

            lvls_below[node] = lv_below

            return lv_below

        def f(query) -> int:
            depth = 0
            node = tree[query]

            while node >= 0:
                parent = (node - 1) // 2  # parent

                left = parent * 2 + 1
                right = parent * 2 + 2

                left_val = 0
                if left == node:
                    left_val = depth
                elif left in lvls_below:
                    left_val = lvls_below[left]

                right_val = 0
                if right == node:
                    right_val = depth
                elif right in lvls_below:
                    right_val = lvls_below[right]

                depth = max(left_val, right_val) + 1
                node = parent

            return depth - 2

        dfs(root)
        print(json.dumps(tree, indent=2))
        print(json.dumps(lvls_below, indent=2))

        return [f(q) for q in queries]

    def test_case_1(self):
        self.assertEqual(
            [2],
            self.treeQueries(
                arr_to_tree(
                    [1, 3, 4, 2, None, 6, 5, None, None, None, None, None, 7], 0
                ),
                [4],
            ),
        )

    def test_case_2(self):
        root = arr_to_tree([5, 8, 9, 2, 1, 3, 7, 4, 6], 0)
        self.assertEqual([3, 2, 3, 2], self.treeQueries(root, [3, 2, 4, 8]))

    def test_case_3(self):
        self.assertEqual(
            [1, 0, 3, 3, 3],
            self.treeQueries(
                arr_to_tree(
                    [
                        1,
                        None,
                        5,
                        3,
                        None,
                        2,
                        4,
                    ],
                    0,
                ),
                [3, 5, 4, 2, 4],
            ),
        )


unittest.main()
