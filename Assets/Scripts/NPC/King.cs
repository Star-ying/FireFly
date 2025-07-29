using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class King : MonoBehaviour
{
    List<string[]> massage = new List<string[]>();
    private int i = 0, j = 0;
    private QuestData_SO task;
    private QuestCondition condition;
    private bool space = true;
    private bool willtalk = false;
    public Text text;
    public Canvas canvas3;
    public Canvas canvas4;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            willtalk = true;
            if (QuestManager.Instance.activeQuests.TryGetValue("King",out _))
            {
                i = 4;
                j = 0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            willtalk = false;
        }
    }
    private void Update()
    {
        if (willtalk)
        {
            if (Input.GetKeyDown(KeyCode.Space) && space)
            {
                if(i < massage.Count && j < massage[i].Length)
                {
                    text.text = massage[i][j];
                    canvas4.gameObject.SetActive(true);
                    canvas3.gameObject.SetActive(false);
                    if (massage[i][j].Equals("��Ը����������𣬴�˵�е�Ӣ�ۣ�"))
                    {
                        BiginSelect(Yes0, No0);
                        Select();
                    }
                    else if (massage[i][j].Equals("���ֻ����ˣ����ı������ˣ�"))
                    {
                        Select();
                    }
                    else if (massage[i][j].Equals("�����Ƕ��Ϸ�������һȺ�����ˣ�����ȥ������ǡ�"))
                    {
                        ShowReward(new string[] {"��ө������ x1","���� x200" });
                        BiginSelect(Yes2, No2);
                        Select();
                    }
                    else if (massage[i][j].Equals("��л���ѹ�������ɾ����������ı��꣡"))
                    {

                    }
                    else
                    {
                        j++;
                    }
                    Player.Instance.isTalk = true;
                }
                else
                {
                    j--;
                    EndTalk();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space) && Player.Instance.isTalk)
            {
                space = false;
                StartCoroutine(Wait());
            }
        }
    }
    private void ShowReward(string[] reward)
    {
        canvas4.transform.Find("Reward").Find("reward").GetComponent<Text>().text = $"{string.Join("\n",reward)}";
        canvas4.transform.Find("Reward").gameObject.SetActive(true);
    }
    private void BiginSelect(UnityEngine.Events.UnityAction yes, UnityEngine.Events.UnityAction no)
    {
        canvas4.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(yes);
        canvas4.transform.Find("No").GetComponent<Button>().onClick.AddListener(no);
    }
    private void Select()
    {
        canvas4.transform.Find("Yes").gameObject.SetActive(true);
        canvas4.transform.Find("No").gameObject.SetActive(true);
    }
    private void EndSelect()
    {
        canvas4.transform.Find("Yes").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas4.transform.Find("No").GetComponent<Button>().onClick.RemoveAllListeners();
        canvas4.transform.Find("Yes").gameObject.SetActive(false);
        canvas4.transform.Find("No").gameObject.SetActive(false);
    }
    private void EndTalk()
    {
        canvas3.gameObject.SetActive(true);
        Player.Instance.isTalk = false;
        canvas4.gameObject.SetActive(false);
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        space = true;
    }
    private void Yes0()
    {
        text.text = "�����̫��л�ˣ������о���";
        i = 2;
        j = 0;
        EndSelect();
        EndTalk();
    }
    public void No0()
    {
        text.text = "������ı����뷨��һ��Ҫ���������ǻ���ָ�л��";
        i = 1;
        j = 0;
        EndTalk();
    }
    public void Yes2()
    {
        text.text = "��л���İ�������������������ĺ���Ϣ��";
        i = 3;
        j = 0;
        EndSelect();
        condition.targetType = "������";
        condition.requiredAmount = 10;
        condition.currentAmount = 10;
        task.questName= "���������ˣ�������";
        task.isComplete = true;
        task.condition = condition;
        task.Rewards.Add(("43001",1));
        task.Rewards.Add(("Exp", 200));
        QuestManager.Instance.activeQuests.Add("King",task);
        canvas4.transform.Find("Reward").gameObject.SetActive(false);
        EndSelect();
        EndTalk();
    }
    public void No2()
    {
        text.text = "��һ�����˰ɣ�������Ϣ��";
        i = 1;
        j = 0;
        EndTalk();
    }
    private void Awake()
    {
        massage.Add(new string[] { "�����Ǵ�˵�е�Ӣ��������һֱ�ڵȴ����ĵ���ѽ��","100��ǰ���ǵ�����ͻȻ�����⽵���˺��࣬����Ⱦ�����нӴ����˺Ͷ�������Ժ����ǵ����ȱ�����Щ���\���������","ͬ����һͬ���ٵģ�����һ��Ԥ�ԣ��������һλӢ�۽��������ǡ���", "��Ը����������𣬴�˵�е�Ӣ�ۣ�" });
        massage.Add(new string[] { "���ֻ����ˣ����ı������ˣ�" });
        massage.Add(new string[] { "�����Ƕ��Ϸ�������һȺ�����ˣ�����ȥ������ǡ�" });
        massage.Add(new string[] { "����û���������ɣ�" });
        massage.Add(new string[] { "��л���ѹ�������ɾ����������ı��꣡" });
        task = new QuestData_SO();
        condition = new QuestCondition();
    }
}
