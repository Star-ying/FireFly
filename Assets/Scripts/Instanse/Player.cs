using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get;private set; }

    public Queue<GameObject> Ripple = new Queue<GameObject>();
    public Queue<GameObject> Bullet = new Queue<GameObject>();
    public Dictionary<string, bool> Keys = new Dictionary<string, bool>();
    public Dictionary<string, int> Base_Property = new Dictionary<string, int>();
    public Dictionary<string, int> Property = new Dictionary<string, int>();

    public bool isSaMu = false;
    public bool isFight = false;
    public bool isAttack = false;
    public bool isAbove = false;
    public bool isFire = false;
    public bool isTalk = false;

    public int Level;
    public int Max_Exp = 0;

    public int margic = 0;
    public int Exp;

    public float fight_r = 2f;
    public float Ray_r = 10f;

    private int bullets = 0;
    private int bulletSize = 20;
    private float time = 0;

    private float moveSpeed = 15f;
    private Vector2 movement;
    private float x = 0;
    private float y = 0;

    public void Awake()
    {
        Instance = this;
        transform.position = new Vector2(0, 0);
        Keys.Add("I", true);
        Keys.Add("J", true);
        Keys.Add("L", true);
        Keys.Add("K", true);
        InitProperty();
        transform.Find("Components").transform.position = new Vector2(transform.position.x + fight_r, transform.position.y);
        for (int i = 0; i < bulletSize; i++)
        {
            GameObject obj = Instantiate(transform.Find("Components").Find("Bullet").gameObject);
            obj.name = $"{i}";
            obj.SetActive(false);
            obj.GetComponent<Bullet>().SetSpawner(gameObject); // 挂载下方脚本
            Bullet.Enqueue(obj);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = Instantiate(transform.Find("Components").Find("Ripple").gameObject);
            obj.name = $"{i}";
            obj.SetActive(false);
            Ripple.Enqueue(obj);
        }
    }
    public GameObject GetPooledObject(Queue<GameObject> PooledObject)
    {
        if (PooledObject.Count > 0)
        {
            GameObject obj = PooledObject.Dequeue();
            return obj;
        }
        return null; // 明确返回 null 表示池已空
    }
    public void ReturnPooledObject(GameObject obj, Queue<GameObject> PooledObject)
    {
        obj.SetActive(false);
        PooledObject.Enqueue(obj);
    }
    public void Update()
    {
        if (!isTalk &&
            GameManager.Exists)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            // 计算移动方向
            x = horizontal == 0 ? 0 : Mathf.Abs(horizontal) / horizontal;
            y = vertical == 0 ? 0 : Mathf.Abs(vertical) / vertical;
            if (x == 0 || y == 0)
            {
                movement = new Vector2(x, y);
            }
            else
            {
                movement = new Vector2(x, y) / Mathf.Pow(Mathf.Abs(x) + Mathf.Abs(y), 0.5f);
            }

            // 应用移动
            transform.Translate(movement * Time.deltaTime * moveSpeed, Space.World);
        }
    }
    public void Attack(string Gameobject, float r)
    {
        if (x == 0 && y == 0)
        {
            transform.Find("Components").Find(Gameobject).transform.position = new Vector2(transform.position.x + r, transform.position.y);
        }
        else if (x != 0 && y != 0)
        {
            transform.Find("Components").Find(Gameobject).transform.eulerAngles = new Vector3(0, 0, x > 0 ? 45 * y / Mathf.Abs(y) : 135 * y / Mathf.Abs(y));
            transform.Find("Components").Find(Gameobject).transform.position = new Vector2(transform.position.x + x / Mathf.Pow(Mathf.Abs(x) + Mathf.Abs(y), 0.5f) * r, transform.position.y + y / Mathf.Pow(Mathf.Abs(x) + Mathf.Abs(y), 0.5f) * r);
        }
        else
        {
            transform.Find("Components").Find(Gameobject).transform.eulerAngles = new Vector3(0, 0, x == 0 ? 90 * y / Mathf.Abs(y) : x > 0 ? 0 : 180);
            transform.Find("Components").Find(Gameobject).transform.position = new Vector2(transform.position.x + x * r, transform.position.y + y * r);
        }
        transform.Find("Components").Find(Gameobject).gameObject.SetActive(true);
    }
    public IEnumerator SumTime()
    {
        while (isSaMu &&
            isAttack)
        {
            time += Time.deltaTime;
            if (time >= 3f)
            {
                time = 0;
                Keys["J"] = false;
                isAttack = false;
                StartCoroutine(EnableAfterDelay(0.3f, "J"));
                transform.Find("Components").Find("Fist").gameObject.SetActive(false);
                transform.Find("Components").Find("BigFist").transform.position = transform.Find("Components").Find("Fist").transform.position;
                transform.Find("Components").Find("BigFist").transform.eulerAngles = transform.Find("Components").Find("Fist").transform.eulerAngles;
                transform.Find("Components").Find("BigFist").gameObject.SetActive(true);
                StartCoroutine(FistSleep());
            }
            yield return new WaitForFixedUpdate();
        }
        time = 0;
        transform.Find("Components").Find("Fist").gameObject.SetActive(false);

    }
    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        transform.Find("Components").Find("Fight_Circle").gameObject.SetActive(false);
        isAbove = false;
        foreach (var temp in Keys.Keys.ToList())
        {
            Keys[temp] = true;
        }
        transform.Find("Role").Find("samu").GetComponent<CircleCollider2D>().enabled = true;
    }
    public IEnumerator dodge()
    {
        moveSpeed *= 2f;
        transform.Find("Role").Find("samu").GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        moveSpeed /= 2f;
        transform.Find("Role").Find("samu").GetComponent<CircleCollider2D>().enabled = true;
    }
    public IEnumerator FistSleep()
    {
        yield return new WaitForSeconds(0.3f);
        transform.Find("Components").Find("BigFist").gameObject.SetActive(false);
    }
    public IEnumerator SpawnRipple()
    {
        while (isSaMu &&
            GameManager.Exists)
        {
            yield return new WaitForSeconds(0.8f);
            GameObject obj = GetPooledObject(Ripple);
            obj.transform.position = transform.position;
            obj.transform.localScale = transform.Find("Components").Find("Ripple").transform.localScale;
            obj.SetActive(true);
        }
    }
    public IEnumerator CountDown()
    {
        yield return new WaitForSeconds(20f);
        transform.Find("Animitor").Find("Meta2").transform.position = transform.position;
        transform.Find("Animitor").Find("Meta2").gameObject.SetActive(true);
        transform.Find("Role").Find("liuying").gameObject.SetActive(true);
        transform.Find("Role").Find("liuying").transform.position = transform.position;
        yield return new WaitForSeconds(0.3f);
        transform.Find("Role").Find("samu").gameObject.SetActive(false);
        transform.Find("Animitor").Find("Meta2").gameObject.SetActive(false);
        isSaMu = false;
    }
    public IEnumerator EnableAfterDelay(float delay, string key)
    {
        yield return new WaitForSeconds(delay);
        Keys[key] = true;
    }
    public IEnumerator beSaMu()
    {
        yield return new WaitForSeconds(0.4f);
        transform.Find("Role").Find("liuying").gameObject.SetActive(false);
        transform.Find("Role").Find("samu").transform.position = transform.position;
        transform.Find("Role").Find("samu").gameObject.SetActive(true);
        transform.Find("Animitor").transform.Find("Metamorphose").gameObject.SetActive(false);
        isSaMu = true;
        foreach (var temp in Keys.Keys.ToList())
        {
            Keys[temp] = true;
        }
        StartCoroutine(CountDown());
        StartCoroutine(SpawnRipple());
    }
    public IEnumerator AttackSleep()
    {
        yield return new WaitForSeconds(0.1f);
        transform.Find("Components").transform.Find("Fight_Square").gameObject.SetActive(false);
    }
    public IEnumerator SpawnRoutine()
    {
        while (bullets < bulletSize &&
            isFire &&
            GameManager.Exists)
        {
            bullets++;
            GameObject obj = GetPooledObject(Bullet);
            obj.transform.position = transform.position;
            if (x == 0 && y == 0)
            {
                obj.GetComponent<Bullet>().movement = new Vector2(1, 0);
            }
            else
            {
                obj.GetComponent<Bullet>().movement = movement;
            }
            obj.SetActive(true);
            yield return new WaitForSeconds(0.4f);
        }

    }
    public IEnumerator HealRoutine()
    {
        while (bullets > 0 &&
            !isFire &&
            GameManager.Exists)
        {
            bullets--;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void InitProperty()
    {
        Base_Property.Add("Base_health", 0);
        Base_Property.Add("Base_margic", 0);
        Base_Property.Add("Base_attack", 0);
        Base_Property.Add("Base_defense", 0);
        Base_Property.Add("AttachedHit",0);
        Base_Property.Add("CriticalHit", 0);
        Base_Property.Add("CriticalInjury", 0);
        Property.Add("health", 0);
        Property.Add("margic", 0);
        Property.Add("attack", 0);
        Property.Add("defense", 0);
        Property.Add("AttachedHit", 0);
        Property.Add("CriticalHit", 0);
        Property.Add("CriticalInjury", 0);
    }
    public void SetProperty(int HP,int MP,int ATK,int DFS)
    {
        Base_Property["Base_health"] = HP;
        Base_Property["Base_margic"] = MP;
        Base_Property["Base_attack"] = ATK;
        Base_Property["Base_defense"] = DFS;
        Property["health"] = HP;
        Property["attack"] = ATK;
        Property["defense"] = DFS;
        transform.Find("Equipment").gameObject.SetActive(false);
        transform.Find("Equipment").gameObject.SetActive(true);
    }
    public void MakeProperty(int HP,int MP,int ATK,int DFS)
    {
        Base_Property["Base_health"] += HP;
        Base_Property["Base_margic"] += MP;
        Base_Property["Base_attack"] += ATK;
        Base_Property["Base_defense"] += DFS;
        Property["health"] += HP;
        Property["attack"] += ATK;
        Property["defense"] += DFS;
        margic += MP;
    }
    public void AddProperty(string property,float rate)
    {
        Property[property] += (int)(Base_Property[$"Base_{property}"] * rate);
    }
    public void AddProperty1(string property,int number)
    {
        Base_Property[property] += number;
        Property[property] += number;
    }
    public void SetEquipment(string name, string position)
    {
        Sprite sprite = Resources.Load<Sprite>(name);
        GameManager.Instance.Canvas.transform.Find("UI").Find("Equipment").Find(position).GetComponent<Image>().sprite = sprite;
        try
        {
            transform.Find("Equipment").Find($"WPN_{name}").gameObject.SetActive(true);
        }
        catch (Exception)
        {

        }
    }
}
