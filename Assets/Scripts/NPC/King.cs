using System.Collections.Generic;
using UnityEngine;
public class King : MonoBehaviour
{
    List<string[]> massage = new();
    private int progress = 0;
    private int Serial_number = 0;
    private QuestData_SO task;
    private QuestCondition condition;
    private bool willtalk = false;
    private void OnEnable()
    {
        massage.Add(new string[] { "您就是传说中的英雄吗，我们一直在等待您的到来呀！","100年前我们的世界突然从天外降下了黑泥，它感染了所有接触的人和动物，自那以后我们的祖先便与这些“黒物”做斗争。","同黑泥一同降临的，还有一则预言：“百年后一位英雄将拯救你们”。", "你愿意帮助我们吗，传说中的英雄？" });
        massage.Add(new string[] { "您又回来了，您改变主意了？" });
        massage.Add(new string[] { "在主城东南方向上有一群丘丘人，请您去清缴它们。" });
        massage.Add(new string[] { "您还没有完成任务吧！" });
        massage.Add(new string[] { "感谢您把怪物清理干净，这是您的报酬！" });
        task = new QuestData_SO();
        condition = new QuestCondition();
        progress = ProgressManager.Instanse.Progress["King"];
    }
    private void Update()
    {
        if (willtalk)
        {
            if (Input.GetKeyDown(KeyCode.Space) && ProgressManager.Instanse.space)
            {
                if(progress < massage.Count && Serial_number < massage[progress].Length)
                {
                    ProgressManager.Instanse.text.text = massage[progress][Serial_number];
                    ProgressManager.Instanse.canvas4.gameObject.SetActive(true);
                    ProgressManager.Instanse.canvas3.gameObject.SetActive(false);
                    if (massage[progress][Serial_number].Equals("你愿意帮助我们吗，传说中的英雄？"))
                    {
                        ProgressManager.Instanse.BiginSelect(Yes0, No0);
                        ProgressManager.Instanse.Select();
                    }
                    else if (massage[progress][Serial_number].Equals("您又回来了，您改变主意了？"))
                    {
                        ProgressManager.Instanse.Select();
                    }
                    else if (massage[progress][Serial_number].Equals("在主城东南方向上有一群丘丘人，请您去清缴它们。"))
                    {
                        ProgressManager.Instanse.ShowReward(new string[] {"流萤变身器 x1","经验 x200" });
                        ProgressManager.Instanse.BiginSelect(Yes2, No2);
                        ProgressManager.Instanse.Select();
                    }
                    else if (massage[progress][Serial_number].Equals("感谢您把怪物清理干净，这是您的报酬！"))
                    {
                        ProgressManager.Instanse.GetReward_Property(new Dictionary<string, int> {["Exp"] = 200 });
                        ProgressManager.Instanse.GetReward_Tool(new Dictionary<string, int> { ["43001"] = 1 });
                        QuestManager.Instance.activeQuests.Remove("King");
                        Serial_number = 0;
                        progress = 5;
                    }
                    else
                    {
                        Serial_number++;
                    }
                    Player.Instance.isTalk = true;
                }
                else
                {
                    ProgressManager.Instanse.EndTalk();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space) && Player.Instance.isTalk)
            {
                ProgressManager.Instanse.space = false;
                StartCoroutine(ProgressManager.Instanse.Wait());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            willtalk = true;
            if (QuestManager.Instance.activeQuests.TryGetValue("King",out _))
            {
                progress = 4;
                Serial_number = 0;
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
    private void OnDisable()
    {
        ProgressManager.Instanse.Progress["King"] = progress;
    }
    private void Yes0()
    {
        ProgressManager.Instanse.text.text = "真的是太感谢了，我们有救了";
        progress = 2;
        Serial_number = 0;
        ProgressManager.Instanse.EndSelect();
        ProgressManager.Instanse.EndTalk();
    }
    public void No0()
    {
        ProgressManager.Instanse.text.text = "如果您改变了想法，一定要回来，我们会万分感谢的";
        progress = 1;
        Serial_number = 0;
        ProgressManager.Instanse.EndTalk();
    }
    public void Yes2()
    {
        ProgressManager.Instanse.text.text = "感谢您的帮助！我们在这里等您的好消息！";
        progress = 3;
        Serial_number = 0;
        ProgressManager.Instanse.EndSelect();
        condition.targetType = "丘丘人";
        condition.requiredAmount = 10;
        condition.currentAmount = 10;
        task.questName= "清理丘丘人（国王）";
        task.isComplete = true;
        task.condition = condition;
        task.Rewards.Add(("43001",1));
        task.Rewards.Add(("Exp", 200));
        QuestManager.Instance.activeQuests.Add("King",task);
        ProgressManager.Instanse.canvas4.transform.Find("Reward").gameObject.SetActive(false);
        ProgressManager.Instanse.EndSelect();
        ProgressManager.Instanse.EndTalk();
    }
    public void No2()
    {
        ProgressManager.Instanse.text.text = "您一定累了吧，请多多休息！";
        progress = 1;
        Serial_number = 0;
        ProgressManager.Instanse.EndTalk();
    }
}
