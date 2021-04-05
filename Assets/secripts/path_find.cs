using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class path_find : MonoBehaviour
{
    [SerializeField] grid gri;
    public Transform from, to;

    private void Update()
    {
        findThePath(from.position, to.position);
    }
    void findThePath(Vector2 start_from,Vector2 to)
    {
        node start_node = gri.get_node_by_position(start_from);
        node target_node = gri.get_node_by_position(to);

        List<node> open_set = new List<node>();
        HashSet<node> closed_set = new HashSet<node>();

        open_set.Add(start_node);

        while (open_set.Count > 0)
        {
            node current_node = open_set[0];
            for (int i = 1; i < open_set.Count; i++)
            {
                if (open_set[i].f_cost < current_node.f_cost || open_set[i].f_cost == current_node.f_cost&&open_set[i].h_cost<current_node.h_cost)
                {
                    current_node = open_set[i];
                }
            }
            open_set.Remove(current_node);
            closed_set.Add(current_node);
            if(current_node == target_node)
            {
                retracePath(start_node, target_node);
                return;
            }

            foreach(node neighbour in gri.get_neighbours(current_node))
            {
                if(!neighbour.walkable|| closed_set.Contains(neighbour))
                {
                    continue;
                }
                int movment_cost_to_neighbour = current_node.grid_y + culclete_distance(current_node, neighbour);
                if(movment_cost_to_neighbour< neighbour.g_cost || !open_set.Contains(neighbour))
                {
                    neighbour.g_cost = movment_cost_to_neighbour;
                    neighbour.h_cost = culclete_distance(neighbour, target_node);
                    neighbour.parent = current_node;

                    if (!open_set.Contains(neighbour))
                    {
                        open_set.Add(neighbour);
                    }
                }
            }
            
        }
    }

    void retracePath(node start_node,node end_node)
    {
        List<node> path = new List<node>();
        node current_node = end_node;
        while (current_node != start_node)
        {
            path.Add(current_node);
            current_node = current_node.parent;
        }
        path.Reverse();

        gri.path = path;
    }

    int culclete_distance(node from_node,node to_node)
    {
        int dist_x = Mathf.Abs(from_node.grid_x - to_node.grid_x);
        int dist_y = Mathf.Abs(from_node.grid_y - to_node.grid_y);

        if(dist_x > dist_y)
        {
            return 14 * dist_y + 10 * (dist_x - dist_y);
        }
        return 14 * dist_x + 10 * (dist_y - dist_x);
    }
    

}
