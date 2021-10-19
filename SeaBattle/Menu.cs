using System;
using static System.Console;

namespace SeaBattle
{
    public static class Menu
    {
        public static void ReturnToTheMenu()
        {
            Title = "Морской бой | Главное меню";
            
            
            WriteLine("===============MENU===============\n" +
                              " Нажмите Enter чтобы Начать Игру\n" +
                              " Нажмите Esc чтобы Выйти из игры\n" +
                              "==================================\n" +
                              "Ваш выбор:");
            
            ConsoleKeyInfo keyPressed;
            keyPressed = ReadKey();
            
            switch (keyPressed.Key)
            {
                case ConsoleKey.Enter:
                    Game.Play();
                    break;
            
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    return;
            }
        }
    }
}