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
                    if (massage[i][j].Equals("你愿意帮助我们吗，传说中的英雄？"))
                    {
                        BiginSelect(Yes0, No0);
                        Select();
                    }
                    else if (massage[i][j].Equals("您又回来了，您改变主意了？"))
                    {
                        Select();
                    }
                    else if (massage[i][j].Equals("在主城东南方向上有一群丘丘人，请您去清缴它们。"))
                    {
                        ShowReward(new string[] {"流萤变身器 x1","经验 x200" });
                        BiginSelect(Yes2, No2);
                        Select();
                    }
                    else if (massage[i][j].Equals("感谢您把怪物清理干净，这是您的报酬！"))
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
        text.text = "真的是太感谢了，我们有救了";
        i = 2;
        j = 0;
        EndSelect();
        EndTalk();
    }
    public void No0()
    {
        text.text = "如果您改变了想法，一定要回来，我们会万分感谢的";
        i = 1;
        j = 0;
        EndTalk();
    }
    public void Yes2()
    {
        text.text = "感谢您的帮助！我们在这里等您的好消息！";
        i = 3;
        j = 0;
        EndSelect();
        condition.targetType = "丘丘人";
        condition.requiredAmount = 10;
        condition.currentAmount = 10;
        task.questName= "清理丘丘人（国王）";
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
        text.text = "您一定累了吧，请多多休息！";
        i = 1;
        j = 0;
        EndTalk();
    }
    private void Awake()
    {
        massage.Add(new string[] { "您就是传说中的英雄吗，我们一直在等待您的到来呀！","100年前我们的世界突然从天外降下了黑泥，它感染了所有接触的人和动物，自那以后我们的祖先便与这些“黒物”做斗争。","同黑泥一同降临的，还有一则预言：“百年后一位英雄将拯救你们”。", "你愿意帮助我们吗，传说中的英雄？" });
        massage.Add(new string[] { "您又回来了，您改变主意了？" });
        massage.Add(new string[] { "在主城东南方向上有一群丘丘人，请您去清缴它们。" });
        massage.Add(new string[] { "您还没有完成任务吧！" });
        massage.Add(new string[] { "感谢您把怪物清理干净，这是您的报酬！" });
        task = new QuestData_SO();
        condition = new QuestCondition();
    }
}
