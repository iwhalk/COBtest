using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Obra.Client.Components;

public partial class PreviewFile : ComponentBase
{
    [Parameter]
    public EventCallback CloseModalLessor { get; set; }
    [Parameter]
    public byte[]? Content { get; set; }
    [Parameter]
    public string PdfName { get; set; }
    [Parameter]
    public bool ShowModal { get; set; }
    private string? BlobUrl { get; set; }

    private readonly IJSInProcessRuntime _js;

    public PreviewFile(IJSInProcessRuntime js)
    {
        _js = js;
    }
    protected override void OnInitialized()
    {
        if (Content != null)
        { 
            BlobUrl = _js.Invoke<string>("CreateObjectURL", Content, PdfName , "application/pdf");
        }
    }
    
    // public override async Task SetParametersAsync(ParameterView parameters)
    // {
    //     if (parameters.TryGetValue<byte[]>(nameof(Content), out var value))
    //     {
    //         if (value is not null)
    //         {
    //             StateHasChanged();
    //         }
    //     }
    //     await base.SetParametersAsync(parameters);
    // }

    public void Dispose()
    {
        _js.Invoke<string>("RevokeObjectURL", BlobUrl);
        BlobUrl = null;
    }
}