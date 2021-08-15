using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Villa_Server.Helper
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

        public static async ValueTask SwalSuccess(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowSwal", "success", message);
        }

        public static async ValueTask SwalError(this IJSRuntime jsRuntime, string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowSwal", "error", message);
        }

    }
}
