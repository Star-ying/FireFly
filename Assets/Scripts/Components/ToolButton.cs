using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToolButton : Button
{
    // 定义状态变化事件
    [SerializeField]
    public UnityEvent onSelectedToNormal = new();
    public bool onSelect = false;

    // 记录上一个状态
    private SelectionState previousState = SelectionState.Normal;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        // 检测状态变化：Selected → Normal
        if (previousState == SelectionState.Selected &&
            state == SelectionState.Normal)
        {
            if (!onSelect)
            {
                onSelectedToNormal?.Invoke();
            }
            else
            {
                return;
            }
        }

        previousState = state; // 更新状态记录
        base.DoStateTransition(state, instant); // 执行原始状态转换
    }
}

