using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ResMgr : BaseManager<ResMgr>
{
    public T Load<T>(string path) where T : Object
    {
        // 1.加载资源   
        T res = Resources.Load<T>(path);
        if(res is GameObject)
        {
            // 2.实例化资源
            return GameObject.Instantiate(res);
        }
        else
        {
            // 3.直接返回资源
            return res;
        }
    }
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadAsync(name, callback));
    }
    //用协程来实现异步加载资源的功能，协程可以在等待资源加载完成的同时，不阻塞主线程，保持游戏的流畅性
    public IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        
        if(request.asset is GameObject)
        {
            //如果加载的资源是一个GameObject，则需要实例化后再返回
            callback(GameObject.Instantiate(request.asset) as T);
        }
        else
        {

            callback(request.asset as T);
        }
        
    }
}

