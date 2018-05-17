using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : IComparable <Loot>
{
	public int loot_weight;
	public int loot_value;   
    public float loot_worth;

	public Loot()
	{
		this.loot_weight = 0;
		this.loot_value = 0;   
		this.loot_worth = 0;
	}

    public Loot(int _item_value, int _weight)
    {
 		this.loot_weight = _weight;
	    this.loot_value = _item_value;   
		if(_weight != 0) 
		{
			this.loot_worth =  (float)_item_value / (float)_weight;
		} 
		else 
		{
			this.loot_worth = 0;
		}
    }
	
    public int CompareTo(Loot l)
    {
        if (this.loot_worth < l.loot_worth)
		{
            return -1;
		}
		if (this.loot_worth > l.loot_worth)
		{
            return 1;
		}
		if (this.loot_value < l.loot_value) 
		{
			return -1;
		}
		if (this.loot_value > l.loot_value) 
		{
			return 1;
		}
		return 0;
    }
}
