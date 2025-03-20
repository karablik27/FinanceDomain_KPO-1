using System;
namespace FinanceApp
{
    public class Colors
    {
        // Enum for choose color that you want.
        public enum ConsoleColorType
        {
            Black,
            DarkBlue,
            DarkGreen,
            DarkCyan,
            DarkRed,
            DarkMagenta,
            DarkYellow,
            Gray,
            DarkGray,
            Blue,
            Green,
            Cyan,
            Red,
            Magenta,
            Yellow,
            White
        }


        /// <summary> 
        /// Prints the message to the console with the color, that u choose. 
        /// </summary> 
        /// <param name="message">The message to be printed.</param> 
        /// <param name="colorType">The text color for the console output.</param> 
        public static void PrintLine(string message, ConsoleColorType colorType)
        {
            ConsoleColor color;

            switch (colorType)
            {
                case ConsoleColorType.Black:
                    color = ConsoleColor.Black;
                    break;
                case ConsoleColorType.DarkBlue:
                    color = ConsoleColor.DarkBlue;
                    break;
                case ConsoleColorType.DarkGreen:
                    color = ConsoleColor.DarkGreen;
                    break;
                case ConsoleColorType.DarkCyan:
                    color = ConsoleColor.DarkCyan;
                    break;
                case ConsoleColorType.DarkRed:
                    color = ConsoleColor.DarkRed;
                    break;
                case ConsoleColorType.DarkMagenta:
                    color = ConsoleColor.DarkMagenta;
                    break;
                case ConsoleColorType.DarkYellow:
                    color = ConsoleColor.DarkYellow;
                    break;
                case ConsoleColorType.Gray:
                    color = ConsoleColor.Gray;
                    break;
                case ConsoleColorType.DarkGray:
                    color = ConsoleColor.DarkGray;
                    break;
                case ConsoleColorType.Blue:
                    color = ConsoleColor.Blue;
                    break;
                case ConsoleColorType.Green:
                    color = ConsoleColor.Green;
                    break;
                case ConsoleColorType.Cyan:
                    color = ConsoleColor.Cyan;
                    break;
                case ConsoleColorType.Red:
                    color = ConsoleColor.Red;
                    break;
                case ConsoleColorType.Magenta:
                    color = ConsoleColor.Magenta;
                    break;
                case ConsoleColorType.Yellow:
                    color = ConsoleColor.Yellow;
                    break;
                case ConsoleColorType.White:
                    color = ConsoleColor.White;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }



        /// <summary> 
        /// Prints the message to the console with the color, that u choose. 
        /// </summary> 
        /// <param name="message">The message to be printed.</param> 
        /// <param name="colorType">The text color for the console output.</param> 
        public static void Print(string message, ConsoleColorType colorType)
        {
            ConsoleColor color;

            switch (colorType)
            {
                case ConsoleColorType.Black:
                    color = ConsoleColor.Black;
                    break;
                case ConsoleColorType.DarkBlue:
                    color = ConsoleColor.DarkBlue;
                    break;
                case ConsoleColorType.DarkGreen:
                    color = ConsoleColor.DarkGreen;
                    break;
                case ConsoleColorType.DarkCyan:
                    color = ConsoleColor.DarkCyan;
                    break;
                case ConsoleColorType.DarkRed:
                    color = ConsoleColor.DarkRed;
                    break;
                case ConsoleColorType.DarkMagenta:
                    color = ConsoleColor.DarkMagenta;
                    break;
                case ConsoleColorType.DarkYellow:
                    color = ConsoleColor.DarkYellow;
                    break;
                case ConsoleColorType.Gray:
                    color = ConsoleColor.Gray;
                    break;
                case ConsoleColorType.DarkGray:
                    color = ConsoleColor.DarkGray;
                    break;
                case ConsoleColorType.Blue:
                    color = ConsoleColor.Blue;
                    break;
                case ConsoleColorType.Green:
                    color = ConsoleColor.Green;
                    break;
                case ConsoleColorType.Cyan:
                    color = ConsoleColor.Cyan;
                    break;
                case ConsoleColorType.Red:
                    color = ConsoleColor.Red;
                    break;
                case ConsoleColorType.Magenta:
                    color = ConsoleColor.Magenta;
                    break;
                case ConsoleColorType.Yellow:
                    color = ConsoleColor.Yellow;
                    break;
                case ConsoleColorType.White:
                    color = ConsoleColor.White;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }

            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

    }
}

