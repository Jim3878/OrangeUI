using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShowHideArgs : EventArgs
{
    public bool isComplete;
    public ShowHideArgs(bool isComplete = false)
    {
        this.isComplete = isComplete;
    }
}

public class ButtonStateArgs : EventArgs
{
    public ButtonState state;
    public ButtonStateArgs(ButtonState state)
    {
        this.state = state;
    }
}

public class TriggerButtonArgs : EventArgs
{
    public int ID;
    public TriggerButtonArgs(int ID)
    {
        this.ID = ID;
    }
}

public class RegistButtonArgs : EventArgs
{
    public IButtonHandler buttonHandler;
    public RegistButtonArgs(IButtonHandler buttonHandler)
    {
        this.buttonHandler = buttonHandler;
    }
}

public class RemoveButtonArgs : EventArgs
{
    public IButtonHandler buttonHandler;
    public RemoveButtonArgs(IButtonHandler buttonHandler)
    {
        this.buttonHandler = buttonHandler;
    }
}