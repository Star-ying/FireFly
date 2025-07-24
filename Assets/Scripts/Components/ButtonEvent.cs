using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    // 定义委托类型（参数为被点击的按钮实例）
    public delegate void ButtonClickHandler(Button clickedButton);

    // 静态事件（所有实例共享）
    public static event ButtonClickHandler Archive;
    public static event ButtonClickHandler Role;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            // 触发事件并传递当前按钮实例
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
