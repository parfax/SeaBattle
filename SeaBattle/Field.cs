using System;

namespace SeaBattle
{
    public class Field
    {
        public char[,] field;
        public int shipQuantity;

        public static readonly char[] Alphabet =
            {
                'а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'к',
                'л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','ю','я'
            };

        private static Random _rand = new Random();
        
        public void Generate(int fieldSize)
        {
            field = new char[fieldSize,fieldSize];
            shipQuantity = 0;
            
            for (var x = 0; x < fieldSize; x++)
            for (var y = 0; y < fieldSize; y++)
            {
                char symbol;
                if (_rand.Next(1, 100) < 20)
                {
                    symbol = '\u25A0';
                    shipQuantity++;
                }
                else symbol = '~';

                field[x, y] = symbol;
            }
        }
    }
}