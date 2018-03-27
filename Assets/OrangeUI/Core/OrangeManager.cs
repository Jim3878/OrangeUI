using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeManager : IOrange
{
    Dictionary<int, IPlatHandler> platHandlerList;
    private bool _isInitialize;
    public bool isInitialize
    {
        get
        {
            return _isInitialize;
        }
    }
    public bool _isTerminate;
    public bool isTerminate
    {
        get
        {
            return _isTerminate;
        }
    }
    public int count
    {
        get
        {
            return platHandlerList.Count;
        }
    }

    public OrangeManager()
    {
        _isInitialize = false;
        _isTerminate = false;
    }

    public void Initialize()
    {
        if (!isInitialize)
        {
            _isInitialize = true;
            platHandlerList = new Dictionary<int, IPlatHandler>();
        }
    }

    public IPlatHandler[] GetAllPlatHandler()
    {
        IPlatHandler[] result = new IPlatHandler[platHandlerList.Count];
        int index = 0;
        foreach (var plat in platHandlerList)
        {
            if (hasPlatHandler(plat.Key))
            {
                result[index] = plat.Value;
                index++;
            }
        }
        return result;
    }

    public IPlatHandler GetPlatHandler(int id)
    {
        if (!hasPlatHandler(id))
        {
            throw new KeyNotFoundException("找不到你要的plat唷\nID = " + id);
        }
        return platHandlerList[id];
    }

    public bool hasPlatHandler(int id)
    {
        if (platHandlerList.ContainsKey(id))
        {
            if (platHandlerList[id].isTerminate)
            {
                platHandlerList.Remove(id);
                return false;
            }
            return true;
        }
        return false;
    }

    public void RegistPlatHandler(IPlatHandler platHandler)
    {
        int id = platHandler.ID;
        if (hasPlatHandler(id))
        {
            RemovePlatHandler(id);
            Debug.Log("[Orange]覆蓋已存在之PlatHandler ID = " + platHandler.ID);
        }
        platHandlerList.Add(platHandler.ID, platHandler);
        platHandler.Initialize(this);

    }

    public void RegistPlatHandler(IPlatHandler platHandler,params IButtonHandler[] btnHandlers)
    {
        int id = platHandler.ID;
        if (hasPlatHandler(id))
        {
            RemovePlatHandler(id);
            Debug.Log("[Orange]覆蓋已存在之PlatHandler ID = " + platHandler.ID);
        }
        platHandlerList.Add(platHandler.ID, platHandler);
        platHandler.Initialize(this,btnHandlers);

    }

    public void RemovePlatHandler(int id)
    {
        if (!hasPlatHandler(id))
        {
            throw new KeyNotFoundException("企圖移除不存在的PlatHandler");
        }
        IPlatHandler plat = platHandlerList[id];
        platHandlerList.Remove(id);
        if (!plat.isTerminate)
            plat.Terminate();
    }

    private void LifeCheck()
    {
        if (isTerminate)
            throw new Exception("橘子已終止");
        if (!isInitialize)
            throw new Exception("橘子還沒初始化");

    }

    public void Terminate()
    {
        if (isTerminate)
            throw new Exception("橘子已終止");
        if (!isTerminate)
        {
            _isTerminate = true;
            IPlatHandler[] phArr = new PlatHandler[platHandlerList.Count];
            int index = 0;
            foreach (var plat in platHandlerList)
            {
                phArr[index] = plat.Value;
                index++;
            }
            for (int i = 0; i < phArr.Length; i++)
            {
                phArr[i].Terminate();
            }
        }
    }
}
