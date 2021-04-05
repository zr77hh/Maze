using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    Vector2 grid_w_size;
    public LayerMask wall_layar;
    public float node_radius;

    node[,] _grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    maze_gen gen;
    private void Start()
    {
        gen = GetComponent<maze_gen>();

        grid_w_size.x = gen.maze_width * gen.wall_size;
        grid_w_size.y = gen.maze_hight * gen.wall_size;
        nodeDiameter = node_radius * 2;
        gridSizeX = Mathf.RoundToInt(grid_w_size.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(grid_w_size.y / nodeDiameter);
        createTheGrid();

    }

    void createTheGrid()
    {
        _grid = new node[gridSizeX, gridSizeY];

        Vector2 bottum_left = (Vector2)transform.position - Vector2.right * grid_w_size.x / 2 - Vector2.up * grid_w_size.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 world_pos = bottum_left + Vector2.right * (x * nodeDiameter + node_radius) + Vector2.up * (y * nodeDiameter + node_radius);
                bool walkable =( Physics2D.OverlapCircle(world_pos, node_radius, wall_layar) == null);
                _grid[x, y] = new node(walkable, world_pos,x,y);
            }

        }
    }

    public List<node> get_neighbours(node nod)
    {
        List<node> neighbours = new List<node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int checkx = nod.grid_x+x;
                int checky = nod.grid_y + y;
                if(checkx >=0&& checkx<gridSizeX&&checky >=0 && checky < gridSizeY)
                {
                    neighbours.Add(_grid[checkx, checky]);
                }
            }
        }
        return neighbours;
    }

    public node get_node_by_position(Vector2 pos)
    {
        float percentX = (pos.x + grid_w_size.x / 2) / grid_w_size.x;
        float percentY = (pos.y + grid_w_size.y / 2) / grid_w_size.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return _grid[x, y];
    }

   
    public List<node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(grid_w_size.x, grid_w_size.y));
        if (_grid != null )
        {
            foreach (node n in _grid)
            {
                Gizmos.color = Color.red;
                if (n.walkable)
                    Gizmos.color = Color.white;
                if(path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.green;
                    }
                }
                Gizmos.DrawCube(n.world_pos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
    


}
