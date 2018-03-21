using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ButtonComponent : MonoBehaviour
{

    public int ID;
    private ButtonHandler _handler;
    public ButtonHandler handler
    {
        get
        {
            if (_handler == null)
            {
                _handler = new ButtonHandler(ID);
                _handler.onInitialize += OnButtonHandlerInitialize;

                return _handler;
            }
            return _handler;

        }
    }
    public bool isInitialize
    {
        get
        {
            return _handler != null;
        }
    }
    public UnityEvent onInitialzie = new UnityEvent();



    protected virtual void OnButtonHandlerInitialize(object sender, EventArgs e)
    {
        onInitialzie.Invoke();
    }

}
