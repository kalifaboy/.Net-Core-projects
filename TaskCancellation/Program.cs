using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ExecuteTaskAsync().ConfigureAwait(false);

            await ExecuteTaskWithTimeoutAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            await ExecuteTaskWithTimeoutAsync(TimeSpan.FromSeconds(0.5)).ConfigureAwait(false);//will throw exception

            await ExecuteManuallyCancellableTaskAsync();

            await CancelANonCancellableTaskAsync();
        }

        private static async Task CancelANonCancellableTaskAsync()
        {
            Console.WriteLine(nameof(CancelANonCancellableTaskAsync));

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var keyBoardTask = Task.Run(() =>
                {
                    Console.WriteLine("Press enter to cancel");
                    Console.ReadKey();

                    cancellationTokenSource.Cancel();
                });

                try
                {
                    var result = await LongRunningOperationWithCancellationTokenAsync(100, cancellationTokenSource.Token);
                    Console.WriteLine("Result {0}", result);
                    Console.WriteLine("Press enter to continue");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }

                await keyBoardTask;
            }
        }

        private static async Task<decimal> LongRunningOperationWithCancellationTokenAsync(int loop, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<decimal>();

            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());

            var task = LongRunningOperation(loop);

            var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);

            return await completedTask;
        }

        private static async Task ExecuteManuallyCancellableTaskAsync()
        {
            Console.WriteLine(nameof(ExecuteManuallyCancellableTaskAsync));

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var keyBoardTask = Task.Run(() =>
                {
                    Console.WriteLine("Press enter to cancel");
                    Console.ReadKey();

                    cancellationTokenSource.Cancel();
                });

                try
                {
                    var longRunnigTaskResult = await LongRunningCancellableOperation(100, cancellationTokenSource.Token).ConfigureAwait(false);
                    Console.WriteLine("Result {0}", longRunnigTaskResult);
                    Console.WriteLine("Press enter to continue");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }

                await keyBoardTask;
            }
        }

        private static async Task ExecuteTaskWithTimeoutAsync(TimeSpan timeSpan)
        {
            Console.WriteLine(nameof(ExecuteTaskWithTimeoutAsync));

            using var cancellationTokenSource = new CancellationTokenSource(timeSpan);
            try
            {
                var result = await LongRunningCancellableOperation(100, cancellationTokenSource.Token).ConfigureAwait(false);
                Console.WriteLine("Result {0}", result);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task was cancelled");
            }

            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

        private static async Task ExecuteTaskAsync()
        {
            Console.WriteLine(nameof(ExecuteTaskAsync));
            Console.WriteLine("Result {0}", await LongRunningOperation(100).ConfigureAwait(false));
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

        private static Task<decimal> LongRunningCancellableOperation(int loop, CancellationToken cancellationToken)
        {
            Task<decimal> task = null;

            task = Task.Run(() => 
            {
                decimal result = 0;

                for (int i = 0; i < loop; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new TaskCanceledException(task);

                    cancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(10);
                    result += 1;
                }

                return result;
            });

            return task;
        }

        private static Task<decimal> LongRunningOperation(int loop)
        {
            return Task.Run(() =>
            {
                decimal result = 0;

                for (int i = 0; i < loop; i++)
                {
                    Thread.Sleep(10);
                    result += 1;
                }
                return result;
            });
        }
    }
}
