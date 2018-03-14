using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrangeUtility;

public class PlatHandler : IPlatHandler, ILogTraceable
{

    private string log = "";
    private IOrange _orange;
    public IOrange orange
    {
        get
        {
            return _orange;
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
    private bool _isShow;
    public bool isShow
    {
        get
        {
            return _isShow;
        }
    }
    private bool _isEnable;
    public bool isEnable
    {
        get
        {
            if (isTerminate)
            {
                return false;
            }
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
    private bool _isTerminate;
    public bool isTerminate
    {
        get
        {
            return _isTerminate;
        }
    }

    public event EventHandler<ShowHideArgs> onShow;
    public event EventHandler<ShowHideArgs> onHide;
    public event EventHandler<RegistButtonArgs> onButtonRegisted;
    public event EventHandler<RemoveButtonArgs> onButtonRemoved;
    public event EventHandler<TriggerButtonArgs> onButtonTrigger;
    public event EventHandler onEnable;
    public event EventHandler onDisable;
    public event EventHandler onInitialize;
    public event EventHandler onTerminate;

    private Dictionary<int, IButtonHandler> buttonHandlerList = new Dictionary<int, IButtonHandler>();

    public PlatHandler(int ID)
    {
        this._ID = ID;
        _isTerminate = false;
        log += string.Format("new()\n{0}\n\n", Utilty.CallStack());
    }

    public void CompleteHide()
    {
        if (!isTerminate)
        {
            _isShow = false;
            if (onHide != null)
                onHide(this, new ShowHideArgs(true));
            log += string.Format("CompleteHide()\n{0}\n\n", Utilty.CallStack());
        }
    }

    public void CompleteShow()
    {
        if (!isTerminate)
        {
            _isShow = true;
            if (onShow != null)
                onShow(this, new ShowHideArgs(true));
            log += string.Format("CompleteShow()\n{0}\n\n", Utilty.CallStack());
        }
    }

    public IButtonHandler[] GetAllButton()
    {
        IButtonHandler[] buttonHandlerArr = new IButtonHandler[buttonHandlerList.Count];
        int i = 0;
        foreach (var keyValuePair in buttonHandlerList)
        {
            buttonHandlerArr[i] = keyValuePair.Value;
            i++;
        }
        log += string.Format("GetAllButton()\n{0}\n\n", Utilty.CallStack());
        return buttonHandlerArr;
    }

    public IButtonHandler GetButton(int id)
    {
        try
        {
            if (buttonHandlerList.ContainsKey(id))
            {
                log += string.Format("GetButton( id:{1} )\n{0}\n\n", Utilty.CallStack(), id);
                return buttonHandlerList[id];
            }
            throw new NullReferenceException(string.Format("哼哼、Button {0}號打從一開始就不存在過，了解了嗎？"));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Hide()
    {
        if (!isTerminate && isEnable && isShow)
        {
            _isShow = false;
            if (onHide != null)
                onHide(this, new ShowHideArgs());
            log += string.Format("Hide()\n{0}\n\n", Utilty.CallStack());
        }
        else
            log += string.Format("Hide()\n但是介面已經關了\n{0}\n\n", Utilty.CallStack());
    }

    public void Show()
    {
        if (!isTerminate && isEnable && !isShow)
        {
            _isShow = true;
            if (onShow != null)
                onShow(this, new ShowHideArgs());
            log += string.Format("Show()\n{0}\n\n", Utilty.CallStack());
        }
        else
            log += string.Format("Show()\n但是介面已經開了\n{0}\n\n", Utilty.CallStack());
    }

    public void Initialize(IOrange orange)
    {
        if (!isInitialize)
        {
            _isInitialize = true;
            this._orange = orange;
            CompleteHide();
            SetEnable(false);
            if (onInitialize != null)
                onInitialize(this, new EventArgs());
            InitializeButtonList();
            log += string.Format("Initialize()\n{0}\n\n", Utilty.CallStack());
        }
        else
            log += string.Format("Initialize()\n但早就Initialize過了\n{0}\n\n", Utilty.CallStack());
    }

    public void RegistButtonHandler(IButtonHandler btn)
    {
        int id = btn.ID;
        if (buttonHandlerList.ContainsKey(id))
        {
            buttonHandlerList.Remove(id);
            buttonHandlerList.Add(id, btn);
            btn.Initialize(this);
            log += string.Format("RegistButton(id:{1},btn{2})\n移除重覆ButtonHandler\n{0}\n\n", Utilty.CallStack(), id, btn);
        }
        else
        {
            buttonHandlerList.Add(id, btn);
            btn.Initialize(this);
            log += string.Format("RegistButton(id:{1},btn{2})\n{0}\n\n", Utilty.CallStack(), id, btn);
        }
        if (onButtonRegisted != null)
            onButtonRegisted(this, new RegistButtonArgs(btn));
    }

    public void RemoveButtonHandler(int id)
    {
        var btn = buttonHandlerList[id];
        buttonHandlerList.Remove(id);

        if (btn != null)
        {
            if (onButtonRemoved != null)
                onButtonRemoved(this, new RemoveButtonArgs(btn));
            log += string.Format("RemoveButton(id:{1})\n移除成功\n{0}\n\n", Utilty.CallStack(), id);
        }
        else
        {
            log += string.Format("RemoveButton(id:{1})\n嘗試移除不存在的ButtonHandler\n{0}\n\n", Utilty.CallStack(), id);
        }
    }

    public void SetEnable(bool value)
    {
        if (isEnable != value)
        {
            _isEnable = value;

            if (isEnable)
            {
                if (onEnable != null)
                    onEnable(this, new EventArgs());
            }
            else
            {
                if (onDisable != null)
                    onDisable(this, new EventArgs());
            }
            log += string.Format("SetEnable( value:{1} )\n{0}\n\n", Utilty.CallStack(), value);
        }
        log += string.Format("SetEnable( value:{1} )\n但是value==isEnable\n{0}\n\n", Utilty.CallStack(), value);
    }

    public void TreggerButton(int id)
    {
        if (!isTerminate)
        {
            if (buttonHandlerList[id].isEnable)
            {
                if (onButtonTrigger != null)
                    onButtonTrigger(this, new TriggerButtonArgs(id));
                log += string.Format("Trigger( id:{1} )\n{0}\n\n", Utilty.CallStack(), id);
            }
        }
        log += string.Format("Trigger( id:{1} )\n但是Button.isEnable = false\n{0}\n\n", Utilty.CallStack(), id);
    }

    private void InitializeButtonList()
    {
        foreach (var keyValuePair in buttonHandlerList)
        {
            keyValuePair.Value.Initialize(this);
        }
    }

    public void Terminate()
    {
        _isTerminate = true;
        if (onTerminate != null)
            onTerminate(this, new EventArgs());
        log += string.Format("Terminate()\n{0}\n\n", Utilty.CallStack());
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
