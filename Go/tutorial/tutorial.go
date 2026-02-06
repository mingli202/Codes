package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

func main() {
	messages := make(chan string)
	var wg sync.WaitGroup

	for i := range make([]bool, 10) {
		wg.Go(func() {
			time.Sleep(time.Second * time.Duration(rand.Float64()*5))
			messages <- fmt.Sprintf("goroutine from %v!", i)
		})
		time.Sleep(time.Millisecond * 100)
	}

	go func() {
		wg.Wait()
		close(messages)
	}()

	for m := range messages {
		fmt.Println(m)
		time.Sleep(time.Millisecond * 100)
	}
}
