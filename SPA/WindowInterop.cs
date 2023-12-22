using Microsoft.JSInterop;

namespace SPA
{
    public static class WindowInterop
    {
        public static async Task OpenNewWindow(IJSRuntime jsRuntime, string url)
        {
            await jsRuntime.InvokeVoidAsync("openNewWindow", url);
        }

        public static async Task PrintWindow(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("printWindow");
        }
    }
}
