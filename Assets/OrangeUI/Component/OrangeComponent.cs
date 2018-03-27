using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeComponent : MonoBehaviour
{

    public List<PlatComponent> platList = new List<PlatComponent>();
    private OrangeManager _orange;
    public OrangeManager orange
    {
        get
        {
            return _orange;
        }
    }
    public bool isInitialze
    {
        get
        {
            return _orange != null;
        }
    }

    public void Initialize()
    {
        if (!isInitialze)
        {
            _orange = new OrangeManager();
            _orange.Initialize();
            foreach (var plat in platList)
            {
                _orange.RegistPlatHandler(plat.handler,plat.GetStaticButtonArray());
                Debug.Log("[Orange]Initilize plat " + plat.gameObject.name);
            }
        }
    }
}
