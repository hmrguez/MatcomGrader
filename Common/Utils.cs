namespace Common;

public static class Utils
{
    public static async Task RunWithTimeoutAsync(Func<CancellationToken, Task> func, TimeSpan timeout)
    {
        using (var cts = new CancellationTokenSource())
        {
            // Start the target function
            var task = func(cts.Token);

            // Create a task that completes after the timeout
            var delayTask = Task.Delay(timeout);

            // Wait for either the function to complete or the timeout to occur
            var completedTask = await Task.WhenAny(task, delayTask);

            if (completedTask == delayTask)
            {
                // Timeout occurred, request cancellation
                cts.Cancel();

                try
                {
                    // Await the task to observe the cancellation exception
                    await task;
                }
                catch (OperationCanceledException)
                {
                    // Handle the cancellation gracefully
                    throw new TimeoutException("The operation has timed out.");
                }
            }
            else
            {
                // The function completed before the timeout
                await task; // Ensure any exceptions/cancellation are observed
            }
        }
    }
}
