using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public float node_size;
    public Vector2 gridSize;
    public LayerMask obstacles;
    int num_of_grid_x, num_of_grid_y;
    public float node_diameter;
    public Transform plyr;
    public static Grid instance;
    Node[,] nodes;

    void Start()
    {
        instance = this;
        node_diameter = 2 * node_size;
        num_of_grid_x = Mathf.RoundToInt( gridSize.x / node_diameter);
        num_of_grid_y = Mathf.RoundToInt( gridSize.y / node_diameter);
        DrawGrid(); 
    }

    public List<Node> final_path = new List<Node>();

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (nodes != null)
        {
            Node plyrnode = worldToNode(plyr.position);
            foreach (Node nd in nodes)
            {
                if (nd.isobstacle)
                    Gizmos.color = Color.red;
                else if (plyrnode == nd)
                    Gizmos.color = Color.yellow;
                else
                    Gizmos.color = Color.white;

                if (final_path != null)
                {
                    if (final_path.Contains(nd))
                        Gizmos.color = Color.cyan;
                }

                Gizmos.DrawCube(nd.position, Vector3.one * (node_diameter - .1f));
            }
        }
    }

    void DrawGrid()
    {
        nodes = new Node[ num_of_grid_x, num_of_grid_y];

        Vector3 start = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int i = 0; i < num_of_grid_x; i++)
        {
            for (int j = 0; j < num_of_grid_y; j++)
            {
                Vector3 point_pos = start + Vector3.right * (i * node_diameter + node_size) + Vector3.forward * (j * node_diameter + node_size);
                point_pos.y = 0.5f;
                bool is_obstacle = false;

                if (Physics.CheckSphere(point_pos, node_diameter, obstacles))
                    is_obstacle = true;

                nodes[i, j] = new Node(is_obstacle, point_pos, i, j);
            }
        }
    }


    public Node worldToNode(Vector3 worldPosition)
    {
        float xdisplacement = (worldPosition.x + (gridSize.x - 1) / 2) / gridSize.x;
        float ydisplacement = (worldPosition.z + (gridSize.y - 1) / 2) / gridSize.y;

        int x = Mathf.RoundToInt((num_of_grid_x) * xdisplacement);
        int y = Mathf.RoundToInt((num_of_grid_y) * ydisplacement);

        return nodes[x, y];
    }

    public List<Node> getNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int nx = node.x + i;
                int ny = node.y + j;

                if (nx == node.x && ny == node.y)
                    continue;

                if (nx >= 0 && nx < num_of_grid_x && ny >= 0 && ny < num_of_grid_y)
                    neighbors.Add(nodes[nx,ny]);
            }
        }

        return neighbors;
    }

}
