using System;
using static System.Console;
using System.Linq;

namespace SeaBattle
{
    public class Game
    {
        // [* CONFIGURATION *]
        private static readonly int width = 20; // Игровое поле
        private static readonly int height = 20; // Игровое поле
        public static int letter, number, shipCount = 0, botShipCount = 0;
        private static readonly char[,] field = new char[height, width];
        private static readonly char[,] botField = new char[height, width];
        private static readonly char[,] showBotField = new char[height, width];

        private static readonly char[]
            Alphabet =
            {
                'а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'к',
                'л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','ю','я'
            };
        
        private static string message;
        static Random rand = new Random();
        private static bool is_game_end;

        public static void Play()
        {
            Title = "Морской бой";


            for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    showBotField[i, j] = '~';
            generate_field(field, ref shipCount);
            Draw(field, 0);
            generate_field(botField, ref botShipCount);
            Draw(botField, height+3);
            visualizeGame();

            while (!is_game_end)
            {
                GetInput();
                visualizeGame();
            }
        }

        private static void GetInput()
        {
            SetCursorPosition(width+20,11);
            Write("Куда стрелять? Введите координаты: ");
            
            
            var input = Convert.ToChar(Read());
            var ind = Array.IndexOf(Alphabet, input);

            if (!int.TryParse(ReadLine(), out number)) return;
            if(number<=height && (ind >= 0 && ind <= width))
            {
                letter = ind;
                Hit(number - 1, letter);
            }
            else message = "Введите координаты на кириллице, например д5";
        }

        private static void visualizeGame()
        {
            Clear();
            Draw(field, 0);
            Draw(showBotField, height+3);
            SetCursorPosition(width+20, 7);
            Write("[ИНФОРМАЦИЯ]");
            SetCursorPosition(width+20, 8);
            Write($"Кол-во ваших кораблей: {shipCount}");
            SetCursorPosition(width+20, 9);
            Write($"Кол-во кораблей бота: {botShipCount}");
            SetCursorPosition(width+20, 10);
            Write(message);
        }

        private static void Hit(int y, int x)
        {
            if (showBotField[y, x] == '*' || showBotField[y, x] == 'X')
                message = "Нельзя стрелять в эту клетку";

            else if (botField[y, x] == '~')
            {
                showBotField[y, x] = '*';
                message = "Промах!";
                BotHit(rand.Next(1, 10), rand.Next(1, 10));
            }
            else if (botField[y, x] == '\u25A0')
            {
                showBotField[y, x] = 'X';
                message = "Попадание!";
                BotHit(rand.Next(1, 10), rand.Next(1, 10));
                botShipCount--;
            }
        }

        private static void BotHit(int x, int y)
        {
            switch (field[x, y])
            {
                case '*':
                case 'X':
                    BotHit(rand.Next(1, 10), rand.Next(1, 10));
                    break;
                case '~':
                    field[x, y] = '*';
                    break;
                case '\u25A0':
                    field[x, y] = 'X';
                    shipCount--;
                    break;
            }
        }

        private static void generate_field(char[,] field, ref int ships)
        {
            ships = 0;
            for (var x = 0; x < height; x++)
            for (var y = 0; y < width; y++)
            {
                char symbol;
                if (rand.Next(1, 100) < 20)
                {
                    symbol = '\u25A0';
                    ships++;
                }
                else
                    symbol = '~';

                field[x, y] = symbol;
            }
        }

        private static void Draw(char[,] field, int crusr)
        {
            for (var i = 0; i < width; i++)
            {
                SetCursorPosition(2 * i + 3, crusr);
                Write(Alphabet[i]);
                for (var j = 0; j < height; j++)
                {
                    SetCursorPosition(0, j + crusr + 1);
                    Write(j+1);
                    SetCursorPosition(2, j + crusr + 1);
                    Write('│');

                    SetCursorPosition(3, j + crusr);
                    SetCursorPosition(2 * j + 3, i + crusr + 1);
                    Write(field[i, j]);
                }

                WriteLine();
            }
        }
    }
}