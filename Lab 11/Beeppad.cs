using System;
using System.Collections.Generic;
using System.Threading;

ConsoleColor ForegroundColor = Console.ForegroundColor;
ConsoleColor BackgroundColor = Console.BackgroundColor;

int ButtonTimeSpan = 500; // milliseconds
int CodeLength = 5;
(int X, int Y) Position = default;

// C major scale, starting with middle C
int[] frequencies =
[
    262,
    294,
    330,
    349,
    392,
    440,
    494,
    523,
    587,
];

if (!OperatingSystem.IsWindows())
{
    Console.WriteLine("Unfortunately this game is not supported on ");
    Console.WriteLine("your operating system. It is Windows only. :(");
    Console.WriteLine("Press enter to close...");
GetInput:
    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.Enter: break;
        case ConsoleKey.Escape: return;
        default: goto GetInput;
    }
    Console.Clear();
    return;
}

try
{
    Console.CursorVisible = false;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
    Queue<int> inputedCode = new();
    int[] answerCode = GetRandomCode();
    ShuffleFrequencies();
    RenderGame();

    // Delay before initial pattern playback
    ShowMessage("🎵 Listen to the pattern!", 1000);
    PlayAnswerAudio(answerCode);
    Thread.Sleep(500);

    while (true)
    {
        RenderGame();
        Console.SetCursorPosition(Position.X * 4 + 6, Position.Y * 2 + 4);
        Console.CursorVisible = true;
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                Position.Y = Position.Y is 0 ? 2 : Position.Y - 1; break;
            case ConsoleKey.DownArrow:
                Position.Y = Position.Y is 2 ? 0 : Position.Y + 1; break;
            case ConsoleKey.LeftArrow:
                Position.X = Position.X is 0 ? 2 : Position.X - 1; break;
            case ConsoleKey.RightArrow:
                Position.X = Position.X is 2 ? 0 : Position.X + 1; break;
            case ConsoleKey.Spacebar:
                Console.Clear();
                ShowMessage("🎵 Listen again!", 1000);
                PlayAnswerAudio(answerCode);
                Thread.Sleep(500);
                break;
            case ConsoleKey.Enter:
                int button = GetButton(Position);
                Console.Write('█');
                Console.CursorVisible = false;
                Console.Beep(frequencies[button - 1], ButtonTimeSpan);
                inputedCode.Enqueue(button);
                if (inputedCode.Count > CodeLength)
                {
                    inputedCode.Dequeue();
                }
                if (InputMatchesAnswer(inputedCode, answerCode))
                {
                    RenderGame(false);
                    Console.WriteLine("    🎉 You Win!");
                    Console.WriteLine();
                    Console.WriteLine("    Press Enter To Close...");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                break;
            case ConsoleKey.Escape:
                Console.Clear();
                Console.Write("BeepPad was closed.");
                return;
        }
    }
}
finally
{
    Console.CursorVisible = true;
    Console.BackgroundColor = BackgroundColor;
    Console.ForegroundColor = ForegroundColor;
}

void ShowMessage(string message, int delayMs)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine($"    {message}");
    Thread.Sleep(delayMs);
}

bool InputMatchesAnswer(Queue<int> inputedCode, int[] answerCode)
{
    if (inputedCode.Count != answerCode.Length)
    {
        return false;
    }
    int i = 0;
    foreach (int number in inputedCode)
    {
        if (number != answerCode[i++])
        {
            return false;
        }
    }
    return true;
}

void PlayAnswerAudio(int[] answerCode)
{
    foreach (int button in answerCode)
    {
        Console.Beep(frequencies[button - 1], ButtonTimeSpan);
        Thread.Sleep(ButtonTimeSpan);
    }
}

int GetButton((int X, int Y) position)
{
    return position.Y switch
    {
        0 => position.X + 1,
        1 => position.X + 4,
        2 => position.X + 7,
        _ => 0,
    };
}

void ShuffleFrequencies()
{
    Random random = new();
    for (int i = frequencies.Length - 1; i > 0; i--)
    {
        int j = random.Next(i + 1);
        (frequencies[i], frequencies[j]) = (frequencies[j], frequencies[i]);
    }
}

int[] GetRandomCode()
{
    Random random = new();
    int[] result = new int[CodeLength];
    for (int i = 0; i < CodeLength; i++)
    {
        result[i] = random.Next(1, 10);
    }
    return result;
}

void RenderGame(bool showCursor = true)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("    Use the arrow keys to move.");
    Console.WriteLine("    Press enter to guess a note.");
    Console.WriteLine("    Press spacebar to hear the pattern again.");
    Console.WriteLine("    Press escape to exit.");
    Console.WriteLine();

    for (int row = 0; row < 3; row++)
    {
        Console.Write("    ");
        for (int col = 0; col < 3; col++)
        {
            WriteHighlighted(row == Position.Y && col == Position.X, (row * 3 + col + 1).ToString(), 4);
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    if (!showCursor)
    {
        Console.CursorVisible = false;
    }
}

void WriteHighlighted(bool isHighlighted, string text, int width)
{
    if (isHighlighted)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }
    Console.Write(text.PadRight(width));
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
}
