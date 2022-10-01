using System.Collections.Generic;
using System.Linq;

namespace Game2048;

public class BlocksManager
{

    public List<List<Block>> BlocksMatrix;
    public List<Block> BlocksList => BlocksMatrix.SelectMany(p => p).ToList();

    public BlocksManager()
    {
        BlocksMatrix = new List<List<Block>>();
        for (int i = 0; i < GameParameters.RowColumnCount; i++)
        {
            BlocksMatrix.Add(new List<Block>());
            for (int j = 0; j < GameParameters.RowColumnCount; j++)
                BlocksMatrix.Last().Add(new Block());
        }

        BlocksMatrix.ForEach2D(p => p.Value = 1);
    }
}