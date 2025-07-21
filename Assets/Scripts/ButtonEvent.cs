using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    // ����ί�����ͣ�����Ϊ������İ�ťʵ����
    public delegate void ButtonClickHandler(Button clickedButton);

    // ��̬�¼�������ʵ������
    public static event ButtonClickHandler Archive;
    public static event ButtonClickHandler Role;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            // �����¼������ݵ�ǰ��ťʵ��
            if (btn.name.Contains("Archive"))
            {
                Archive?.Invoke(btn);
            }
            else
            {
                Role?.Invoke(btn);
            }
        });
    }
}
