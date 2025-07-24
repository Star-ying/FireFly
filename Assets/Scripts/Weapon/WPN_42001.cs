using UnityEngine;

public class WPN_42001 : MonoBehaviour
{
    Equipment e = new Equipment("AK-47", 0, 40, 80, 0);
    private void OnEnable()
    {
        e.Descrition("攻击附带#1#的真实伤害，#2#%概率触发暴击(爆伤固定50%)@20@20");
        Player.Instance.MakeProperty(e.HP, e.MP, e.ATK, e.DFS);
        Player.Instance.AddProperty1("AttachedHit", 20);
        Player.Instance.AddProperty1("CriticalHit", 20);
        Player.Instance.AddProperty1("CriticalInjury", 50);
    }
}