﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Guacamole.Client.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<TResult> ThrowTimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));

                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }

                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}
