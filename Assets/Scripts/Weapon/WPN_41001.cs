using System;
using UnityEngine;

public class WPN_41001 : MonoBehaviour
{
    Equipment e = new Equipment("��Ħ֮��", 120, 0, 60, 20);
    public int health;
    private void OnEnable()
    {
        e.Descrition("��������#1#%�����������ӵ�ǰ�������#2#%��ֵ@50@5");
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
