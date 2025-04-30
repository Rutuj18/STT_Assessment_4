using System;

class Program
{
    static void Main()
    {
        int wins = 0;
        int draws = 0;
        int losses = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Rock, Paper, Scissors");
            Console.WriteLine();

            Move playerMove;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Choose [r]ock, [p]aper, [s]cissors, or [e]xit: ");
                string input = (Console.ReadLine() ?? "").Trim().ToLower();

                switch (input)
                {
                    case "r":
                    case "rock":
                        playerMove = Move.Rock;
                        validInput = true;
                        break;
                    case "p":
                    case "paper":
                        playerMove = Move.Paper;
                        validInput = true;
                        break;
                    case "s":
                    case "scissors":
                        playerMove = Move.Scissors;
                        validInput = true;
                        break;
                    case "e":
                    case "exit":
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid Input. Try Again...");
                        break;
                }
            }

            Move computerMove = (Move)new Random().Next(3);
            Console.WriteLine($"The computer chose {computerMove}.");
            switch (playerMove, computerMove)
            {
                case (Move.Rock, Move.Paper) or (Move.Paper, Move.Scissors) or (Move.Scissors, Move.Rock):
                    Console.WriteLine("You lose.");
                    losses++;
                    break;
                case (Move.Rock, Move.Scissors) or (Move.Paper, Move.Rock) or (Move.Scissors, Move.Paper):
                    Console.WriteLine("You win.");
                    wins++;
                    break;
                default:
                    Console.WriteLine("This game was a draw.");
                    draws++;
                    break;
            }
            Console.WriteLine($"Score: {wins} wins, {losses} losses, {draws} draws");
            Console.WriteLine("Press Enter To Continue...");
            Console.ReadLine();
        }
    }

    enum Move
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
    }
}
