using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLootVal : MonoBehaviour {

    public Text lootText;
	
	// Update is called once per frame
	public void Show_Gold_Value () {
        lootText.text = "Weight: "+GetComponent<Treasure>().gold_weight +"\nValue: "+ GetComponent<Treasure>().gold_value;
	}
}
