using EvaluaIndividuos;
using System.Collections.Concurrent;

public class WebhookCoordinator
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<WebhooksVerazPostRequest>> _pendingRequests = new();

    public Task<WebhooksVerazPostRequest> WaitForResponseAsync(string requestId, TimeSpan timeout)
    {
        var tcs = new TaskCompletionSource<WebhooksVerazPostRequest>(TaskCreationOptions.RunContinuationsAsynchronously);
        _pendingRequests.TryAdd(requestId, tcs);

        var cts = new CancellationTokenSource(timeout);
        cts.Token.Register(() =>
        {
            if (_pendingRequests.TryRemove(requestId, out _))
            {
                tcs.TrySetCanceled();
            }
        });

        return tcs.Task;
    }

    public void CompleteRequest(string requestId, WebhooksVerazPostRequest payload)
    {
        if (_pendingRequests.TryRemove(requestId, out var tcs))
        {
            tcs.TrySetResult(payload);
        }
    }
}