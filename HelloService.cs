using Microsoft.JSInterop;

namespace MauiBlazorJsInteropTest;
public sealed class HelloService : IAsyncDisposable
{
    private Lazy<Task<IJSObjectReference>> moduleTask;
    private const string modulePath = "./_content/MauiBlazorJsInteropTest/hello.js";

    public HelloService(IJSRuntime jSRuntime)
    {
        moduleTask = new(() => jSRuntime.InvokeAsync<IJSObjectReference>(
            "import", modulePath).AsTask());
    }

    public async Task PromptHello()
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("promptHello");
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
