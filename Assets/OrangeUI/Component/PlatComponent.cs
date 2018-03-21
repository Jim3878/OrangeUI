using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlatComponent : MonoBehaviour
{
    public int ID;
    public List<ButtonComponent> buttonList=new List<ButtonComponent>();
    private PlatHandler _handler;
    public PlatHandler handler
    {
        get
        {
            if (_handler==null)
            {
                _handler = new PlatHandler(this.ID);
                _handler.onInitialize += OnPlatHandlerInitialize;
            }
            return _handler;
        }
    }
    public UnityEvent onInitialize = new UnityEvent();
    public bool isInitialize
    {
        get
        {
            return _handler!=null;
        }
    }

    protected virtual void OnPlatHandlerInitialize(object sender,EventArgs e)
    {
        onInitialize.Invoke();
        RegisterButtonHandlerList();
    }

    private void RegisterButtonHandlerList()
    {
        foreach (var btn in buttonList)
        {
            handler.RegistButtonHandler(btn.handler);
        }
    }
}
