package main

import (
	"fmt"

	"golang.org/x/tour/tree"
)

// Walk walks the tree t sending all values
// from the tree to the channel ch.
func Walk(t *tree.Tree, ch chan int) {
	if t.Left != nil {
		Walk(t.Left, ch)
	}

	ch <- t.Value

	if t.Right != nil {
		Walk(t.Right, ch)
	}
}

// Same determines whether the trees
// t1 and t2 contain the same values.
func Same(t1, t2 *tree.Tree) bool {
	ch1 := make(chan int)
	ch2 := make(chan int)

	go func() {
		Walk(t1, ch1)
		close(ch1)
	}()

	go func() {
		Walk(t2, ch2)
		close(ch2)
	}()

	for val1 := range ch1 {
		fmt.Println(val1)

		val2, ok := <-ch2

		if !ok {
			return false
		}

		if val1 != val2 {
			return false
		}
	}

	_, ok := <-ch2

	if ok {
		return false
	}

	return true
}

func main() {
	fmt.Println(Same(tree.New(10), tree.New(10)))
	fmt.Println(Same(tree.New(10), tree.New(9)))
}
