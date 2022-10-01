using System;
using System.Numerics;

namespace Game2048;
public static class GameParameters
{
    public const int BlockSize = 150;
    public const int BlockCount = 16;
    public const int BlockOffset = 15;
    public static int RowColumnCount => (int)Math.Sqrt(BlockCount);
    public const int BlockFontSize = 30;
    public const int BlockCornerRadius = 20;

    public static Vector2 WindowSize =>
        new Vector2(BlockSize * RowColumnCount + (RowColumnCount + 1) * BlockOffset);
}
