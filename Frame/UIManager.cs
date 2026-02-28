using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public enum UILayer
{
    Bot,
    Mid,
    Top,
    System
}
public class UIManager : BaseManager<UIManager>
{
    
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform top;
    private Transform mid;
    private Transform system;
    //记录UIcanvas父对象
    public RectTransform canvas;
     public UIManager()
    {
        //找到canvas
        GameObject obj = ResMgr.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);
        GameObject.DontDestroyOnLoad(ResMgr.Instance.Load<GameObject>("UI/EventSystem"));
        //找到各个层级
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

    }
/// <summary>
/// 管理显示UI界面的方法，参数为界面名称，层级和回调函数
/// </summary>
/// <param name="panelName">面板名</param>
/// <param name="layer">显示在那一层</param>
/// <param name="callback">面板创建成功后，你想做的事情</param>
    public void ShowPanel<T>(string panelName,UILayer layer = UILayer.Mid,UnityAction<T> callback = null) where T : BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if(callback != null)
            callback(panelDic[panelName] as T);
            return;
        }
        ResMgr.Instance.LoadAsync<GameObject>("UI/"+panelName, (obj) =>
        {
            //把他作为canvas的子物体，并设置位置和缩放
            Transform father = bot;
            switch(layer)
            {
                case UILayer.Mid:
                    father = mid;
                    break;
                case UILayer.Top:
                    father = top;
                    break;
                case UILayer.System:
                    father = system;
                    break;
            }
            //设置相对位置和大小
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;//相对位置不变
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;//右上角
            (obj.transform as RectTransform).offsetMin = Vector2.zero;//左下角,避免偏移

            T panel = obj.GetComponent<T>();
            if(panel != null && callback != null)
            {
                callback(panel);
            }
            panelDic.Add(panelName,panel);
            panel.ShowMe();
        });
    }

    public void HidePanel(string panelName)
    {
        if(panelDic.TryGetValue(panelName,out BasePanel panel))
        {
            panelDic.Remove(panelName);
            GameObject.Destroy(panel.gameObject);
        }
    }
    
    /// <summary>
    /// 得到一个已经存在的面板
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if(panelDic.TryGetValue(name,out BasePanel panel))
        {
            return panel as T;
        }
        Debug.LogError($"没有找到名为{name}的面板");
        return null;
    }
    public Transform GetLayer(UILayer layer)
    {
        switch(layer)   
        {
            case UILayer.Mid:
                return mid;
            case UILayer.Top:
                return top;
            case UILayer.System:
                return system;
            case UILayer.Bot:
                return bot;
            default:
                return null;
                
        }
    }
    /// <summary>
    /// 添加自定义事件监听
    /// </summary>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType eventType,UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        //添加事件监听
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //事件类型，如鼠标悬停
        entry.eventID = eventType;
        entry.callback.AddListener(callback);
        //添加响应函数到事件触发器中
        trigger.triggers.Add(entry);
        
    }
}
