class Program
{
    public static void Main(string[] argv)
    {
        int[,] arr =
        {
            { 1, 2 },
            { 3, 4 }
        };

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int k = 0; k < arr.GetLength(1); k++)
            {
                Console.Write(arr[i, k]);
            }
            Console.WriteLine();
        }
    }

    public static void RockPaperScissors()
    {
        Random rand = new Random();

        string[] moves = { "rock", "paper", "scissors" };

        Console.WriteLine("Play Rock, Paper, Scissors!");

        bool win = false;
        while (!win)
        {
            string computerMove = moves[rand.Next(0, 2)];
            Console.WriteLine("Your move (rock, paper, or scissors):");
            string? player = Console.ReadLine();

            if (player == null)
            {
                Console.WriteLine("Enter a valid move.");
                continue;
            }

            player = player.ToLower();
            if (Array.IndexOf(moves, player) == -1)
            {
                Console.WriteLine("Enter a valid move.");
                continue;
            }

            switch (player)
            {
                case "rock":
                    if (computerMove == "rock")
                    {
                        Console.WriteLine("Draw!");
                        break;
                    }
                    if (computerMove == "paper")
                    {
                        Console.WriteLine("You lose!");
                        break;
                    }
                    win = true;
                    break;
                case "paper":
                    if (computerMove == "paper")
                    {
                        Console.WriteLine("Draw!");
                        break;
                    }
                    if (computerMove == "scissors")
                    {
                        Console.WriteLine("You lose!");
                        break;
                    }
                    win = true;
                    break;
                case "scissors":
                    if (computerMove == "scissors")
                    {
                        Console.WriteLine("Draw!");
                        break;
                    }
                    if (computerMove == "rock")
                    {
                        Console.WriteLine("You lose!");
                        break;
                    }
                    win = true;
                    break;
            }
        }
        Console.WriteLine("Congrats you won!");
    }

    public static void GuessNumber()
    {
        Random rand = new Random();

        int target = rand.Next(1, 1000);

        int guess = 0;

        Console.WriteLine("Guess the secret number between 1 and 1000:");
        int.TryParse(Console.ReadLine(), out guess);

        while (guess != target)
        {
            if (guess < 1 || guess > 1000)
            {
                Console.WriteLine("Please enter a valid number:");
            }
            else if (guess > target)
            {
                Console.WriteLine("Too high! Try again:");
            }
            else
            {
                Console.WriteLine("Too low! Try again:");
            }

            int.TryParse(Console.ReadLine(), out guess);
        }

        Console.WriteLine("You guessed it!");
    }

    public static void Loop()
    {
        int counter = 0;
        while (counter < 5)
        {
            Console.WriteLine("Hello world");
            counter++;
        }
    }

    public static void SpeedLimit()
    {
        const int SPEED_LIMIT = 100;
        int speed = 0;

        while (true)
        {
            Console.WriteLine("What was your speed?");

            if (!int.TryParse(Console.ReadLine(), out speed))
            {
                Console.WriteLine("Enter a valid number");
                continue;
            }
            break;
        }

        if (speed > SPEED_LIMIT)
        {
            Console.WriteLine(
                "You are over the speed limit. Limit is {0}, you were going {1}",
                SPEED_LIMIT,
                speed
            );
        }
        else
        {
            Console.WriteLine("Keep going");
        }
    }
}
