using System.Diagnostics;
using System.Text;

namespace DelegatesLinQ2
{
    // Delegate types for processing pipeline
    public delegate string DataProcessor(string input);
    public delegate void ProcessingEventHandler(string stage, string input, string output);

    /// <summary>
    /// Homework 2: Custom Delegate Chain
    /// Create a data processing pipeline using multicast delegates.
    /// 
    /// Requirements:
    /// 1. Create a processing pipeline that transforms text data through multiple steps
    /// 2. Use multicast delegates to chain processing operations
    /// 3. Add logging/monitoring capabilities using events
    /// 4. Demonstrate adding and removing processors from the chain
    /// 5. Handle errors in the processing chain
    /// 
    /// Techniques used: Similar to 6_2_MulticastDelegate
    /// - Multicast delegate chaining
    /// - Delegate combination and removal
    /// - Error handling in delegate chains
    /// </summary>
    public class DataProcessingPipeline
    {
        // TODO: Declare events for monitoring the processing
        // public event ProcessingEventHandler ProcessingStageCompleted;

        public event ProcessingEventHandler ProcessingStageCompleted;

        // Individual processing methods that students need to implement
        protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
        {
            ProcessingStageCompleted?.Invoke(stage, input, output);
        }
        public static string RemoveSpaces(string input)
        {
            // TODO: Remove all spaces from input
            //throw new NotImplementedException("Students need to implement this method");
            string output = input.Replace(" ", "");
            return output;
        }

        public static string ToUpperCase(string input)
        {
            // TODO: Convert input to uppercase
            //throw new NotImplementedException("Students need to implement this method");
            return input.ToUpper();
        }

        public static string AddTimestamp(string input)
        {
            // TODO: Add current timestamp to the beginning of input
            //throw new NotImplementedException("Students need to implement this method");
            return $"[{DateTime.Now:HH:mm:ss}] {input}";
        }

        public static string ReverseString(string input)
        {
            // TODO: Reverse the characters in the input string
            //throw new NotImplementedException("Students need to implement this method");
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        public static string EncodeBase64(string input)
        {
            // TODO: Encode the input string to Base64
            //throw new NotImplementedException("Students need to implement this method");
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string ValidateInput(string input)
        {
            // TODO: Validate input (throw exception if null or empty)
            //throw new NotImplementedException("Students need to implement this method");
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");
            return input;
        }

        // Method to process data through the pipeline
        public string ProcessData(string input, DataProcessor pipeline)
        {
            // TODO: Process input through the pipeline and raise events
            // Handle any exceptions that occur during processing
            //throw new NotImplementedException("Students need to implement this method");
            string currentInput = input;
            string output = input;

            foreach (DataProcessor handler in pipeline.GetInvocationList())
            {
                try
                {
                    output = handler(currentInput);
                    OnProcessingStageCompleted(handler.Method.Name, currentInput, output);
                    currentInput = output;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in stage '{handler.Method.Name}': {ex.Message}");
                    break;
                }
            }

            return output;
        }

        // TODO: Add method to raise processing events
        // protected virtual void OnProcessingStageCompleted(string stage, string input, string output)
    }

    // Logger class to monitor processing
    public class ProcessingLogger
    {
        // TODO: Implement event handler to log processing stages
        // public void OnProcessingStageCompleted(string stage, string input, string output)
        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            Console.WriteLine($"[LOG] Stage: {stage}, Input: \"{input}\", Output: \"{output}\"");
        }
    }

    // Performance monitor class
    public class PerformanceMonitor
    {
        // TODO: Track processing times and performance metrics
        // public void OnProcessingStageCompleted(string stage, string input, string output)
        // public void DisplayStatistics()
        private Dictionary<string, List<long>> timingData = new();

        public void OnProcessingStageCompleted(string stage, string input, string output)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Stop();
            long elapsed = sw.ElapsedMilliseconds;

            if (!timingData.ContainsKey(stage))
                timingData[stage] = new List<long>();
            timingData[stage].Add(elapsed);
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== Performance Statistics ===");
            foreach (var kvp in timingData)
            {
                double avg = kvp.Value.Count > 0 ? (double)kvp.Value.Sum() / kvp.Value.Count : 0;
                Console.WriteLine($"Stage: {kvp.Key}, Calls: {kvp.Value.Count}, Avg Time: {avg:F2} ms");
            }
        }
    }

    public class DelegateChain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 2: CUSTOM DELEGATE CHAIN ===");
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Implement the DataProcessingPipeline class");
            Console.WriteLine("2. Implement all processing methods (RemoveSpaces, ToUpperCase, etc.)");
            Console.WriteLine("3. Create a multicast delegate chain for processing");
            Console.WriteLine("4. Add logging and monitoring capabilities");
            Console.WriteLine("5. Demonstrate adding/removing processors from the chain");
            Console.WriteLine();

            // TODO: Students should implement the following:
            
            DataProcessingPipeline pipeline = new DataProcessingPipeline();
            ProcessingLogger logger = new ProcessingLogger();
            PerformanceMonitor monitor = new PerformanceMonitor();

            // Subscribe to events
            pipeline.ProcessingStageCompleted += logger.OnProcessingStageCompleted;
            pipeline.ProcessingStageCompleted += monitor.OnProcessingStageCompleted;

            // Create processing chain
            DataProcessor processingChain = DataProcessingPipeline.ValidateInput;
            processingChain += DataProcessingPipeline.RemoveSpaces;
            processingChain += DataProcessingPipeline.ToUpperCase;
            processingChain += DataProcessingPipeline.AddTimestamp;

            // Test the pipeline
            string testInput = "Hello World from C#";
            Console.WriteLine($"Input: {testInput}");
            
            string result = pipeline.ProcessData(testInput, processingChain);
            Console.WriteLine($"Output: {result}");

            // Demonstrate adding more processors
            processingChain += DataProcessingPipeline.ReverseString;
            processingChain += DataProcessingPipeline.EncodeBase64;

            // Test again with extended pipeline
            result = pipeline.ProcessData("Extended Pipeline Test", processingChain);
            Console.WriteLine($"Extended Output: {result}");

            // Demonstrate removing a processor
            processingChain -= DataProcessingPipeline.ReverseString;
            result = pipeline.ProcessData("Without Reverse", processingChain);
            Console.WriteLine($"Modified Output: {result}");

            // Display performance statistics
            monitor.DisplayStatistics();

            // Error handling test
            try
            {
                result = pipeline.ProcessData(null, processingChain); // Should handle null input
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handled: {ex.Message}");
            }
            

            Console.WriteLine("Please implement the missing code to complete this homework!");

            // Example of what the complete implementation should demonstrate:
            Console.WriteLine("\nExpected behavior:");
            Console.WriteLine("1. Chain multiple text processing operations");
            Console.WriteLine("2. Log each processing stage");
            Console.WriteLine("3. Monitor performance of each operation");
            Console.WriteLine("4. Handle errors gracefully");
            Console.WriteLine("5. Allow dynamic modification of the processing chain");

            Console.ReadKey();
        }
    }
}
