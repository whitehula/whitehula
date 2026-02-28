using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr :  BaseManager<MonoMgr>
{
    private MonoController controller;
    public MonoMgr()
    {
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
        Object.DontDestroyOnLoad(obj);
    }
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    public Coroutine StartCoroutine(string methodName)
    {
        //参数为字符串时，必须保证该字符串对应的方法存在于controller所在的GameObject上，否则会抛出异常
        return controller.StartCoroutine(methodName);
    }
     public Coroutine StartCoroutine(string methodName, object value)
    {
        return controller.StartCoroutine(methodName, value);
    }

}
