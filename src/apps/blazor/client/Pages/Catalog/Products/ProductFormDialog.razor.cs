using FSH.Starter.Blazor.Client.Components;
using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Client.Pages.Catalog.Products;

public partial class ProductFormDialog
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public UpdateProductCommand Model { get; set; } = default!;

    [Inject]
    private IApiClient ProductClient { get; set; } = default!;

    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private string? successMessage;

    private MudForm? _form;
    private bool _uploading;
    private const long MaxAllowedSize = 3145728;

    private async Task OnValidSubmit()
    {
        if (IsCreate == null) return;

        Snackbar.Add(IsCreate.Value ? "Creating product..." : "Updating product...", Severity.Info);

        if (IsCreate.Value) // Create product
        {
            var model = Model.Adapt<CreateProductCommand>();
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ProductClient.CreateProductEndpointAsync("1", model),
                Snackbar,
                Navigation
                
            );

            if (response != null)
            {
                successMessage = "Product created successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
        else // Update product
        {
            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ProductClient.UpdateProductEndpointAsync("1", Model.Id, Model),
                Snackbar,
                Navigation
                
            );

            if (response != null)
            {
                successMessage = "Product updated successfully!";
                MudDialog.Close(DialogResult.Ok(true));
                Refresh?.Invoke();
            }
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
