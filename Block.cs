using System;

namespace Game2048;
public class Block
{
    public int Value;

    public Block()
    {
        Random rnd = new Random();
    }

    public int GetNextValue() => 2 * Value;
}
