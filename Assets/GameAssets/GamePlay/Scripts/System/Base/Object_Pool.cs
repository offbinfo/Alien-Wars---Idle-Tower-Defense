using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pool : MonoBehaviour
{
    public PoolTag _tag;
    protected Action<object> _OnInit;
    protected Action _OnReturn;
    public void OnInit(object obj)
    {
        _OnInit?.Invoke(obj);
    }
    public void OnReturn()
    {
        _OnReturn?.Invoke();
    }
    public void AddEventInit(Action<object> OnInit)
    {
        _OnInit += OnInit;
    }
    public void RemoveEventInit(Action<object> OnInit)
    {
        _OnInit -= OnInit;
    }
    public void AddEventReturn(Action OnReturn)
    {
        _OnReturn += OnReturn;
    }
    public void RemoveEventReturn(Action OnReturn)
    {
        _OnReturn -= OnReturn;
    }
    #region event animation 
    public virtual void ReturnPool()
    {
        _OnReturn?.Invoke(); 
        PoolCtrl.instance.Return(this);
    }
    #endregion
}
