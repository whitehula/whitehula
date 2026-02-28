using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 周期函数、协程和事件    
/// </summary> <summary>
/// 
/// </summary>
public class MonoController : MonoBehaviour
{
    public event UnityAction OnUpdate;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if(OnUpdate != null)
        {
            OnUpdate();//即使没有继承monobehavior，也可以通过事件来调用Update函数，达到周期调用的效果
        }
    }
    public void AddUpdateListener(UnityAction fun)
    {
        OnUpdate += fun;
    }
    public void RemoveUpdateListener(UnityAction fun)
    {
        OnUpdate -= fun;
    }
}
