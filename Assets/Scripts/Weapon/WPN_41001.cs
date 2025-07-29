using System;
using UnityEngine;

public class WPN_41001 : MonoBehaviour
{
    Equipment e = new Equipment("护摩之杖", 120, 0, 60, 20);
    public int health;
    private void OnEnable()
    {
        e.Descrition("生命增加#1#%，攻击力增加当前最大生命#2#%的值@50@5");
        Player.Instance.MakeProperty(e.HP, e.MP, e.ATK, e.DFS);
        Player.Instance.AddProperty("health",0.5f);
        health = Player.Instance.Property["health"];
        Player.Instance.Property["attack"] += (int)(health * 0.05);
    }
    private void Update()
    {
        if(Player.Instance.Property["health"] != health)
        {
            health = Player.Instance.Property["health"];
            Player.Instance.Property["attack"] += (int)(Player.Instance.Property["health"] * 0.05);
        }
    }
}
