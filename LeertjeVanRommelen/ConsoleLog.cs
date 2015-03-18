using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Exception(Exception e)
        {
            var messages = CollectMessages(e);
            var message = string.Join(Environment.NewLine, messages);
            
            WriteWithColor(ConsoleColor.DarkGray, message);

            Console.WriteLine("");
        }

        private IEnumerable<string> CollectMessages(Exception exception)
        {
            if (exception != null)
            {
                var result = new[] { exception.Message };
                return result.Concat(CollectMessages(exception.InnerException));
            }
            return new string[0];
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