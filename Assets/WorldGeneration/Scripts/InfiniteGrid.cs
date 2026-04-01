using System;
using System.Collections.Generic;

class InfiniteGrid<T>
{
    private Dictionary<(int x, int y), T> grid = new();

    public void Set(int x, int y, T value)
    {
        grid[(x, y)] = value;
    }

    public T Get(int x, int y)
    {
        if (grid.TryGetValue((x, y), out T value))
            return value;

        return default(T); // or throw exception if you prefer
    }

    public bool Has(int x, int y)
    {
        return grid.ContainsKey((x, y));
    }
}