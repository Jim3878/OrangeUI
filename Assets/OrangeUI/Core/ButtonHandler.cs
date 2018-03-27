using OrangeUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : IButtonHandler, ILogTraceable
{
    private string log;
    private IPlatHandler _platHanglder;
    public IPlatHandler platHandler
    {
        get
        {
            return _platHanglder;
        }
    }
    private int _ID;
    public int ID
    {
        get
        {
            return _ID;
        }
    }
    private bool _isEnable;
    public bool isEnable
    {
        get
        {
            return _isEnable;
        }
    }
    private bool _isInitialize;
    public bool isInitialize
    {
        get
        {
            return _isInitialize;
        }
    }
    private ButtonState _currentState;
    public ButtonState currentState
    {
        get
        {
            return _currentState;
        }
    }
    private bool _isTerminated;
    public bool isTerminated
    {
        get
        {
            return _isTerminated;
        }
    }

    public event EventHandler onEnable;
    public event EventHandler onDisable;
    public event EventHandler<ButtonStateArgs> onButtonStateChange;
    public event EventHandler onInitialize;
    public event EventHandler onTerminated;
    public event EventHandler onTrigger;

    public ButtonHandler(int ID)
    {
        this._ID = ID;
        _isTerminated = false;
        _isInitialize = false;
    }

    public void Initialize(IPlatHandler platHandler)
    {
        if (!isInitialize)
        {
            _platHanglder = platHandler;
            _isInitialize = true;
            _isTerminated = false;
            _isEnable = false;
            _currentState = ButtonState.NORMAL;
            if (onInitialize != null)
                onInitialize(this, new EventArgs());
            log += string.Format("Initialize()\n{0}\n\n", Utilty.CallStack());
        }
        else
        {
            log += string.Format("Initialize()\n然而早已Init過\n{0}\n\n", Utilty.CallStack());
        }
    }

    public void SetButtonState(ButtonState state)
    {
        LifeCheck();
        if (state != currentState)
        {
            _currentState = state;
            if (onButtonStateChange != null)
                onButtonStateChange(this, new ButtonStateArgs(state));
            log += string.Format("SetButtonState(state:{1})\n{0}\n\n", Utilty.CallStack(), state);
        }
        else
        {
            log += string.Format("SetButtonState(state:{1})\n但State是相同的\n{0}\n\n", Utilty.CallStack(), state);
        }
    }


    public void SetEnable(bool value)
    {
        LifeCheck();
        if (value != isEnable)
        {
            _isEnable = value;
            if (onEnable != null)
                switch (value)
                {
                    case true:
                        onEnable(this, new EventArgs());
                        break;
                    case false:
                        onDisable(this, new EventArgs());
                        break;
                    default:
                        break;
                }
            log += string.Format("SetEnable(value:{1})\n{0}\n\n", Utilty.CallStack(), value);
        }
        else if (value == isEnable)
        {
            log += string.Format("SetEnable(state:{1})\n然而isEnable==value\n{0}\n\n", Utilty.CallStack(), value);
        }
    }

    public void Terminate()
    {
        if (!_isTerminated)
        {
            _isTerminated = true;
            if (onTerminated != null)
                onTerminated(this, new EventArgs());
            log += string.Format("Terminated()\n{0}\n\n", Utilty.CallStack());
        }
        else
        {
            log += string.Format("Terminated()\n然而早已Terminated過\n{0}\n\n", Utilty.CallStack());
        }
    }

    public void Trigger()
    {
        LifeCheck();
        if (onTrigger != null)
            this.onTrigger(this, new EventArgs());
        log += string.Format("Trigger()\n{0}\n\n", Utilty.CallStack());
    }

    private void LifeCheck()
    {
        if (!isInitialize)
        {
            throw new Exception("按鈕還沒有初始化");
        }
        if (isTerminated)
        {
            throw new Exception("按鈕已經被終止");
        }
    }

    public string GetLog()
    {
        return log;
    }

    public void ClearLog()
    {
        log = "";
    }
}
