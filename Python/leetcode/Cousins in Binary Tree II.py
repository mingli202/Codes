from typing import Optional
from collections import deque


class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right


class Solution:
    def replaceValueInTree(self, root: Optional[TreeNode]) -> Optional[TreeNode]:
        if root is None:
            return None

        root.val = 0

        q: deque[TreeNode] = deque()
        q.append(root)

        while len(q) > 0:
            s_lvl = 0
            s_parent = 0

            for current in q:
                if current.left:
                    s_lvl += current.left.val
                if current.right:
                    s_lvl += current.right.val

            for _ in range(len(q)):
                current = q.popleft()

                if current.left:
                    s_parent += current.left.val
                    q.append(current.left)
                if current.right:
                    s_parent += current.right.val
                    q.append(current.right)

                if current.left:
                    current.left.val = s_lvl - s_parent
                if current.right:
                    current.right.val = s_lvl - s_parent

        return root
