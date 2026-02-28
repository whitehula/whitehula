using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T> where T :class,new()
{
    //该类必须有一个无参构造函数（new())
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
    
}
