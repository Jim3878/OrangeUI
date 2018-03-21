using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class OrangeBindTest {

    [Test]
    public void OrangerIsTerminate_AfterTerminate_IsTerminateWillTransToTrue()
    {
        OrangeManager om = new OrangeManager();
        om.Initialize();
        Assert.IsFalse(om.isTerminate);

        om.Terminate();

        Assert.IsTrue(om.isTerminate);
    }

	[Test]
	public void OrangeTerminate_CallTerminate_AllPlatHandlerWillTerminateToo() {
        // Use the Assert class to test conditions.

        OrangeManager om = new OrangeManager();
        int platHandlerCount = 10;
        PlatHandler[] ph = new PlatHandler[platHandlerCount];
        om.Initialize();
        for(int i = 0; i < platHandlerCount; i++)
        {
            ph[i] = new PlatHandler(i);
            om.RegistPlatHandler(ph[i]);
        }

        om.Terminate();

        Assert.IsTrue(om.isTerminate);
        for (int i = 0; i < platHandlerCount; i++)
        {
            Assert.IsTrue(ph[i].isTerminate);   
        }
    }

    [Test]
    public void PlatHandlerTerminate_CallTerminate_WillRemoveFromOrangeAndTerminateAllButton()
    {
        int btnCount = 10;
        int platID = 1;
        OrangeManager om = new OrangeManager();
        PlatHandler ph = new PlatHandler(platID);
        ButtonHandler[] bh = new ButtonHandler[btnCount];
        om.Initialize();
        om.RegistPlatHandler(ph);
        for(int i = 0; i < btnCount; i++)
        {//建立按鈕並注冊
            bh[i] = new ButtonHandler(i);
            ph.RegistButtonHandler(bh[i]);
        }

        ph.Terminate();//銷毀介面

        //確認介面及其下的按鈕是否銷毀，以及是否從橘子反注冊
        Assert.IsTrue(ph.isTerminate);
        Assert.IsFalse(om.hasPlatHandler(platID));
        for(int i = 0; i < btnCount; i++)
        {
            Assert.IsTrue(bh[i].isTerminated);
        }
    }

    [Test]
    public void PlatHandlerOnInitialize_RegistButtonByOnInitializeEventHandler_ButtonWillInitializeAndFoundFromPlatHandler()
    {
        int btnCount = 10;
        int platID = 1;
        OrangeManager om = new OrangeManager();
        PlatHandler ph = new PlatHandler(platID);
        ButtonHandler[] bh = new ButtonHandler[btnCount];
        for (int i = 0; i < btnCount; i++)
        {//建立按鈕
            bh[i] = new ButtonHandler(i);
        }
        ph.onInitialize += (a, b) =>
        {//當介面初始化後註冊按鈕
            for (int i = 0; i < btnCount; i++)
            {
                ph.RegistButtonHandler(bh[i]);
            }
        };

        om.Initialize();
        om.RegistPlatHandler(ph);

        for(int i = 0; i < btnCount; i++)
        {
            ph.HasButtonHandler(i);
        }
    }

    [Test]
    public void ButtonHandlerTerminate_CallTerminate_WillRemoveFromPlatHandler()
    {
        int bhID = 10;
        ButtonHandler bh = new ButtonHandler(bhID);
        PlatHandler ph = new PlatHandler(1);
        ph.Initialize(new PlatHandlerTest.DummyOrange());
        ph.RegistButtonHandler(bh);

        bh.Terminate();

        Assert.IsTrue(bh.isTerminated);
        Assert.IsFalse(ph.HasButtonHandler(bhID));
    }


}
