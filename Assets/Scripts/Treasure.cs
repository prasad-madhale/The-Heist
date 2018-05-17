using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
	public int gold_weight = 5;
	public int gold_value = 10; 
    public float breaking_time = 2;
	public float looting_time = 2;
	public bool secured = false;

    private void Start()
    {
		set_looting_time();
    }

	private void set_looting_time()
    {
		looting_time = (float) gold_weight / (float) 10;
    }

    public void empty_treasure()
	{
		if (looting_time > 0) 
		{
			Player.instance.set_loot_time (looting_time);
		}
		looting_time = 0;
	}
	
	public void open_treasure()
	{
		if (breaking_time > 0) {
			Player.instance.set_break_time (breaking_time);
		}
		breaking_time = 0;
	}

	public Loot get_treasure_loot()
	{
		return new Loot(gold_value, gold_weight);
	}

	public Vector3 get_treasure_position()
	{
		return transform.position;
	}

	public void treasure_reset() {
		breaking_time = 2;
		set_looting_time ();
	}

    public bool has_gold()
    {
        if (looting_time > 0)
            return true;
        return false;
    }
}
