using System;
using System.Linq;
using System.Text;

namespace InputKeyCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string lineData;
            do
            {
                lineData = Console.ReadLine();
                if (!string.IsNullOrEmpty(lineData))
                {
                    var lineDataChars = lineData.ToCharArray().ToList();
                    lineDataChars.ForEach(c => sb.Append($"Keycode for {c}={(byte) c}\t"));
                    Console.WriteLine(sb.ToString());


                }
                sb.Clear();
            } while ((lineData != "exit") && (lineData != "quit"));
        }
    }
}
