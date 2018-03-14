using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IButtonHandler  {

    event EventHandler onEnable;
    event EventHandler onDisable;
    event EventHandler<ButtonStateArgs> onButtonStateChange;
    event EventHandler onInitialize;
    event EventHandler onTerminated;

    IPlatHandler platHandler { get; }
    int ID { get; }
    bool isEnable { get; }
    bool isInitialize { get; }
    bool isTerminated { get; }
    ButtonState currentState { get; }

    void Initialize(IPlatHandler platHandler);
    void SetButtonState(ButtonState state);
    void SetEnable(bool value);
    void Terminated();
	
}
