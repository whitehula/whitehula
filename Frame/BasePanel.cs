using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BasePanel : MonoBehaviour
{
    private Dictionary<string,List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();//使用里氏转换原则，将UIBehaviour转换为基类UIBehaviour，避免使用具体的实现类

    protected void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<TMP_Dropdown>();
        FindChildrenControl<TextMeshProUGUI>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<Slider>();
        // Button[] buttons = GetComponentsInChildren<Button>();
        // foreach (Button button in buttons)
        // {
        //     controlDic.Add(button.gameObject.name, button);
        // }

        // Image[] images = GetComponentsInChildren<Image>();
        // foreach (Image image in images)
        // {
        //     controlDic.Add(image.gameObject.name, image);
        // }

    }
    public virtual void ShowMe()
    {
        
    }
    public virtual void HideMe()
    {
        
    }
    protected virtual void OnClick(string controlName)
    {
        MusicMgr.Instance.PlaySound("Click");
    }

    /// <summary>
    /// 获取子对象的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        //protected指的是只能在当前类和子类中访问，外部无法访问，可用于基类
        if(controlDic.ContainsKey(controlName))
        {
            foreach(UIBehaviour control in controlDic[controlName])
            {
                if(control is T)
                {
                    return control as T;//使用as关键字将基类转换为子类，避免使用具体的实现类
                }
            }
        }
        Debug.LogError($"没有找到名为{controlName}的{typeof(T).Name}控件");
        return null;
        
    }




    /// <summary>
    /// 找子对象的控件
    /// </summary>
    /// <typeparam name="T"></typeparam> <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        
        foreach (T control in controls)
        {
            string controlName = control.gameObject.name;
            if(controlDic.ContainsKey(controlName))
            {
                controlDic[controlName].Add(control);//如果已经存在同名控件，则将新的控件添加到列表中，而不是覆盖原有的控件
                //比如按钮既有Button，又有image
            }
            else
            {
                controlDic.Add(controlName, new List<UIBehaviour>() { control });//如果不存在同名控件，则创建一个新的列表，并将控件添加到列表中
            }
            if(control is Button)
            {
                //碰到按钮就绑定事件
                (control as Button).onClick.AddListener(() => OnClick(controlName));
            }
            if (control is TextMeshProUGUI tmp)
            {
                if (tmp.GetComponentInParent<Button>() == null)
                {
                   // 只给按钮本体加缩放动画
UIManager.AddCustomEventListener(control, EventTriggerType.PointerEnter, (eventData) =>
{
    control.transform.DOKill();
    control.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.15f).SetEase(Ease.OutBack);
});
UIManager.AddCustomEventListener(control, EventTriggerType.PointerExit, (eventData) =>
{
    control.transform.DOKill();
    control.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
});
                }
            }
        else if (control is Button || control is Toggle || control is TMP_Dropdown  || control is Slider)
        {
           // 只给按钮本体加缩放动画
UIManager.AddCustomEventListener(control, EventTriggerType.PointerEnter, (eventData) =>
{
    control.transform.DOKill();
    control.transform.DOScale(new Vector3(1.1f, 1.1f, 1), 0.15f).SetEase(Ease.OutBack);
});
UIManager.AddCustomEventListener(control, EventTriggerType.PointerExit, (eventData) =>
{
    control.transform.DOKill();
    control.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
});
        }
        }
    }

}

