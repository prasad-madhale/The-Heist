using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weight : MonoBehaviour
{

    Text weighttext;

    public Image bagBar;

    public void Awake()
    {
        weighttext = GetComponent<Text>();

    }
    public void Update()
    {
        weighttext.text = "Weight: " + Player.instance.get_player_gold_weight() + "/" + Player.instance.CAPACITY;
        bagBar.fillAmount = Player.instance.get_player_gold_weight() / (float)Player.instance.CAPACITY;
    }
}
