using FSH.Starter.Blazor.Infrastructure.Api;
using FSH.Starter.Blazor.Infrastructure.Auth;
using FSH.Starter.Shared.Authorization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    private ProductResponse _currentDto = new();
    private string searchString = "";
    private bool _loading;

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

        await LoadBrandsAsync();
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
            productFilter.MinimumRate = Convert.ToDouble(SearchMinimumRate);
            productFilter.MaximumRate = Convert.ToDouble(SearchMaximumRate);
            productFilter.BrandId = SearchBrandId;
            var result = await productclient.SearchProductsEndpointAsync("1", productFilter);

            return new GridData<ProductResponse>
            {
                Items = result.Items ?? new List<ProductResponse>(),
                TotalItems = result.TotalCount
            };
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

        await ShowEditFormDialog("CreateAnItem", model, true);
    }
    private async Task OnClone()
    {
    //    var copy = _selectedItems.First();
    //    var model = Mapper.Map<ProductResponse, CreateProductCommand>(copy, opts =>
    //    {
    //        opts.AfterMap((src, dest) =>
    //        {
    //            dest.Id = 0;
    //        });
    //    });
    //    await ShowEditFormDialog("CreateAnItem", model);
    }
    private async Task OnEdit(ProductResponse dto)
    {
        var command = dto.Adapt<UpdateProductCommand>();
        await ShowEditFormDialog("EditTheItem", command, false);
    }
    private async Task OnDelete(ProductResponse dto)
    {
        if (dto.Id.HasValue)
        {
            await productclient.DeleteProductEndpointAsync("1", dto.Id.Value);
            await _table.ReloadServerData();
            _selectedItems.Clear();
        }
    }
    private async Task OnRefresh()
    {
        await _table.ReloadServerData();
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
