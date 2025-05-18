using System;

namespace FileAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("File Analyzer - .NET Core");
            Console.WriteLine("This tool analyzes text files and provides statistics.");

            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command-line argument.");
                Console.WriteLine("Example: dotnet run myfile.txt");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' does not exist.");
                return;
            }

            try
            {
                Console.WriteLine($"Analyzing file: {filePath}");

                // Read the file content
                string content = File.ReadAllText(filePath);

                // TODO: Implement analysis functionality
                // 1. Count words
                // 2. Count characters (with and without whitespace)
                // 3. Count sentences
                // 4. Identify most common words
                // 5. Average word length

                // Example implementation for counting lines:
                int lineCount = File.ReadAllLines(filePath).Length;
                Console.WriteLine($"Number of lines: {lineCount}");

                // TODO: Additional analysis to be implemented

                //Q1: 
                Console.WriteLine("Content of File: ");
                Console.WriteLine(content);
                Console.WriteLine("------------Q1---------------");
                char[] delimiters = {' ',',','.'};
                string[] Words = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                //foreach (string s in Words)
                //{
                //    Console.WriteLine(s);
                //}
                Console.WriteLine($"Number of words in File {filePath}: {Words.Length}");

                //Q2:
                Console.WriteLine("------------Q2---------------");
                Console.WriteLine($"Number of characters in File {filePath} (with whitespace): {content.Length}");

                int cntCharacter = 0;
                foreach (char c in content)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        cntCharacter++;
                    }
                }
                Console.WriteLine($"Number of characters in File {filePath} (without whitespace): {cntCharacter}");
                //Q3:
                Console.WriteLine("------------Q3---------------");
                string[] sentence = content.Split('.',StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in sentence)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine($"Number of sentence in File {filePath}: {sentence.Length}");
                //Q4:
                Console.WriteLine("------------Q4---------------");
                Dictionary<string, int> map = new Dictionary<string, int>();
                foreach (string s in Words)
                {
                    string lower = s.ToLower();
                    if (map.ContainsKey(lower))
                    {
                        map[lower] += 1;
                    }
                    else
                    {
                        map[lower] = 1;
                    }
                }   
                foreach(var pair in map)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value}"); 
                }
                var mostCommon = map.OrderByDescending(x => x.Value).First();
                Console.WriteLine($"Most common word: {mostCommon.Key} ({mostCommon.Value} times)");
                //Q5:
                Console.WriteLine("------------Q5---------------");
                int totalWordLength = 0;
                foreach (string word in Words)
                {
                    totalWordLength += word.Length;
                }
                double avgWordLength = Words.Length > 0 ? (double)totalWordLength / Words.Length : 0;
                Console.WriteLine($"Average word length: {avgWordLength:F2} characters");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error during file analysis: {ex.Message}");
            }
        }
    }
}
