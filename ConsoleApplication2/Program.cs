using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace ConsoleApplication2
{
    public class Program
    {
        // [* CONFIGURATION *]
        private static readonly int width = 10; // Игровое поле
        private static readonly int height = 10; // Игровое поле
        public static int letter, index, shipCount=0,botShipCount=0;
        private static readonly char[,] field = new char[height, width];
        private static readonly char[,] botField = new char[height, width];
        private static readonly char[,] showBotField = new char[height, width];

        private static readonly string[]
            alphabet = {"а", "б", "в", "г", "д", "е", "ж", "з", "и", "к"}; // Нумерация и буквы

        private static readonly string[] numeration = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
        private static string message;
        static Random rand = new Random();
        public static List<User> users;
        private static FileStream path;
        private static XmlSerializer serializer;

        // MENU
        public static void Main(string[] args)
        {
            users = new List<User>();
            Load();

            Console.Title = "Морской бой | Menu";
            Console.WriteLine("Login: ");
            string login = Console.ReadLine();
            Console.WriteLine("Password: ");
            string pass = Console.ReadLine();
            User user = new User(login, pass);


            if (!CheckProfileOnExistance(login, pass)) // Создаем ещё один аккаунт если его там нету
            {
                users.Add(user);
                path = new FileStream("Users.xml", FileMode.OpenOrCreate);
                serializer.Serialize(path, users);
                path.Close();
            }
            else
            {
                Console.WriteLine($"Hello, {login}"); // Вход в существующий аккаунт
                Console.ReadLine();
            }

            // Console.WriteLine("===============MENU===============\n" +
            //                   " Нажмите Enter чтобы Начать Игру\n" +
            //                   " Нажмите Esc чтобы Выйти из игры\n" +
            //                   "==================================\n" +
            //                   "Ваш выбор:");
            //
            // ConsoleKeyInfo keyPressed;
            // keyPressed = Console.ReadKey();
            //
            // switch (keyPressed.Key)
            // {
            //     case ConsoleKey.Enter:
            //         GO();
            //         break;
            //
            //     case ConsoleKey.Escape:
            //         Console.WriteLine(" Выходим из игры...");
            //         Thread.Sleep(500);
            //         break;
            // }
        }

        private static void Load()
        {
            serializer = new XmlSerializer(typeof(List<User>));
            if (!File.Exists("Users.xml")) return;
            
            path = new FileStream("Users.xml", FileMode.Open);
            serializer = new XmlSerializer(typeof(List<User>));
            users = serializer.Deserialize(path) as List<User>;
            path.Close();
        }
        
        public static bool CheckProfileOnExistance(string login, string password)
        {
            if (users is null) 
                return false;
            
            foreach (User user in users)
            {
                if (user.login == login && user.pass == password)
                {
                    return true;
                }
            }
            return false;
        }
        
        private static void GO()
        {
            Console.Title = "Морской бой";
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                showBotField[i, j] = '/';
            generate_field(field, ref shipCount); draw(field,0);
            generate_field(botField, ref botShipCount); draw(botField,13);
            visualizeGame();

            while (true)
            {
                var isValid = true;
                while (isValid)
                {
                    Console.SetCursorPosition(30, 11);
                    Console.Write("Куда стрелять? Введите координаты: ");
                    switch (Console.Read())
                    {
                        case 'а':
                            letter = 0;
                            isValid = false;
                            break;
                        case 'б':
                            letter = 1;
                            isValid = false;
                            break;
                        case 'в':
                            letter = 2;
                            isValid = false;
                            break;
                        case 'г':
                            letter = 3;
                            isValid = false;
                            break;
                        case 'д':
                            letter = 4;
                            isValid = false;
                            break;
                        case 'е':
                            letter = 5;
                            isValid = false;
                            break;
                        case 'ж':
                            letter = 6;
                            isValid = false;
                            break;
                        case 'з':
                            letter = 7;
                            isValid = false;
                            break;
                        case 'и':
                            letter = 8;
                            isValid = false;
                            break;
                        case 'к':
                            letter = 9;
                            isValid = false;
                            break;
                        default:
                            message = "Введите координаты на кириллице, например д5";
                            visualizeGame();
                            break;
                    }
                }

                index = int.Parse(Console.ReadLine()) - 1;
                Hit(index, letter);
                visualizeGame();
            }
        }

        private static void visualizeGame()
        {
            Console.Clear();
            draw(field, 0);
            draw(showBotField, 13);
            Console.SetCursorPosition(30, 7);
            Console.Write("[ИНФОРМАЦИЯ]");
            Console.SetCursorPosition(30, 8);
            Console.Write($"Кол-во ваших кораблей: {shipCount}");
            Console.SetCursorPosition(30, 9);
            Console.Write($"Кол-во кораблей бота: {botShipCount}");
            Console.SetCursorPosition(30, 10);
            Console.Write(message);
        }

        public static void Hit(int x, int y)
        {
            if (showBotField[x, y] == '*' || showBotField[x, y] == 'X')
                message = "Нельзя стрелять в эту клетку";
                
            else if (botField[x, y] == '/')
            {
                showBotField[x, y] = '*';
                message = "Промах!";
                BotHit(rand.Next(1, 10), rand.Next(1, 10));
            }
            else if (botField[x, y] == '\u25A0')
            {
                showBotField[x, y] = 'X';
                message = "Попадание!";
                BotHit(rand.Next(1, 10), rand.Next(1, 10));
                botShipCount--;
            }
        }
        public static void BotHit(int x, int y)
        {
            if (field[x, y] == '*' || field[x, y] == 'X')
                BotHit(rand.Next(1, 10), rand.Next(1, 10));
            
            else if (field[x, y] == '/')
                field[x, y] = '*';
            
            else if (field[x, y] == '\u25A0')
            {
                Console.WriteLine($"dsadasd");
                field[x, y] = 'X';
                shipCount--;
            }
        }

        public static void generate_field(char[,] field, ref int ships)
        {
            char symbol;
            ships = 0;
            for (var x = 0; x < height; x++)
            for (var y = 0; y < width; y++)
            {
                if (rand.Next(1, 100) < 20)
                {
                    symbol = '\u25A0';
                    ships++;
                }
                else
                    symbol = '/';
                field[x, y] = symbol;
            }
        }

        public static void draw(char[,] field, int crusr)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(2 * i + 3, crusr);
                Console.Write(alphabet[i]);
            }

            for (var i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(0, i + crusr + 1);
                Console.Write(numeration[i]);
                Console.SetCursorPosition(2, i + crusr + 1);
                Console.Write("|");
                Console.SetCursorPosition(3, i + crusr);
                for (var j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(2 * j + 3, i + crusr + 1);
                    Console.Write(field[i, j]);
                }

                Console.WriteLine();
            }
        }
    }
}