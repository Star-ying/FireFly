using UnityEngine;

public class WPN_42001 : MonoBehaviour
{
    Equipment e = new Equipment("AK-47", 0, 40, 80, 0);
    private void OnEnable()
    {
        e.Descrition("��������#1#����ʵ�˺���#2#%���ʴ�������(���˹̶�50%)@20@20");
        Player.Instance.MakeProperty(e.HP, e.MP, e.ATK, e.DFS);
        Player.Instance.AddProperty1("AttachedHit", 20);
        Player.Instance.AddProperty1("CriticalHit", 20);
        Player.Instance.AddProperty1("CriticalInjury", 50);
    }
}