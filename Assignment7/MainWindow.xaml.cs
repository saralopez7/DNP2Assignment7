using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Assignment7
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();
        private IList<Task> _tasks = new List<Task>(3);

        public MainWindow()
        {
            InitializeComponent();
        }

        public void HeavyWork()
        {
            double secondsToSleep = _random.NextDouble() * 10;

            Thread.Sleep(TimeSpan.FromSeconds(secondsToSleep));
            Console.WriteLine($"{secondsToSleep}");
        }

        public Task HeavyWorkAsync()
        {
            return Task.Run(() => HeavyWork());
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.Invoke(() =>
            {


                _tasks.Add(Task.Factory.StartNew(async () =>
                {
                    await HeavyWorkAsync();
                    Dispatcher.Invoke(() => textLabel.Content += "Executing task 1...\n");
                }));

                _tasks.Add(Task.Factory.StartNew(async () =>
                {
                    await HeavyWorkAsync();
                    Dispatcher.Invoke(() => textLabel.Content += "Executing task 2...\n");
                }));

                _tasks.Add(Task.Factory.StartNew(async () =>
                {
                    await HeavyWorkAsync();
                    Dispatcher.Invoke(() => textLabel.Content += "Executing task 3...\n");
                }));
            });
            await Task.WhenAll(_tasks).ContinueWith(c =>
            {
                Dispatcher.Invoke(() => textLabel.Content += "Tasks execution finished\n");
            });

            stopWatch.Stop();
            //#region 1 and 2. Start 3 HeavyWork tasks so that they run in parallel.

            //Parallel.Invoke(async () =>
            //{
            //    2.Make sure to use your label to indicate that the operation has started. 
            //    Dispatcher.Invoke(() => textLabel.Content += "Executing task 1...\n");
            //    _tasks.Add(HeavyWorkAsync());

            //    Dispatcher.Invoke(() => textLabel.Content += "Executing task 2...\n");
            //    _tasks.Add(HeavyWorkAsync());

            //    Dispatcher.Invoke(() => textLabel.Content += "Executing task 3...\n");
            //    _tasks.Add(HeavyWorkAsync());

            //    await Task.Delay(8000).ContinueWith((x) =>
            //    {
            //        if (_tasks.Any(task => !task.IsCompleted))
            //        {
            //            Dispatcher.Invoke(() => textLabel.Content += "Task hasn´t yet completed...\n");
            //        }
            //    });
            //});

            //#endregion

            //#region 3. When all of the 3 tasks are complete, indicate this completion

            //await Task.WhenAll(_tasks);
            //Dispatcher.Invoke(() => textLabel.Content += "Tasks execution finished\n");
            //stopWatch.Stop();
            //Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", stopWatch.Elapsed);

            //#endregion


        }
    }
}