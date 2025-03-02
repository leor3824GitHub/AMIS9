using System.Reflection;
using System.Runtime.CompilerServices;
using FSH.Starter.Blazor.Client.Components;
using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Components.EntityTable;
using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using FSH.Starter.Shared.Authorization;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.Catalog.Products;
public partial class Products_table
{
    private List<BrandResponse> _brands = new();
    private MudDataGrid<ProductResponse> _table = default!;
    private HashSet<ProductResponse> _selectedItems = new();
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    private IAuthorizationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient productclient { get; set; } = default!;
    [Inject]
    private ISnackbar Snackbar { get; set; }
    private ProductResponse _currentDto = new();
    private string searchString = "";
    private bool _loading;
    private string successMessage = "";

private IEnumerable<ProductResponse>? _entityList;
    private int _totalItems;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canExport;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthState;
        _canSearch = await AuthService.HasPermissionAsync(state.User, FshActions.Search, FshResources.Products);
        _canCreate = await AuthService.HasPermissionAsync(state.User, FshActions.Create, FshResources.Products);
        _canUpdate = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.Products);
        _canDelete = await AuthService.HasPermissionAsync(state.User, FshActions.Delete, FshResources.Products);
        _canExport = await AuthService.HasPermissionAsync(state.User, FshActions.Export, FshResources.Products);

        //await LoadBrandsAsync();
    }
    private async Task<GridData<ProductResponse>> ServerReload(GridState<ProductResponse> state)
    {
        try
        {
            _loading = true;
            var productFilter = new SearchProductsCommand
            {
                PageSize = state.PageSize,
                PageNumber = state.Page + 1,
                AdvancedSearch = new()
                {
                    Fields = new[] { "name" },
                    Keyword = searchString
                }
            };

            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => productclient.SearchProductsEndpointAsync("1", productFilter), Toast, Navigation)
                is { } result)
            {
                _totalItems = result.TotalCount;
                _entityList = result.Items;
            }

            _loading = false;
            return new GridData<ProductResponse> { TotalItems = _totalItems, Items = _entityList };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new GridData<ProductResponse>
            {
                Items = new List<ProductResponse>(),
                TotalItems = 0
            };
        }
        finally
        {
            _loading = false;
        }
    }
    private async Task ShowEditFormDialog(string title, UpdateProductCommand command, bool IsCreate)
    {
        var parameters = new DialogParameters
        {
            { nameof(ProductFormDialog.Model), command },
            { nameof(ProductFormDialog.IsCreate), IsCreate }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ProductFormDialog>(title,parameters ,options);
        var state = await dialog.Result;

        if (!state.Canceled)
        {
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }

    private async Task OnCreate()
    {
        var model = new UpdateProductCommand();

        await ShowEditFormDialog("Create new Item", model, true);
    }
    private async Task OnClone()
    {
        var copy = _selectedItems.First();
        if (copy != null)
        {
            var command = new Mapper().Map<ProductResponse, UpdateProductCommand>(copy);
            command.Id = Guid.NewGuid(); // Assign a new Id for the cloned item
            await ShowEditFormDialog("Clone an Item", command, true);
        }
    }
    private async Task OnEdit(ProductResponse dto)
    {
        var command = dto.Adapt<UpdateProductCommand>();
        await ShowEditFormDialog("Edit the Item", command, false);
    }
    private async Task OnDelete(ProductResponse dto)
    {
        var productId = dto.Id;
        _ = productId ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Product", dto.Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled && productId.HasValue)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => productclient.DeleteProductEndpointAsync("1", productId.Value),
                Snackbar);

            await _table.ReloadServerData();
        }
    }
    private async Task OnDeleteChecked()
    {
        var productId = _selectedItems.First().Id;
        _ = productId ?? throw new InvalidOperationException("IdFunc can't be null!");

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, "Product", _selectedItems.First().Id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled && productId.HasValue)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => productclient.DeleteProductEndpointAsync("1", productId.Value),
                Snackbar);

            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }
    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
        _selectedItems = new HashSet<ProductResponse>();
    }

    private Task OnSearch(string text)
    {
        searchString = text;
        return _table.ReloadServerData();
    }

    private async Task LoadBrandsAsync()
    {
        if (_brands.Count == 0)
        {
            var response = await productclient.SearchBrandsEndpointAsync("1", new SearchBrandsCommand());
            if (response?.Items != null)
            {
                _brands = response.Items.ToList();
            }
        }
    }

    // Advanced Search

    private Guid? _searchBrandId;
    private Guid? SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadServerData();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadServerData();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
            _ = _table.ReloadServerData();
        }
    }
}
