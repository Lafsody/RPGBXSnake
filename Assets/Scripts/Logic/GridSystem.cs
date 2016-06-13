using UnityEngine;
using System.Collections;

public class GridSystem {

    private GridObject[,] grids;
    private int width;
    private int height;

    public GridSystem(int _width, int _height)
    {
        width = _width;
        height = _height;
        grids = new GridObject[width, height];
    }

    public void ClearGrids()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grids[i, j] = null;
            }
        }
    }

    public bool IsBorder(int x, int y)
    {
        return x < 0 || x >= width || y < 0 || y >= height;
    }

    public bool HasObjectOnGrid(int x, int y)
    {
        if (IsBorder(x, y))
        {
            Debug.Log("Out of bound: grids position at " + x + ", " + y);
            return false;
        }
        return grids[x, y] != null;
    }

    public GridObject GetObjectOnGrid(int x, int y)
    {
        if(!HasObjectOnGrid(x, y))
        {
            Debug.Log("Try to get Null Object at grid " + x + ", " + y);
            return null;
        }
        return grids[x, y];
    }
}
