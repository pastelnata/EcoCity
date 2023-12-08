using System.Text;

namespace world_of_zuul
{
    public static class VisualTextWriter
    {

        private static int sleep = 15;

// displays individual letters on the console consecutively, one after another
        public static void Write(string? word = "")
        {
            if (word != null)
            {
                foreach (char i in word)
                {
                    Console.Write(i);
                    Thread.Sleep(sleep);
                }
            }
        }

//  displays individual letters on the console consecutively, one after another, followed by a newline
        public static void WriteLine(string? word = "")
        {
            if (word != null)
            {
                foreach (char i in word)
                {
                    Console.Write(i);
                    Thread.Sleep(sleep);
                }
                
            }
            Thread.Sleep(20*sleep);
            Console.WriteLine();
        }

// Resets the font color to default
        public static void ColorReset()
        {
            Console.ResetColor();
        }



// SetColor() - Changes font color
/*
Available colors:
                    Black
                    DarkBlue
                    DarkGreen
                    DarkCyan
                    DarkRed
                    DarkMagenta
                    DarkYellow
                    Gray
                    DarkGray
                    Blue
                    Green
                    Cyan
                    Red
                    Magenta
                    Yellow
                    White 
*/
        public static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}