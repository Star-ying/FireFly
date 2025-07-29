using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class samu : MonoBehaviour
{
    public float L_energy = 0;

    private void OnEnable()
    {
        Player.Instance.SetProperty(300, 100, 30, 20);
        transform.position = Player.Instance.transform.position;
        L_energy = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(!Player.Instance.isTalk &&
            GameManager.Exists)
        {
            if (Player.Instance.isFight)
            {
                L_energy -= Time.deltaTime * 1f;
            }
            if (Input.GetKeyDown(KeyCode.J) && Player.Instance.Keys.GetValueOrDefault("J") && !Player.Instance.isAttack)
            {
                Player.Instance.isAttack = true;
                Player.Instance.Attack("Fist", Player.Instance.fight_r);
                StartCoroutine(Player.Instance.SumTime());
            }
            else if (Input.GetKeyUp(KeyCode.J) && Player.Instance.Keys.GetValueOrDefault("J") && Player.Instance.isAttack)
            {
                Player.Instance.Keys["J"] = false;
                Player.Instance.isAttack = false;
                StartCoroutine(Player.Instance.EnableAfterDelay(0.3f, "J"));
            }
            else if (Input.GetKeyUp(KeyCode.I) && Player.Instance.isAbove && Player.Instance.isSaMu)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Player.Instance.transform.Find("Components").Find("Fire_Circle").gameObject.SetActive(false);
                Player.Instance.transform.Find("Components").Find("Fight_Circle").transform.position = new Vector2(transform.position.x, transform.position.y + 1.98f);
                Player.Instance.transform.Find("Components").Find("Fight_Circle").gameObject.SetActive(true);
                StartCoroutine(Player.Instance.Wait(1f));
            }
            else if (Input.GetKeyDown(KeyCode.L) && Player.Instance.Keys.GetValueOrDefault("L"))
            {
                if (L_energy > 0)
                {
                    Player.Instance.isFight = true;
                    Player.Instance.Attack("Ray", Player.Instance.Ray_r);
                }
            }
            else if (Input.GetKeyUp(KeyCode.L) && Player.Instance.Keys.GetValueOrDefault("L"))
            {
                Player.Instance.Keys["L"] = false;
                Player.Instance.isFight = false;
                StartCoroutine(Player.Instance.EnableAfterDelay(1f, "L"));
            }
            else if (Input.GetKeyDown(KeyCode.I) && Player.Instance.Keys.GetValueOrDefault("I") && Player.Instance.margic >= 100)
            {
                Player.Instance.margic -= 100;
                Player.Instance.isAbove = true;
                foreach (var temp in Player.Instance.Keys.Keys.ToList())
                {
                    Player.Instance.Keys[temp] = false;
                }
                Player.Instance.transform.Find("Components").Find("Fire_Circle").transform.position = Player.Instance.transform.position;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
                Player.Instance.transform.Find("Components").Find("Fire_Circle").gameObject.SetActive(true);
            }
        }
    }
}
