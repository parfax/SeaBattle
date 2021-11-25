using System;
using static System.Console;

namespace SeaBattle
{
    public class UI
    {
        public void Write(ConsoleColor color, string text)
        {
            ForegroundColor = color;
            WriteLine(text);
            ResetColor();
        }
    }
}