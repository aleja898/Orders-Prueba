using Microsoft.AspNetCore.Components;
using System.Security.Principal;

namespace Orders.Fronted.Shared
{
    public partial class GenericList<Titem>
    {
        [Parameter] public RenderFragment? Loading { get; set; }
        [Parameter] public RenderFragment? NoRecords { get; set; }
        [Parameter, EditorRequired] public RenderFragment? Body { get; set; } = null!;
        [Parameter, EditorRequired] public List<Titem> MyList { get; set; } = null!;
    }
}
