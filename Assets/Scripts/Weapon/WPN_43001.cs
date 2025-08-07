using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPN_43001 : MonoBehaviour
{
    Equipment e = new Equipment("萨满变身器", 0, 40, 30, 40);
    private void OnEnable()
    {
        e.Descrition("萨姆状态下所有能量获取效率增加#1#%@20");
        Player.Instance.MakeProperty(e.HP, e.MP, e.ATK, e.DFS);
        Player.Instance.AddProperty_Num("margic_rate", 20);
    }
}
