using FSH.Starter.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Catalog.Products;
public partial class ProductFormDialog
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    [EditorRequired][Parameter] public UpdateProductCommand Model { get; set; } = default!;

    [Parameter] public Action? Refresh { get; set; }

    private MudForm? _form;
    private bool _saving;
    private bool _saveingnew;
    private bool _uploading;

    private const long MaxAllowedSize = 3145728;

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
