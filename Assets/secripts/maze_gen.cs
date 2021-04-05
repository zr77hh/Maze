using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze_gen : MonoBehaviour
{
    [SerializeField] GameObject wall_prifap;
    public Transform ref_pos;
    [HideInInspector] public float wall_size;
    public int maze_width, maze_hight;
    maze_room[,] rooms;
    bool maze_is_completed = false;
    int width_k = 0;
    int hight_k = 0;
    private void Awake()
    {
        transform.position =Vector3.zero;

        ref_pos.position = new Vector2(-maze_width + 1, -maze_hight + 1);

        make_grid();
        while (!maze_is_completed)
        {
            kill();
            hunt();
        }
        make_entrance_exit();
    }

    void make_entrance_exit()
    {
        Destroy(rooms[0, 0].bottum_wall);
        Destroy(rooms[maze_width - 1, maze_hight-1].top_wall);
    }
    void make_grid()
    {
        wall_prifap.transform.localScale = new Vector3(wall_prifap.transform.localScale.x,wall_size,wall_prifap.transform.localScale.z);
        rooms = new maze_room[maze_width, maze_hight];
        for (int w = 0; w < maze_width; w++)
        {
            for (int h = 0; h < maze_hight; h++)
            {
                rooms[w, h] = new maze_room();
                if (w == 0)
                {
                    rooms[w, h].left_wall = Instantiate(wall_prifap, new Vector2((w*wall_size) - wall_size/2, h*wall_size)+ (Vector2)ref_pos.position, Quaternion.identity);
                    rooms[w, h].left_wall.transform.parent = transform;

                }
                else
                {
                    rooms[w, h].left_wall = rooms[w-1, h].right_wall;
                }
                rooms[w, h].right_wall = Instantiate(wall_prifap, new Vector2((w*wall_size) + wall_size/2, h*wall_size) + (Vector2)ref_pos.position, Quaternion.identity);
                rooms[w, h].right_wall.transform.parent = transform;
                if (h == 0)
                {
                    rooms[w, h].bottum_wall = Instantiate(wall_prifap, new Vector2(w*wall_size, (h*wall_size) - wall_size/2) + (Vector2)ref_pos.position, Quaternion.Euler(0, 0, 90f));
                    rooms[w, h].bottum_wall.transform.parent = transform;

                }
                else
                {
                    rooms[w, h].bottum_wall = rooms[w, h-1].top_wall;
                }
                rooms[w, h].top_wall = Instantiate(wall_prifap, new Vector2((w*wall_size), (h*wall_size) + wall_size/2) + (Vector2)ref_pos.position, Quaternion.Euler(0, 0, 90f));
                rooms[w, h].top_wall.transform.parent = transform;


            }
        }
    }
    void kill()
    {
        
        bool  still_rooms_exists = true;
        while (still_rooms_exists)
        {
            
            rooms[width_k, hight_k].is_visited = true;
            int direction = get_random_direction(width_k, hight_k);
            if( direction == 0)
            {
                still_rooms_exists = false;
                return;
            }
            // if diyrection = 1 then go left
            else if (direction == 1)
            {
                if (thes_room_exists(width_k - 1, hight_k)&& !rooms[width_k - 1, hight_k].is_visited)
                {
                    width_k -= 1;
                    Destroy(rooms[width_k, hight_k].right_wall);
                }

            }
            else if (direction == 2)
            {
                if (thes_room_exists(width_k + 1, hight_k) && !rooms[width_k + 1, hight_k].is_visited)
                {
                    Destroy(rooms[width_k, hight_k].right_wall);
                    width_k += 1;
                }
            }
            else if(direction == 3)
            {
                if (thes_room_exists(width_k, hight_k - 1) && !rooms[width_k, hight_k-1].is_visited)
                {
                    hight_k -= 1;
                    Destroy(rooms[width_k, hight_k].top_wall);
                }
            }
            else if(direction == 4)
            {
                if (thes_room_exists(width_k, hight_k + 1) && !rooms[width_k , hight_k+1].is_visited)
                {
                    Destroy(rooms[width_k, hight_k].top_wall);
                    hight_k += 1;
                }
            }
            

        }
    }
    int get_random_direction(int current_rom_w,int current_room_h)
    {
        List<int> avelabol_diyrections = new List<int>();
        if (thes_room_exists(current_rom_w-1,current_room_h) && !rooms[current_rom_w - 1, current_room_h].is_visited)
        {
            avelabol_diyrections.Add(1);

        }
        if (thes_room_exists(current_rom_w + 1, current_room_h) && !rooms[current_rom_w + 1, current_room_h].is_visited)
        {
            avelabol_diyrections.Add(2);
        }
        if (thes_room_exists(current_rom_w, current_room_h - 1) && !rooms[current_rom_w , current_room_h-1].is_visited)
        {
            avelabol_diyrections.Add(3);
        }if(thes_room_exists(current_rom_w, current_room_h + 1) && !rooms[current_rom_w , current_room_h+1].is_visited)
        {
            avelabol_diyrections.Add(4);
        }


        if (avelabol_diyrections.Count != 0)
        {
            return avelabol_diyrections[Random.Range(0, avelabol_diyrections.Count)];
        }

        return 0;

    }
    bool thes_room_exists(int w, int h)
    {
        if (w >= 0 && w < maze_width && h >= 0 && h < maze_hight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void hunt()
    {
        maze_is_completed = true;
        for (int w = 0; w < maze_width; w++)
        {
            for (int h = 0; h < maze_hight; h++)
            {
                if (!rooms[w, h].is_visited&&is_have_visited_neighbors(w,h))
                {
                    maze_is_completed = false;
                    width_k = w;
                    hight_k = h;
                    distroy_random_wall(w, h); 
                    return;
                }
            }
        }
    }
    bool is_have_visited_neighbors(int with,int higt)
    {
        if (thes_room_exists(with - 1, higt)&&rooms[with-1,higt].is_visited)
        {
            return true;
        }else if(thes_room_exists(with + 1, higt) && rooms[with + 1, higt].is_visited)
        {
            return true;
        }
        else if (thes_room_exists(with , higt-1) && rooms[with, higt-1].is_visited)
        {
            return true;
        }
        else if (thes_room_exists(with , higt+1) && rooms[with, higt+1].is_visited)
        {
            return true;
        }else
        {
            return false;

        }
    }
    void distroy_random_wall(int w, int h)
    {
        int diyrection = random_direction_for_walls(w, h);
        if(diyrection == 0)
        {
            return;
        }
        if (diyrection == 1 )
        {
            Destroy(rooms[w - 1, h].right_wall);
            return;
        }
        else if (diyrection == 2)
        {
            Destroy(rooms[w, h].right_wall);
            return;
        }
        else if (diyrection == 3 )
        {
            Destroy(rooms[w, h - 1].top_wall);
            return;
        }
        else if (diyrection == 4 )
        {
            Destroy(rooms[w, h].top_wall);
            return;
        }

    }

    int random_direction_for_walls(int current_rom_w, int current_room_h)
    {
        List<int> available_directions = new List<int>();
        if (thes_room_exists(current_rom_w - 1, current_room_h) && rooms[current_rom_w - 1, current_room_h].is_visited)
        {
            available_directions.Add(1);

        }
        if (thes_room_exists(current_rom_w + 1, current_room_h) && rooms[current_rom_w + 1, current_room_h].is_visited)
        {
            available_directions.Add(2);
        }
        if (thes_room_exists(current_rom_w, current_room_h - 1) && rooms[current_rom_w, current_room_h - 1].is_visited)
        {
            available_directions.Add(3);
        }
        if (thes_room_exists(current_rom_w, current_room_h + 1) && rooms[current_rom_w, current_room_h + 1].is_visited)
        {
            available_directions.Add(4);
        }


        if (available_directions.Count != 0)
        {
            return available_directions[Random.Range(0, available_directions.Count)];
        }

        return 0;
    }
}
