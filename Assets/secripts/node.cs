using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    public bool walkable;
    public int g_cost;
    public int h_cost;

    public Vector2 world_pos;

    public int grid_x;
    public int grid_y;

    public node parent;

    public node(bool Walkable,Vector2 WorldsPos,int _grid_x,int _grid_y)
    {
        walkable = Walkable;
        world_pos = WorldsPos;
        grid_x = _grid_x;
        grid_y = _grid_y;
    }

    public int f_cost
    {
        get
        {
            return g_cost + h_cost;
        }
    }

}
