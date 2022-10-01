using Gtk;
using System;
using System.Linq;

namespace Game2048;
public class BlocksController
{
    public BlocksView View;
    public BlocksManager Blocks;
    public GameOver GameOverObject;

    public BlocksController()
    {
        GameOverObject = new GameOver();
        Blocks = new BlocksManager();
        View = new BlocksView(Blocks);
        Spawn();
        Spawn();
    }

    public void Move(object s, KeyPressEventArgs e)
    {
        switch (e.Event.Key)
        {
            case Gdk.Key.w or Gdk.Key.uparrow:
                Move(Direction.Up);
                break;
            case Gdk.Key.s or Gdk.Key.downarrow:
                Move(Direction.Down);
                break;
            case Gdk.Key.a or Gdk.Key.leftarrow:
                Move(Direction.Left);
                break;
            case Gdk.Key.d or Gdk.Key.rightarrow:
                Move(Direction.Right);
                break;
        }
    }

    public void Move(Direction direction)
    {
        if (GameOverObject.IsGameOver)
            GameOverObject.GameOverWindow();

        TryMove(direction, out bool merged);
        
        if (merged)
            Spawn();

        if (GameOverObject.CheckGameOver(Blocks))
            GameOverObject.IsGameOver = true;
    }

    private void Spawn()
    {
        if (!Blocks.BlocksList.Any(p => p.Value == 1))
            return;

        Random rnd = new Random();
        Block emptyBlock = Blocks.BlocksList.Where(p => p.Value == 1).OrderBy(x => rnd.Next()).First();
        var coords = Blocks.BlocksMatrix.CoordinatesOf(emptyBlock);
        Blocks.BlocksMatrix[coords.X][coords.Y].Value = rnd.Next(101) < 30 ? 4 : 2;
    }

    private void TryMove(Direction direction, out bool result)
    {
        result = false;
        if (direction == Direction.Up || direction == Direction.Down)
        {
            if (MergeEmptyUpDown(direction))
                result = true;
            if (MergeUpDown(direction))
                result = true;
            if (result)
                MergeEmptyUpDown(direction);
        }
        else
        {
            if (MergeEmptyLeftRight(direction))
                result = true;
            if (MergeLeftRight(direction))
                result = true;
            if (result)
                MergeEmptyLeftRight(direction);
        }
    }

    private bool MergeUpDown(Direction direction)
    {
        bool merged = false;
        for (int i = direction == Direction.Up ? 0 : GameParameters.RowColumnCount - 1;
            direction == Direction.Up ? i < GameParameters.RowColumnCount - 1 : i > 0;
            i = direction == Direction.Up ? i + 1 : i - 1)
        {
            for (int j = 0; j < GameParameters.RowColumnCount; j++)
                if (TryMerge(new Cairo.Point(direction == Direction.Up ? i + 1 : i - 1, j), 
                                                                       new Cairo.Point(i, j)))
                    merged = true;
        }
        return merged;
    }

    private bool MergeLeftRight(Direction direction)
    {
        bool merged = false;
        for (int i = 0; i < GameParameters.RowColumnCount; i++)
        {
            for (int j = direction == Direction.Left ? 0 : GameParameters.RowColumnCount - 1;
                direction == Direction.Left ? j < GameParameters.RowColumnCount - 1 : j > 0;
                j = direction == Direction.Left ? j + 1 : j - 1)
                if (TryMerge(new Cairo.Point(i, direction == Direction.Left ? j + 1 : j - 1), 
                                                                            new Cairo.Point(i, j)))
                    merged = true;
        }
        return merged;
    }

    private bool MergeEmptyUpDown(Direction direction)
    {
        bool merged = false;
        int loop = 0;
        bool wasMerged;
        do
        {
            wasMerged = false;
            for (int i = direction == Direction.Up ? 0 : GameParameters.RowColumnCount - 1;
            direction == Direction.Up ? i < GameParameters.RowColumnCount - 1 : i > 0;
            i = direction == Direction.Up ? i + 1 : i - 1)
                for (int j = 0; j < GameParameters.RowColumnCount; j++)
                    if (TryMergeEmpty(new Cairo.Point(direction == Direction.Up ? i + 1 : i - 1, j), 
                                                                                new Cairo.Point(i, j)))
                        wasMerged = true;

            if (loop++ == 0 && wasMerged)
                merged = true;

        } while (wasMerged);

        return merged;
    }

    private bool MergeEmptyLeftRight(Direction direction)
    {
        bool merged = false;
        int loop = 0;
        bool wasMerged;
        do
        {
            wasMerged = false;
            for (int i = 0; i < GameParameters.RowColumnCount; i++)
                for (int j = direction == Direction.Left ? 0 : GameParameters.RowColumnCount - 1;
                    direction == Direction.Left ? j < GameParameters.RowColumnCount - 1 : j > 0;
                    j = direction == Direction.Left ? j + 1 : j - 1)
                    if (TryMergeEmpty(new Cairo.Point(i, direction == Direction.Left ? j + 1 : j - 1), 
                                                                                     new Cairo.Point(i, j)))
                        wasMerged = true;

            if (loop++ == 0 && wasMerged)
                merged = true;

        } while (wasMerged);

        return merged;
    }


    private bool TryMerge(Cairo.Point source, Cairo.Point target)
    {
        if (Blocks.BlocksMatrix[source.X][source.Y].Value == 1 || Blocks.BlocksMatrix[target.X][target.Y].Value == 1)
            return false;

        if (Blocks.BlocksMatrix[source.X][source.Y].Value != Blocks.BlocksMatrix[target.X][target.Y].Value)
            return false;

        Blocks.BlocksMatrix[target.X][target.Y].Value = Blocks.BlocksMatrix[target.X][target.Y].GetNextValue();
        Blocks.BlocksMatrix[source.X][source.Y].Value = 1;
        return true;
    }

    private bool TryMergeEmpty(Cairo.Point source, Cairo.Point empty)
    {
        if (Blocks.BlocksMatrix[empty.X][empty.Y].Value != 1 || Blocks.BlocksMatrix[source.X][source.Y].Value == 1)
            return false;

        Blocks.BlocksMatrix = Blocks.BlocksMatrix.Swap(source, empty);
        return true;
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
