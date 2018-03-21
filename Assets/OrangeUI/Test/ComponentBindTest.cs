using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ComponentBindTest {
    
    //[UnityTest]
    //public IEnumerator OrangeWillInitialze_AfterOrangeAwake()
    //{
    //    bool isBtnInit = false;
    //    bool isPlatInit = false;
    //    GameObject go = new GameObject();
    //    var btn = go.AddComponent<ButtonComponent>();
    //    btn.onInitialzie.AddListener(() => isBtnInit = true);
    //    go = new GameObject();
    //    var plat = go.AddComponent<PlatComponent>();
    //    plat.onInitialize.AddListener(() => isPlatInit = true);
    //    go = new GameObject();
    //    var orange = go.AddComponent<OrangeComponent>();
    //    orange.platList.Add(plat);
    //    plat.buttonList.Add(btn);

    //    orange.Initialize();
        
    //    Assert.IsTrue(orange.isInitialze);
    //    Assert.IsTrue(plat.isInitialize);
    //    Assert.IsTrue(btn.isInitialize);
    //    Assert.IsTrue(isBtnInit);
    //    Assert.IsTrue(isPlatInit);
    //}
}
