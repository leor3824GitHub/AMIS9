using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Catalog.Products;
public partial class ProductFormDialog
{
    [CascadingParameter] 
    private MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter]

    public UpdateProductCommand Model { get; set; } = default!;
    [Inject]
    private IApiClient productclient { get; set; } = default!;
    [Parameter] public Action? Refresh { get; set; }
    [Parameter] public bool? IsCreate { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private MudForm? _form;
    private bool _saving;
    private bool _saveingnew;
    private bool _uploading;

    private const long MaxAllowedSize = 3145728;

    private async Task OnValidSubmit()
    {
        //if (_form == null) return;
        //if (!await _form.Validate()) return;

        //_saving = true;
        if (IsCreate.HasValue)
        {
            Snackbar.Add(IsCreate.Value ? "Creating product..." : "Updating product...", Severity.Info);
            if (IsCreate.Value)
            {
                var model = Model.Adapt<CreateProductCommand>();
                var response = await productclient.CreateProductEndpointAsync("1", model);
                if (response != null)
                {
                    MudDialog.Close(DialogResult.Ok(true));
                    Refresh?.Invoke();
                }
            }
            else
            {
                var response = await productclient.UpdateProductEndpointAsync("1", Model.Id, Model);
                if (response != null)
                {
                    MudDialog.Close(DialogResult.Ok(true));
                    Refresh?.Invoke();
                }
            }
        }
        //_saving = false;
    }
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
