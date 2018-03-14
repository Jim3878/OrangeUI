using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class PlatHandlerTest
{
    public class DummyOrange : IOrange
    {
        public IPlatHandler[] GetAllPlatHandler()
        {
            throw new NotImplementedException();
        }

        public IPlatHandler GetPlatHandler(int id)
        {
            throw new NotImplementedException();
        }

        public void RegistPlatHandler(int id, IPlatHandler platHandler)
        {
            throw new NotImplementedException();
        }

        public void RemovePlatHandler(int id)
        {
            throw new NotImplementedException();
        }
    }
    public class DummyButton : IButtonHandler
    {
        public IPlatHandler platHandler { get { return null; } }

        public int ID { get { return -1; } }

        public bool isEnable { get { return false; } }

        public bool isInitialize { get { return false; } }

        public ButtonState currentState { get { return ButtonState.NORMAL; } }

        public bool isTerminated { get { return false; } }

        public event EventHandler onEnable;
        public event EventHandler onDisable;
        public event EventHandler<ButtonStateArgs> onButtonStateChange;
        public event EventHandler onInitialize;
        public event EventHandler onTerminated;

        public int initizlie = 0;
        public void Initialize(IPlatHandler handler)
        {
            initizlie++;
        }

        public ButtonState state;
        public void SetButtonState(ButtonState state)
        {
            this.state = state;
        }

        public int setEnable = 0;
        public void SetEnable(bool value)
        {
            setEnable++;
        }

        public int terminated = 0;
        public void Terminated()
        {
            terminated++;
        }
    }

    public class RegistRemoveButtonHandlerTest
    {
        [Test]
        public void RegistButtonHandler_BeforeInitialize_ButtonWillInitializeTwice()
        {
            PlatHandler plat = new PlatHandler(0);
            DummyButton btn = new DummyButton();

            plat.RegistButtonHandler(btn);
            plat.Initialize(new DummyOrange());

            Assert.AreEqual(2, btn.initizlie);
        }

        [Test]
        public void RegistButtonHandler_AfterInitialize_ButtonWillInitialize()
        {
            var plat = GetPlatHandler();
            DummyButton btn = new DummyButton();

            plat.Initialize(new DummyOrange());
            plat.RegistButtonHandler(btn);

            Assert.AreEqual(1, btn.initizlie);
        }
    }
    
    public class ShowHideTest
    {
        private int showCount;
        private int hideCount;

        [Test]
        public void ShowHide_AfterTerminated_WillNotShow()
        {
            var plat = GetShowHideTestPlatHandler();
            plat.Initialize(GetDummyOrange());

            plat.Terminate();
            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();

            try
            {
                ShowHideCount_Equals(0, 1);
            }
            catch (Exception)
            {
                Debug.Log(plat.GetLog());
                throw;
            }
        }

        [Test]
        public void ShowHide_BeforeInitialize_WillNotShow()
        {
            var plat = GetShowHideTestPlatHandler();

            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();
            plat.Initialize(GetDummyOrange());

            ShowHideCount_Equals(0, 1);

        }

        [Test]
        public void ShowHide_OnDisable_WillNotShow()
        {
            var plat = GetShowHideTestPlatHandler();
            plat.Initialize(GetDummyOrange());

            plat.SetEnable(false);
            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();
            plat.Hide();
            plat.Show();

            ShowHideCount_Equals(0, 1);
        }

        [Test]
        public void CompleteShowHide_AfterTerminated_WillNotShowHide()
        {
            var plat = GetShowHideTestPlatHandler();
            plat.Initialize(GetDummyOrange());

            plat.Terminate();
            plat.CompleteHide();
            plat.CompleteShow();
            plat.CompleteHide();
            plat.CompleteShow();
            plat.CompleteHide();
            plat.CompleteShow();

            ShowHideCount_Equals(0, 1);
        }

        public void ShowHideCount_Equals(int showCount = 0, int hideCount = 0)
        {
            Assert.AreEqual(showCount, this.showCount);
            Assert.AreEqual(hideCount, this.hideCount);
        }
        PlatHandler GetShowHideTestPlatHandler()
        {
            var plat = GetPlatHandler();
            showCount = 0;
            hideCount = 0;
            plat.onHide += (a, b) => hideCount++;
            plat.onShow += (a, b) => showCount++;
            return plat;
        }


    }

    static PlatHandler GetPlatHandler()
    {
        return new PlatHandler(0);
    }
    
    static IOrange GetDummyOrange()
    {
        return new DummyOrange();
    }
}
