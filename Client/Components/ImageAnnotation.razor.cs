using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Excubo.Blazor.Canvas;

namespace Obra.Client.Components
{
    public partial class ImageAnnotation : ComponentBase
    {
        private protected IJSRuntime js;
        private string ImageBase64Tenant { get; set; }
        private ElementReference container;
        public Canvas _context;
        public Canvas _context2;
        private Excubo.Blazor.Canvas.Contexts.Context2D ctx1;
        private Excubo.Blazor.Canvas.Contexts.Context2D ctx2;
        private double canvasx;
        private double canvasy;
        private double last_mousex;
        private double last_mousey;
        private double mousex;
        private double mousey;
        private bool mousedown = false;
        public string clr = "red";

        [Parameter]
        public int IdBlob { get; set; }
        [Parameter]
        public EventCallback Delete { get; set; }
        [Parameter]
        public EventCallback Save { get; set; }

        public ImageAnnotation(IJSRuntime js)
        {
            this.js = js;
        }

        private class Position
        {
            public double Left { get; set; }
            public double Top { get; set; }
        }
        private async Task ToggleColorAsync()
        {
            clr = clr == "black" ? "red" : "black";
            await ctx1.StrokeStyleAsync(clr);
        }
        private async Task ClearAsync()
        {
            await ctx1.ClearRectAsync(0, 0, 600, 450);
        }
        public async Task ImageAsync()
        {
            ImageBase64Tenant = await _context.ToDataURLAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await js.InvokeVoidAsync("eval", $"myimage = document.getElementById('{IdBlob}')");

                ctx2 = await _context2.GetContext2DAsync();
                await ctx2.DrawImageAsync("myimage", 0, 0, 600, 450);

                ctx1 = await _context.GetContext2DAsync();
                // initialize settings
                await ctx1.GlobalCompositeOperationAsync(CompositeOperation.Source_Over);
                await ctx1.StrokeStyleAsync(clr);
                await ctx1.LineWidthAsync(3);
                await ctx1.LineJoinAsync(LineJoin.Round);
                await ctx1.LineCapAsync(LineCap.Round);
                // this retrieves the top left corner of the canvas container (which is equivalent to the top left corner of the canvas, as we don't have any margins / padding)
                var p = await js.InvokeAsync<Position>("eval", $"let e = document.querySelector('[_bl_{container.Id}=\"\"]'); e = e.getBoundingClientRect(); e = {{ 'Left': e.x, 'Top': e.y }}; e");
                (canvasx, canvasy) = (p.Left, p.Top);
            }
        }

        private void MouseDownCanvas(MouseEventArgs e)
        {
            render_required = false;
            this.last_mousex = mousex = e.ClientX - canvasx;
            this.last_mousey = mousey = e.ClientY - canvasy;
            this.mousedown = true;
        }

        private void MouseUpCanvas(MouseEventArgs e)
        {
            render_required = false;
            mousedown = false;
        }

        async Task MouseMoveCanvasAsync(MouseEventArgs e)
        {
            render_required = false;
            if (!mousedown)
            {
                return;
            }
            mousex = e.ClientX - canvasx;
            mousey = e.ClientY - canvasy;
            await DrawCanvasAsync(mousex, mousey, last_mousex, last_mousey, clr);
            last_mousex = mousex;
            last_mousey = mousey;
        }
        private void MouseDownCanvas(TouchEventArgs e)
        {
            render_required = false;
            this.last_mousex = mousex = e.Touches[0].ClientX - canvasx;
            this.last_mousey = mousey = e.Touches[0].ClientY - canvasy;
            this.mousedown = true;
        }

        private void MouseUpCanvas(TouchEventArgs e)
        {
            render_required = false;
            mousedown = false;
        }

        async Task MouseMoveCanvasAsync(TouchEventArgs e)
        {
            render_required = false;
            if (!mousedown)
            {
                return;
            }
            mousex = e.Touches[0].ClientX - canvasx;
            mousey = e.Touches[0].ClientY - canvasy;
            await DrawCanvasAsync(mousex, mousey, last_mousex, last_mousey, clr);
            last_mousex = mousex;
            last_mousey = mousey;
        }
        async Task DrawCanvasAsync(double prev_x, double prev_y, double x, double y, string clr)
        {
            await using (var ctx2 = await ctx1.CreateBatchAsync())
            {
                await ctx2.BeginPathAsync();
                await ctx2.MoveToAsync(prev_x, prev_y);
                await ctx2.LineToAsync(x, y);
                await ctx2.StrokeAsync();
            }

        }
        private bool render_required = true;


        protected override bool ShouldRender()
        {
            if (!render_required)
            {
                render_required = true;
                return false;
            }
            return base.ShouldRender();
        }
        private async Task ClickedSave()
        {
            await Save.InvokeAsync();
        }
        private async Task ClickedDelete()
        {
            await Delete.InvokeAsync();
        }
    }
}
