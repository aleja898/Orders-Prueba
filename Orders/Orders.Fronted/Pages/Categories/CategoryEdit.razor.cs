using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Fronted.Shared;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Fronted.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public partial class CategoryEdit
    {
        private Category? category;
        private FormWithName<Category>? categoryForm;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var responseHttp = await Repository.GetAsync<Category>($"api/categories/{Id}");

            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }
            }
            else
            {
                category = responseHttp.Response;
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"api/categories", category);

            if (responseHttp.Error)
            {
                var mensaje = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", mensaje);
                return;
            }

            Return();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = false,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con éxito.");
        }

        private void Return()
        {
            categoryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/categories");
        }
    }
}