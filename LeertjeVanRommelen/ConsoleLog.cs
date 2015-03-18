using System;

namespace LeertjeVanRommelen
{
    internal class ConsoleLog : ILog
    {
        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Error(string message, params object[] args)
        {
            WriteWithColor(ConsoleColor.Red, message, args);
        }

        private void WriteWithColor(ConsoleColor color, string message, params object[] args)
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message, args);
            Console.ForegroundColor = previousColor;
        }
    }
}