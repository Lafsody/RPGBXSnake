using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSystem {

    public class Point
    {
        public int x, y;
        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    private int width;
    private int height;
    private GridObject[,] grids;
    private List<Point> freeSpaces;

    public GridSystem(int _width, int _height)
    {
        width = _width;
        height = _height;
        grids = new GridObject[width, height];
        freeSpaces = new List<Point>();

        ClearGrids();
    }

    public void ClearGrids()
    {
        freeSpaces.Clear();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grids[i, j] = null;
                freeSpaces.Add(new Point(i, j));
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

    public void AddObject(int x, int y, GridObject gridObject)
    {
        if (HasObjectOnGrid(x, y))
        {
            Debug.Log("Already Have Object in grid " + x + ", " + y);
            return;
        }
        grids[x, y] = gridObject;
        for(int i = 0; i < freeSpaces.Count; i++)
        {
            if(x == freeSpaces[i].x && y == freeSpaces[i].y)
            {
                freeSpaces.Remove(freeSpaces[i]);
                break;
            }
        }
    }

    public void RemoveObject(int x, int y)
    {
        if (!HasObjectOnGrid(x, y))
        {
            Debug.Log("Remove: No Object on Grid " + x + ", " + y);
            return;
        }
        grids[x, y] = null;
        freeSpaces.Add(new Point(x, y));
    }

    public Point GetFreeSpace()
    {
        if(freeSpaces.Count == 0)
        {
            Debug.Log("No free space left!");
            return null;
        }
        int r = Random.Range(0, freeSpaces.Count - 1);
        return freeSpaces[r];
    }
}
