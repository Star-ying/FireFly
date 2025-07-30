using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��չQuestData_SO��֧��������¼
[System.Serializable]
public class QuestCondition
{
    public string targetType; // ��"Kill"��"Collect"
    public int currentAmount = 0;
    public int requiredAmount;
}
public class QuestData_SO
{
    public string questName;
    public string description;
    public QuestCondition condition = new QuestCondition();
    public bool isComplete = false; // �������״̬
    public List<(string, int)> Rewards = new List<(string, int)>();
}
// ���������������ģʽ��
public class QuestManager : MonoBehaviour
{
    public Canvas canvas3;
    public static QuestManager Instance;
    public Dictionary<string,QuestData_SO> activeQuests = new(); // ��ǰ���������
    public int page = 0;

    private void Awake()
    {
        Instance = this;
    }
    // �����¼�
    public void OnConditioned(string enemyType)
    {
        foreach (var quest in activeQuests)
        {
            if (!quest.Value.isComplete)
            {
                if (quest.Value.condition.targetType == enemyType && quest.Value.condition.currentAmount < quest.Value.condition.requiredAmount)
                {
                    quest.Value.condition.currentAmount++;
                    if (quest.Value.condition.currentAmount >= quest.Value.condition.requiredAmount)
                    {
                        canvas3.transform.Find("UI_Massage").Find("Text").GetComponent<Text>().text = $"����{quest.Value.questName}�����";
                        canvas3.gameObject.SetActive(true);
                        StartCoroutine(Wait());
                    }
                }
            }
        }
    }
    public void ShowTask()
    {
        int i = 0;
        canvas3.transform.Find("UI").Find("Task").Find("Page").GetComponent<InputField>().text = $"{page + 1}";
        canvas3.transform.Find("UI").Find("Task").Find("Last").gameObject.SetActive(!(page == 0));
        canvas3.transform.Find("UI").Find("Task").Find("Next").gameObject.SetActive(page * 8 < activeQuests.Count - 8);
        foreach(var quest in activeQuests.Values)
        {
            if(i + 8 * page < activeQuests.Count)
            {
                GameObject task = (GameObject)Instantiate(Resources.Load("ģ��/task"), canvas3.transform.Find("UI").Find("Task"));
                task.transform.Find("name").GetComponent<Text>().text = quest.questName;
                task.transform.GetComponent<Slider>().value = quest.condition.currentAmount / quest.condition.requiredAmount;
                task.transform.Find("Completeness").GetComponent<Text>().text = $"{quest.condition.currentAmount}/{quest.condition.requiredAmount}";
                task.transform.position = canvas3.transform.Find("UI").Find("Task").Find($"task{i - page * 8 + 1}").transform.position;
            }
        }
    }
    public void NextPage()
    {
        page++;
        ShowTask();
    }
    public void LastPage()
    {
        page--;
        ShowTask();
    }
    public void CancelCheck()
    {
        page = 0;
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        canvas3.gameObject.SetActive(false);
    }
}
