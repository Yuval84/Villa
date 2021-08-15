
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Villa_Client.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

    }
}
