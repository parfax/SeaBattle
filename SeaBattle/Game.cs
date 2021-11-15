using System;
using static System.Console;
using System.Linq;

namespace SeaBattle
{
    public class Game
    {
        // [* CONFIGURATION *]
        private const int fieldSize = 10; // Игровое поле
        public static int letter, number;
        private static readonly char[,] showBotField = new char[fieldSize, fieldSize];
        
        private static string message;
        static Random rand = new Random();
        private static bool is_game_end;
        
        private static Field playerField = new Field();
        private static Field botField = new Field();

        public static void Play()
        {
            Title = "Морской бой";


            for (var i = 0; i < fieldSize; i++)
                for (var j = 0; j < fieldSize; j++)
                    showBotField[i, j] = '~';

            
            playerField.Generate(fieldSize);
            botField.Generate(fieldSize);
            
            visualizeGame();

            while (!is_game_end)
            {
                GetInput();
                visualizeGame();
            }
        }

        private static void GetInput()
        {
            SetCursorPosition(fieldSize+20,11);
            Write("Куда стрелять? Введите координаты: ");
            
            
            var input = Convert.ToChar(Read());
            var ind = Array.IndexOf(Field.Alphabet, input);

            if (!int.TryParse(ReadLine(), out number)) return;
            if(number<=fieldSize && (ind >= 0 && ind <= fieldSize))
            {
                letter = ind;
                Hit(number - 1, letter);
            }
            else message = "Введите координаты на кириллице, например д5";
        }

        private static void visualizeGame()
        {
            Clear();
            Draw(playerField.field, 0);
            Draw(showBotField, fieldSize+3);
            SetCursorPosition(fieldSize+20, 7);
            Write("[ИНФОРМАЦИЯ]");
            SetCursorPosition(fieldSize+20, 8);
            Write($"Кол-во ваших кораблей: {playerField.shipQuantity}");
            SetCursorPosition(fieldSize+20, 9);
            Write($"Кол-во кораблей бота: {botField.shipQuantity}");
            SetCursorPosition(fieldSize+20, 10);
            Write(message);
        }

        private static void Hit(int y, int x)
        {
            if (showBotField[y, x] == '*' || showBotField[y, x] == 'X')
                message = "Нельзя стрелять в эту клетку";

            else if (botField.field[y, x] == '~')
            {
                showBotField[y, x] = '*';
                message = "Промах!";
                BotHit(rand.Next(fieldSize), rand.Next(fieldSize));
            }
            else if (botField.field[y, x] == '\u25A0')
            {
                showBotField[y, x] = 'X';
                message = "Попадание!";
                BotHit(rand.Next(fieldSize), rand.Next(fieldSize));
                botField.shipQuantity--;
            }
        }

        private static void BotHit(int x, int y)
        {
            while (true)
            {
                switch (playerField.field[x, y])
                {
                    case '*':
                    case 'X':
                        x = rand.Next(fieldSize);
                        y = rand.Next(fieldSize);
                        continue;
                    case '~':
                        playerField.field[x, y] = '*';
                        break;
                    case '\u25A0':
                        playerField.field[x, y] = 'X';
                        playerField.shipQuantity--;
                        break;
                }

                break;
            }
        }
        

        private static void Draw(char[,] field, int crusr)
        {
            for (var i = 0; i < fieldSize; i++)
            {
                SetCursorPosition(2 * i + 3, crusr);
                Write(Field.Alphabet[i]);
                for (var j = 0; j < fieldSize; j++)
                {
                    SetCursorPosition(0, j + crusr + 1);
                    Write(j+1);
                    SetCursorPosition(2, j + crusr + 1);
                    Write('│');

                    SetCursorPosition(3, j + crusr);
                    SetCursorPosition(2 * j + 3, i + crusr + 1);
                    
                    // if(field[i, j] == '~') ForegroundColor = ConsoleColor.Blue;
                    // else ForegroundColor = ConsoleColor.White;
                    Write(field[i, j]);
                }

                WriteLine();
            }
        }
    }
}