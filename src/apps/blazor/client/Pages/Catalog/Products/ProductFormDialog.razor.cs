using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Catalog.Products;
public partial class ProductFormDialog
{
    [CascadingParameter] 
    private MudDialogInstance MudDialog { get; set; } = default!;
    //[Parameter]
    //[EditorRequired]
    //public TRequest Model { get; set; } = default!;
    public CreateProductCommand Model { get; set; } = new();
    [Inject]
    private IApiClient productclient { get; set; } = default!;
    [Parameter] public Action? Refresh { get; set; }

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
        var response = await productclient.CreateProductEndpointAsync("1", Model);
        //_saving = false;
        if (response != null)
        {
            MudDialog.Close(DialogResult.Ok(true));
            Refresh?.Invoke();
        }
    }
    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
