using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class ButtonHandlerTest
{

    [Test]
    public void Initialize_Check_Test()
    {
        ButtonHandler btn = new ButtonHandler(1);

        Assert.Throws<Exception>(() => btn.SetEnable(true));
        Assert.Throws<Exception>(() => btn.SetButtonState(ButtonState.DISABLE));
        Assert.Throws<Exception>(() => btn.Trigger());
    }

    [Test]
    public void Trigger_Test()
    {
        int btnID = 10;
        ButtonHandler btn = new ButtonHandler(btnID);
        PlatHandler plat = new PlatHandler(1);
        plat.Initialize(new PlatHandlerTest.DummyOrange());
        plat.RegistButtonHandler(btn);
        int isTrigger = -1;
        btn.onTrigger += (a, b) => {
            isTrigger = (a as ButtonHandler).ID;
        };

        plat.SetEnable(true);
        btn.SetEnable(true);
        btn.Trigger();

        try
        {
            Assert.AreEqual(btnID, isTrigger);
        }
        catch (Exception e)
        {
            Debug.Log(plat.GetLog());
            Debug.Log("====================");
            Debug.Log(btn.GetLog());
            throw;
        }

    }

}
