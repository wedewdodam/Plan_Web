using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Plan_Blazor_Lib.Note_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plan_Web.Pages
{
    public partial class Index
    {
        [Inject] public NavigationManager MyNav { get; set; } // Url
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] public IKhma_Note_Lib khma_Note_Lib { get; set; }

        private List<Khma_Note_Entity> annA { get; set; } = new List<Khma_Note_Entity>();
        private List<Khma_Note_Entity> annB { get; set; } = new List<Khma_Note_Entity>();
        private List<Khma_Note_Entity> annC { get; set; } = new List<Khma_Note_Entity>();

        private void OnUrlA()
        {
            MyNav.NavigateTo("/");
        }

        protected override async Task OnInitializedAsync()
        {
            annA = await khma_Note_Lib.GetList_Main("C4");
            annB = await khma_Note_Lib.GetList_Main("B6");
            annC = await khma_Note_Lib.GetList_Main("C4");
        }
    }
}
