using Microsoft.JSInterop;

namespace CircuitBreakerApp
{
    public static class WindowInterop
    {
        public static async Task<object> OpenNewWindow(IJSRuntime jsRuntime, string url)
        {
            return await jsRuntime.InvokeAsync<object>("openNewWindow", url);
        }

        public static async Task PrintWindow(IJSRuntime jsRuntime, object window)
        {
            await jsRuntime.InvokeVoidAsync("printWindow", window);
        }
    }
}
