using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class liuying : MonoBehaviour
{
    private void OnEnable()
    {
        Player.Instance.SetProperty(200,100,20,5);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && Player.Instance.Keys.GetValueOrDefault("J") && !Player.Instance.isAttack)
        {
            Player.Instance.Keys["J"] = false;
            StartCoroutine(Player.Instance.EnableAfterDelay(0.5f, "J"));
            Player.Instance.Attack("Fight_Square", Player.Instance.fight_r);
            StartCoroutine(Player.Instance.AttackSleep());
        }
        else if (Input.GetKeyUp(KeyCode.J) && Player.Instance.Keys.GetValueOrDefault("J") && Player.Instance.isAttack)
        {
            Player.Instance.Keys["J"] = false;
            Player.Instance.isAttack = false;
            StartCoroutine(Player.Instance.EnableAfterDelay(0.3f, "J"));
        }
        else if (Input.GetKeyDown(KeyCode.I) && Player.Instance.Keys.GetValueOrDefault("I") && Player.Instance.margic >= 100)
        {
            Player.Instance.margic -= 100;
            foreach (var temp in Player.Instance.Keys.Keys.ToList())
            {
                Player.Instance.Keys[temp] = false;
            }
            Player.Instance.transform.Find("Animitor").transform.Find("Metamorphose").gameObject.transform.position = Player.Instance.transform.position;
            Player.Instance.transform.Find("Animitor").transform.Find("Metamorphose").gameObject.SetActive(true);
            StartCoroutine(Player.Instance.beSaMu());
        }
        else if (Input.GetKeyDown(KeyCode.K) && Player.Instance.Keys.GetValueOrDefault("K"))
        {
            Player.Instance.Keys["K"] = false;
            StartCoroutine(Player.Instance.dodge());
            StartCoroutine(Player.Instance.EnableAfterDelay(0.6f, "K"));
        }
        else if (Input.GetKeyDown(KeyCode.L) && Player.Instance.Keys.GetValueOrDefault("L"))
        {
            Player.Instance.isFire = true;
            StartCoroutine(Player.Instance.SpawnRoutine());
        }
        else if (Input.GetKeyUp(KeyCode.L) && Player.Instance.Keys.GetValueOrDefault("L"))
        {
            Player.Instance.Keys["L"] = false;
            Player.Instance.isFire = false;
            StartCoroutine(Player.Instance.HealRoutine());
            StartCoroutine(Player.Instance.EnableAfterDelay(0.4f, "L"));
        }
    }
}