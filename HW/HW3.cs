// Build a task scheduler
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduler
{
    // Simple task priority enum
    public enum TaskPriority
    {
        Low,
        Normal,
        High
    }

    // Interface for task definition
    public interface IScheduledTask
    {
        string Name { get; }
        TaskPriority Priority { get; }
        TimeSpan Interval { get; }
        DateTime LastRun { get; }
        Task ExecuteAsync();
    }

    // A basic implementation of a scheduled task
    public class SimpleTask : IScheduledTask
    {
        private readonly Func<Task> _action;
        private DateTime _lastRun = DateTime.MinValue;

        public string Name { get; }
        public TaskPriority Priority { get; }
        public TimeSpan Interval { get; }

        public DateTime LastRun => _lastRun;

        public SimpleTask(string name, TaskPriority priority, TimeSpan interval, Func<Task> action)
        {
            Name = name;
            Priority = priority;
            Interval = interval;
            _action = action;
        }

        public async Task ExecuteAsync()
        {
            await _action();
            _lastRun = DateTime.Now;
        }
    }

    // The scheduler that students need to implement
    public class TaskScheduler
    {
        // TODO: Implement task queue/storage mechanism
        List<IScheduledTask> listTask;

        public TaskScheduler()
        {
            // TODO: Initialize your scheduler
            listTask = new List<IScheduledTask>();
        }

        public void AddTask(IScheduledTask task)
        {
            listTask.Add(task);
            // TODO: Add task to the scheduler
            //throw new NotImplementedException();
        }

        public void RemoveTask(string taskName)
        {
            //foreach(var t in listTask)
            //{
            //    if(taskName == t.Name)
            //    {
            //        listTask.Remove(t);
            //    }
            //}
            listTask.RemoveAll(t => t.Name == taskName);
            // TODO: Remove task from the scheduler
            //throw new NotImplementedException();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                // Lọc các task đủ điều kiện chạy (đã đến thời điểm chạy lại)
                var readyTasks = new List<IScheduledTask>();
                foreach (var task in listTask)
                {
                    if ((now - task.LastRun) >= task.Interval)
                    {
                        readyTasks.Add(task);
                    }
                }

                // Ưu tiên: sắp xếp theo Priority giảm dần (High -> Low)
                readyTasks.Sort((a, b) => b.Priority.CompareTo(a.Priority));

                // Chạy từng task theo thứ tự ưu tiên
                foreach (var task in readyTasks)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Executing: {task.Name} ({task.Priority})");

                    try
                    {
                        await task.ExecuteAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in task '{task.Name}': {ex.Message}");
                    }
                }

                // Tránh CPU chạy 100% bằng cách delay nhẹ giữa các vòng lặp
                await Task.Delay(500, cancellationToken);
            }
            // TODO: Implement the scheduling logic
            // - Run higher priority tasks first
            // - Only run tasks when their interval has elapsed since LastRun
            // - Keep running until cancellation is requested
            //throw new NotImplementedException();
        }

        public List<IScheduledTask> GetScheduledTasks()
        {
            return listTask;
            // TODO: Return a list of all scheduled tasks
            //throw new NotImplementedException();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Task Scheduler Demo");

            // Create the scheduler
            var scheduler = new TaskScheduler();

            // Add some tasks
            scheduler.AddTask(new SimpleTask(
                "High Priority Task",
                TaskPriority.High,
                TimeSpan.FromSeconds(2),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running high priority task");
                    await Task.Delay(500); // Simulate some work
                }
            ));

            scheduler.AddTask(new SimpleTask(
                "Normal Priority Task",
                TaskPriority.Normal,
                TimeSpan.FromSeconds(3),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running normal priority task");
                    await Task.Delay(300); // Simulate some work
                }
            ));

            scheduler.AddTask(new SimpleTask(
                "Low Priority Task",
                TaskPriority.Low,
                TimeSpan.FromSeconds(4),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running low priority task");
                    await Task.Delay(200); // Simulate some work
                }
            ));
            //Answer:
            Console.WriteLine("List of task:");
            foreach (var task in scheduler.GetScheduledTasks()) {
                Console.WriteLine($"{task.Name} | {task.Interval} | {task.Priority} | {task.LastRun}");
            }
            Console.WriteLine("List of task after remove:");
            //scheduler.RemoveTask("High Priority Task");
            //foreach (var task in scheduler.GetScheduledTasks())
            //{
            //    Console.WriteLine($"{task.Name} | {task.Interval} | {task.Priority} | {task.LastRun}");
            //}

            // Create a cancellation token that will cancel after 20 seconds
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

            // Or allow the user to cancel with a key press
            Console.WriteLine("Press any key to stop the scheduler...");

            // Run the scheduler in the background
            var schedulerTask = scheduler.StartAsync(cts.Token);

            // Wait for user input
            Console.ReadKey();
            cts.Cancel();

            try
            {
                await schedulerTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Scheduler stopped by cancellation.");
            }

            Console.WriteLine("Scheduler demo finished!");
        }
    }
}