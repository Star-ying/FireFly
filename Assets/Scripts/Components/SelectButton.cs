using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButton : Button
{
    // 记录上一个状态
    private SelectionState previousState = SelectionState.Normal;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        // 检测状态变化：Selected → Normal
        if (previousState == SelectionState.Normal &&
            state == SelectionState.Highlighted)
        {
            BagManager.Instanse.tool_Button.GetComponent<ToolButton>().onSelect = true;
        }
        else if(state == SelectionState.Normal)
        {
            BagManager.Instanse.tool_Button.GetComponent<ToolButton>().onSelect = false;
        }

        previousState = state; // 更新状态记录
        base.DoStateTransition(state, instant); // 执行原始状态转换
    }
    // 鼠标抬起事件处理
    public override void OnPointerUp(PointerEventData eventData)
    {
        BagManager.Instanse.Equip();
        BagManager.Instanse.CancelSelect();
        base.OnPointerUp(eventData); // 保持原有点击逻辑
    }
}
