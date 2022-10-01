using Gtk;
using Cairo;
using System;

namespace Game2048;
public class BlocksView : DrawingArea
{
    private BlocksManager grid;

    public BlocksView(BlocksManager grid)
    {
        this.grid = grid;
    }

    public void DrawGrid(Context context)
    {
        for (int i = 0; i < GameParameters.RowColumnCount; i++)
            for (int j = 0; j < GameParameters.RowColumnCount; j++)
                DrawBlock(context, grid.BlocksMatrix[j][i].Value, new Cairo.Point(i, j));
    }

    public void DrawBlock(Context context, int value, Cairo.Point position)
    {
        DrawSquare(context, value, position, DrawType.Shadow);
        DrawSquare(context, value, position, DrawType.Background);
        DrawSquare(context, value, position, DrawType.Main);
        if (value != 1)
            DrawText(context, value, position);
        QueueDraw();     
    }

    public Cairo.Point IndexPositionToRealPosition(Cairo.Point indexPosition)
    {
        int offset = GameParameters.BlockOffset;
        int size = GameParameters.BlockSize;
        int x = offset * (indexPosition.X + 1) + indexPosition.X * size;
        int y = offset * (indexPosition.Y + 1) + indexPosition.Y * size;
        return new Cairo.Point(x, y);
    }

    public Cairo.Point RealPositionToIndexPosition(Cairo.Point realPosition)
    {
        int offset = GameParameters.BlockOffset;
        int size = GameParameters.BlockSize;
        int x = (int)Math.Round((double)(realPosition.X - offset) / (size + offset));
        int y = (int)Math.Round((double)(realPosition.Y - offset) / (size + offset));
        return new Cairo.Point(x, y);
    }

    private void DrawSquare(Context cx, int value, Cairo.Point position, DrawType type)
    {
        cx.LineWidth = 3;
        if (value == 1 && type == DrawType.Main)
            cx.SetSourceRGB(0.85, 0.85, 0.85);
        else if (type == DrawType.Main)
            cx.SetSourceRGBA(1, 1f - 0.7f * Math.Clamp((Math.Log2(value) + 4) / 16f, 0f, 1f),
                0, Math.Clamp((Math.Log2(value) + 5) / 16f, 0f, 1f));
        else if (type == DrawType.Shadow)
            cx.SetSourceRGB(0.65, 0.65, 0.65);
        else
            cx.SetSourceRGB(1, 1, 1);

        Cairo.Point positionReal = IndexPositionToRealPosition(position);
        int a = positionReal.X;
        int b = GameParameters.BlockSize + a;
        int c = positionReal.Y;
        int d = (int)Math.Round((type == DrawType.Shadow 
                                      ? GameParameters.BlockSize 
                                      : GameParameters.BlockSize * 0.94) + c);
        int radius = GameParameters.BlockCornerRadius;
        double PiHalf = Math.PI / 2;

        cx.Arc(a + radius, c + radius, radius, 2 * PiHalf, 3 * PiHalf);
        cx.Arc(b - radius, c + radius, radius, 3 * PiHalf, 4 * PiHalf);
        cx.Arc(b - radius, d - radius, radius, 0 * PiHalf, 1 * PiHalf);
        cx.Arc(a + radius, d - radius, radius, 1 * PiHalf, 2 * PiHalf);
        cx.ClosePath();
        cx.Fill();
    }

    private void DrawText(Context cx, int value, Cairo.Point position)
    {
        if (value < 32)
            cx.SetSourceRGB(0.2, 0.2, 0.2);
        else
            cx.SetSourceRGB(0.95, 0.95, 0.95);

        Pango.FontDescription font = new Pango.FontDescription();
        font.Family = "Consolas";
        font.Weight = Pango.Weight.Ultraheavy;
        font.Stretch = Pango.Stretch.UltraExpanded;
        font.Size = Convert.ToInt32(GameParameters.BlockFontSize * Pango.Scale.PangoScale);
        Pango.Layout layout = CreatePangoLayout(value.ToString());

        layout.FontDescription = font;
        layout.GetPixelSize(out int textWidth, out int textHeight);

        int blockSize = GameParameters.BlockSize;
        Cairo.Point positionReal = IndexPositionToRealPosition(position);
        cx.MoveTo((blockSize - textWidth) / 2 + positionReal.X,
                  (blockSize * 0.96 - textHeight) / 2 + positionReal.Y);
        Pango.CairoHelper.ShowLayout(cx, layout);
        cx.Fill();
    }

    protected override bool OnDrawn(Context cx)
    {
        DrawGrid(cx);
        return true;
    }

    public enum DrawType
    {
        Background,
        Shadow,
        Main
    }
}
