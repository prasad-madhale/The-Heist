using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class move : MonoBehaviour {

    public Transform player;
    private NavMeshAgent agent;
    Astar astar;
    public static move instance;

    private void Awake()
    {
	    agent = player.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        instance = this;
    }

    public void autoMove(List<Node> mover)
    {
        if (mover!= null && mover.Count >= 2)
        {
            agent.destination = mover[1].position;
            mover.RemoveAt(0);
        }
    }
}
