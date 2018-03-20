using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPlatHandler
{
    event EventHandler<ShowHideArgs> onShow;
    event EventHandler<ShowHideArgs> onHide;
    event EventHandler<RegistButtonArgs> onButtonRegisted;
    event EventHandler<RemoveButtonArgs> onButtonRemoved;
    event EventHandler<TriggerButtonArgs> onButtonTrigger;
    event EventHandler onEnable;
    event EventHandler onDisable;
    event EventHandler onInitialize;
    event EventHandler onTerminate;

    IOrange orange { get; }
    int ID { get; }
    bool isShow { get; }
    bool isEnable { get; }
    bool isInitialize { get; }
    bool isTerminate { get; }
    int ButtonCount { get; }

    void Initialize(IOrange orange);
    void Show();
    void CompleteShow();
    void Hide();
    void CompleteHide();
    void SetEnable(bool value);
    void RegistButtonHandler(IButtonHandler btn);
    void RemoveButtonHandler(int id);
    bool HasButtonHandler(int id);
    void Terminate();
    IButtonHandler GetButton(int id);
    IButtonHandler[] GetAllButton();
    void TriggerButton(int id);
}
