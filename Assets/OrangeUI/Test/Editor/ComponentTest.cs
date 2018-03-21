using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ComponentTest
{
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [Test]
    public void OrangeWillInitialze_OnOrangeInitialize()
    {
        bool isBtnInit = false;
        bool isPlatInit = false;
        GameObject go = new GameObject();
        var btn = go.AddComponent<ButtonComponent>();
        btn.onInitialzie.AddListener(() => isBtnInit = true);
        go = new GameObject();
        var plat = go.AddComponent<PlatComponent>();
        plat.onInitialize.AddListener(() => isPlatInit = true);
        go = new GameObject();
        var orange = go.AddComponent<OrangeComponent>();
        orange.platList.Add(plat);
        plat.buttonList.Add(btn);

        orange.Initialize();

        Assert.IsTrue(orange.isInitialze);
        Assert.IsTrue(plat.isInitialize);
        Assert.IsTrue(btn.isInitialize);
        Assert.IsTrue(isBtnInit);
        Assert.IsTrue(isPlatInit);
    }

}
