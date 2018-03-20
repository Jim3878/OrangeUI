﻿using System;
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

    public IPlatHandler[] GetAllPlatHandler()
    {
        IPlatHandler[] result = new IPlatHandler[platHandlerList.Count];
        int index = 0;
        foreach (var plat in platHandlerList)
        {
            result[index] = plat.Value;
            index++;
        }
        return result;
    }

    public IPlatHandler GetPlatHandler(int id)
    {
        if (!platHandlerList.ContainsKey(id))
        {
            throw new KeyNotFoundException("找不到你要的plat唷\nID = " + id);
        }
        return platHandlerList[id];
    }

    public bool hasPlatHandler(int id)
    {
        return platHandlerList.ContainsKey(id);
    }

    public void RegistPlatHandler(IPlatHandler platHandler)
    {
        int id = platHandler.ID;
        if (platHandlerList.ContainsKey(id))
        {
            platHandlerList.Remove(id);
            Debug.Log("[Orange]覆蓋已存在之PlatHandler ID = " + platHandler.ID);
        }
        platHandler.Initialize(this);

    }

    public void RemovePlatHandler(int id)
    {
        if (!platHandlerList.ContainsKey(id))
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

    public void Initialize()
    {
        if (!isInitialize)
        {
            _isInitialize = true;
        }
    }

    public void Terminate()
    {
        if (!isTerminate)
        {
            _isTerminate = true;
            foreach (var plat in platHandlerList)
            {
                plat.Value.Terminate();
            }
        }
    }
}