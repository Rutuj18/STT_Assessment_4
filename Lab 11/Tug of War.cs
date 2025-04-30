using System;
using System.Threading;

class TugOfWarGame
{
    static void Main()
    {
        Exception? exception = null;

        try
        {
            while (true)
            {
                int position = 0;
                const int displacement = 10;
                string L() => new(' ', displacement + position + 4);
                string R() => new(' ', displacement - position + 4);
                string Ground =
                    new string(' ', 2) +
                    new string('-', displacement + (15 - displacement) + 2) +
                    new string('=', displacement * 2 + 2) +
                    new string('-', displacement + (15 - displacement) + 2) +
                    new string(' ', 2);
                bool frame_a = false;

                Console.Clear();

                // Check terminal size before starting the game
                if (!EnsureTerminalSize(60, 20))
                {
                    return; // Exit if terminal is too small and user chooses to quit
                }

                Console.Write("""
                      Tug Of War
                    
                      Out pull your opponent in a rope pulling
                      competition. Mash the [left]+[right] arrow
                      keys and/or the [A]+[D] keys to pull on the
                      rope. First player to pull the center of the
                      rope into their boundary wins.
                    
                      Choose Your Opponent:
                      [1] Easy.......2 mashes per second
                      [2] Medium.....4 mashes per second
                      [3] Hard.......8 mashes per second
                      [4] Harder....16 mashes per second
                      [P] Pause game during play
                      [escape] give up
                    """);

                int? requiredMash = null;
                while (requiredMash is null)
                {
                    Console.CursorVisible = false;
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1 or ConsoleKey.NumPad1: requiredMash = 02; break;
                        case ConsoleKey.D2 or ConsoleKey.NumPad2: requiredMash = 04; break;
                        case ConsoleKey.D3 or ConsoleKey.NumPad3: requiredMash = 08; break;
                        case ConsoleKey.D4 or ConsoleKey.NumPad4: requiredMash = 16; break;
                        case ConsoleKey.Escape: return;
                    }
                }

                if (requiredMash is null)
                {
                    continue; // Shouldn't happen, but just in case
                }

                Console.Clear();

                int mash = 0;
                int presses = 0;
                int sleeps = 0;
                ConsoleKey lastKey = default;
                DateTime start = DateTime.Now;
                bool gamePaused = false;

                while (true)
                {
                    // Handle pause state
                    if (gamePaused)
                    {
                        DisplayPauseScreen();
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.P)
                        {
                            gamePaused = false;
                            Console.Clear();
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            return; // Exit game
                        }
                        continue;
                    }

                    // Process a limited number of keys per frame to prevent buffer overflow
                    int keyProcessLimit = 10;
                    int keysProcessed = 0;

                    while (Console.KeyAvailable && keysProcessed < keyProcessLimit)
                    {
                        keysProcessed++;
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        ConsoleKey key = keyInfo.Key;

                        if (key == ConsoleKey.Escape)
                        {
                            return;
                        }
                        else if (key == ConsoleKey.P)
                        {
                            gamePaused = true;
                            break;
                        }
                        else if (lastKey is not default(ConsoleKey) &&
                            key is ConsoleKey.A or ConsoleKey.D or ConsoleKey.LeftArrow or ConsoleKey.RightArrow &&
                            key != lastKey)
                        {
                            presses++;
                            mash++;
                            lastKey = default;
                        }
                        else if (key is ConsoleKey.A or ConsoleKey.D or ConsoleKey.LeftArrow or ConsoleKey.RightArrow)
                        {
                            lastKey = key;
                        }
                    }

                    if (sleeps is 2)
                    {
                        position = mash < requiredMash.Value
                            ? position + 1
                            : position - 1;
                        sleeps = 0;
                        mash = 0;

                        // Check for win/lose condition
                        if (Math.Abs(position) >= displacement)
                        {
                            break;
                        }
                    }

                    // Render game state
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine();
                    Console.WriteLine("  Tug Of War");
                    Console.WriteLine();

                    // Display visual progress indicator
                    int progressBarWidth = displacement * 2;
                    int progressPosition = displacement + position;
                    string progressBar = "[" + new string(' ', progressPosition) + "O" +
                                         new string(' ', progressBarWidth - progressPosition - 1) + "]";
                    Console.WriteLine($"  {progressBar}");

                    Console.Write(frame_a
                        ?
                        $@"{L()}o                             o {R()}{"\n"}" +
                        $@"{L()}LL-------------+-------------JJ\{R()}{"\n"}" +
                        $@"{L()}\\                            //{R()}{"\n"}" +
                        $@"{L()}| \                          / |{R()}{"\n"}"
                        :
                        $@"{L()} o                             o{R()}{"\n"}" +
                        $@"{L()}/LL-------------+-------------JJ{R()}{"\n"}" +
                        $@"{L()}\\                            //{R()}{"\n"}" +
                        $@"{L()}| \                          / |{R()}{"\n"}");
                    Console.WriteLine(Ground);
                    Console.WriteLine();
                    Console.WriteLine(frame_a
                        ? "           *** Mash [A]+[D] or [Left]+[Right] ***"
                        : "           ''' Mash [A]+[D] or [Left]+[Right] '''");
                    Console.WriteLine("           (Press [P] to pause, [Esc] to quit)");

                    Thread.Sleep(500);
                    sleeps++;
                    frame_a = !frame_a;
                }

                bool win = position < 0;
                double seconds = (DateTime.Now - start).TotalSeconds;
                double average = seconds > 0 ? presses / seconds : 0; // Prevent division by zero

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("  Tug Of War");
                Console.WriteLine();
                Console.Write(win
                    ?
                    $@"{L()}o                               {R()}{"\n"}" +
                    $@"{L()}LL------------+------.  o___    {R()}{"\n"}" +
                    $@"{L()}\\                    \//   \\__{R()}{"\n"}" +
                    $@"{L()}| \                    \_____\  {R()}{"\n"}"
                    :
                    $@"{L()}                               o{R()}{"\n"}" +
                    $@"{L()}    ___o  .------+------------JJ{R()}{"\n"}" +
                    $@"{L()}__//   \\/                    //{R()}{"\n"}" +
                    $@"{L()}  /_____/                    / |{R()}{"\n"}");
                Console.WriteLine(Ground);
                Console.WriteLine();
                Console.WriteLine("  You " + (win ? "Win!" : "Lose!"));
                Console.WriteLine($"  Average: {average:0.##} mashes per second");
                Console.WriteLine($"  Required: {requiredMash} mashes per second");
                Console.WriteLine("  [enter] return to menu");
                Console.WriteLine("  [escape] exit game");

                bool enterPressed = false;
                while (!enterPressed)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Enter: enterPressed = true; break;
                        case ConsoleKey.Escape: return;
                    }
                }
            }
        }
        catch (Exception e)
        {
            exception = e;
            throw;
        }
        finally
        {
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine(exception?.ToString() ?? "Tug Of War was closed.");
        }
    }

    // Check if terminal window is large enough
    static bool EnsureTerminalSize(int requiredWidth, int requiredHeight)
    {
        int currentWidth = Console.WindowWidth;
        int currentHeight = Console.WindowHeight;

        if (currentWidth < requiredWidth || currentHeight < requiredHeight)
        {
            Console.Clear();
            Console.WriteLine($"Terminal window too small for optimal gameplay!");
            Console.WriteLine($"Current: {currentWidth}x{currentHeight}");
            Console.WriteLine($"Required: {requiredWidth}x{requiredHeight}");
            Console.WriteLine("\nPlease resize your terminal and press Enter to continue...");
            Console.WriteLine("Or press Escape to exit the game.");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                    return false;
                if (key == ConsoleKey.Enter)
                {
                    currentWidth = Console.WindowWidth;
                    currentHeight = Console.WindowHeight;

                    if (currentWidth >= requiredWidth && currentHeight >= requiredHeight)
                        return true;

                    Console.WriteLine("Terminal still too small. Please resize and try again.");
                }
            }
        }

        return true;
    }

    // Display pause screen
    static void DisplayPauseScreen()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("  Tug Of War - PAUSED");
        Console.WriteLine();
        Console.WriteLine("  Game is paused.");
        Console.WriteLine();
        Console.WriteLine("  Press [P] to continue");
        Console.WriteLine("  Press [Escape] to quit");
    }
}