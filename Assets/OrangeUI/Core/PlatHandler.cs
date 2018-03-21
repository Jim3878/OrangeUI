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
    public int ButtonCount
    {
        get
        {
            LifeCheck();
            return this.buttonHandlerList.Count;
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

    private Dictionary<int, IButtonHandler> buttonHandlerList;

    public PlatHandler(int ID)
    {
        this._ID = ID;
        _isInitialize = false;
        _isTerminate = false;
        log += string.Format("new()\n{0}\n\n", Utilty.CallStack());
    }

    public void CompleteHide()
    {
        LifeCheck();
        _isShow = false;
        if (onHide != null)
            onHide(this, new ShowHideArgs(true));
        log += string.Format("CompleteHide()\n{0}\n\n", Utilty.CallStack());

    }

    public void CompleteShow()
    {
        LifeCheck();

        _isShow = true;
        if (onShow != null)
            onShow(this, new ShowHideArgs(true));
        log += string.Format("CompleteShow()\n{0}\n\n", Utilty.CallStack());

    }

    public IButtonHandler[] GetAllButton()
    {
        LifeCheck();
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
        LifeCheck();
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
        LifeCheck();
        if (isEnable && isShow)
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
        LifeCheck();
        if (isEnable && !isShow)
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
            buttonHandlerList = new Dictionary<int, IButtonHandler>();
            _isInitialize = true;
            this._orange = orange;
            _isEnable = false;
            CompleteHide();
            if (onInitialize != null)
                onInitialize(this, new EventArgs());

            log += string.Format("Initialize()\n{0}\n\n", Utilty.CallStack());
        }
    }

    public void Initialize(IOrange orange, params IButtonHandler[] btns)
    {
        if (!isInitialize)
        {
            buttonHandlerList = new Dictionary<int, IButtonHandler>();
            this._orange = orange;
            _isInitialize = true;
            InitializeButton(btns);

            SetEnable(false);
            if (onInitialize != null)
                onInitialize(this, new EventArgs());

            log += string.Format("Initialize()\n{0}\n\n", Utilty.CallStack());
        }
    }

    private void InitializeButton(IButtonHandler[] btns)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            RegistButtonHandler(btns[i]);
        }
    }

    public void RegistButtonHandler(IButtonHandler btn)
    {
        LifeCheck();
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
        LifeCheck();
        var btn = buttonHandlerList[id];
        buttonHandlerList.Remove(id);

        if (btn != null)
        {
            btn.Terminate();
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
        LifeCheck();
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
        else
        {
            log += string.Format("SetEnable( value:{1} )\n但是value==isEnable\n{0}\n\n", Utilty.CallStack(), value);
        }
    }

    public void TriggerButton(int id)
    {
        LifeCheck();
        if (buttonHandlerList[id].isEnable)
        {
            if (onButtonTrigger != null)
                onButtonTrigger(this, new TriggerButtonArgs(id));
            log += string.Format("Trigger( id:{1} )\n{0}\n\n", Utilty.CallStack(), id);
        }
    }

    public void LifeCheck()
    {
        if (isTerminate)
        {
            throw new Exception("介面已經終止");
        }
        if (!isInitialize)
        {
            throw new Exception("還末初始化");
        }
    }

    public void Terminate()
    {
        if (isTerminate)
        {
            throw new Exception("介面已終止");
        }
        _isTerminate = true;
        IButtonHandler[] bhArr = new ButtonHandler[buttonHandlerList.Count];
        int index = 0;
        foreach (var btn in buttonHandlerList)
        {
            bhArr[index] = btn.Value;
            index++;
        }
        for(int i = 0; i < bhArr.Length; i++)
        {
            bhArr[i].Terminate();
        }

        if (!orange.isTerminate && orange.hasPlatHandler(ID))
        {
            orange.RemovePlatHandler(ID);
        }
        if (onTerminate != null)
            onTerminate(this, new EventArgs());
        log += string.Format("Terminate()\n{0}\n\n", Utilty.CallStack());
    }

    public bool HasButtonHandler(int id)
    {
        LifeCheck();
        return buttonHandlerList.ContainsKey(id);
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
