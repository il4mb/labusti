using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Remote
{
    class CommandInterpreter
    {
        private static readonly Regex OptionRegex = new Regex(@"-(\w+)(?:\s+([^-\s].*?))?(?=\s|$)", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            string commandLine = string.Join(" ", args);

            // Parse the command line
            var options = ParseCommandLine(commandLine);

            // Process the options
            ProcessOptions(options);
        }

        public static Dictionary<string, string> ParseCommandLine(string commandLine)
        {
            var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var matches = OptionRegex.Matches(commandLine);

            foreach (Match match in matches)
            {
                string option = match.Groups[1].Value;
                string value = match.Groups[2].Value.Trim(' ', '"', '\'');

                options[option] = value;
            }

            return options;
        }

        public static void ProcessOptions(Dictionary<string, string> options)
        {
            foreach (var option in options)
            {
                Console.WriteLine($"Option: {option.Key}, Value: {option.Value}");

                // Handle specific options
                switch (option.Key.ToLower())
                {
                    case "a":
                        // Handle -a option
                        Console.WriteLine($"CPU Affinity: {option.Value}");
                        break;

                    case "c":
                        // Handle -c option
                        Console.WriteLine($"Copy Program: {option.Value}");
                        break;

                    case "d":
                        // Handle -d option
                        Console.WriteLine("Don't wait for process to terminate");
                        break;

                    case "e":
                        // Handle -e option
                        Console.WriteLine("Does not load profile");
                        break;

                    case "f":
                        // Handle -f option
                        Console.WriteLine("Force copy file");
                        break;

                    case "i":
                        // Handle -i option
                        Console.WriteLine($"Interactive session: {option.Value}");
                        break;

                    case "n":
                        // Handle -n option
                        Console.WriteLine($"Timeout: {option.Value}");
                        break;

                    case "p":
                        // Handle -p option
                        Console.WriteLine($"Password: {option.Value}");
                        break;

                    case "u":
                        // Handle -u option
                        Console.WriteLine($"User: {option.Value}");
                        break;

                    case "w":
                        // Handle -w option
                        Console.WriteLine($"Working Directory: {option.Value}");
                        break;

                    case "x":
                        // Handle -x option
                        Console.WriteLine("Display UI on secure desktop");
                        break;

                    // Add more options here as needed

                    default:
                        Console.WriteLine($"Unknown option: {option.Key}");
                        break;
                }
            }
        }
    }
}
