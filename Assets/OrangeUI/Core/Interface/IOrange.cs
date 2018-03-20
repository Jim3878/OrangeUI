using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrange {

    bool isInitialize { get; }
    bool isTerminate { get; }
    int count { get; }

    void Initialize();
    void Terminate();
    void RegistPlatHandler( IPlatHandler platHandler);
    void RemovePlatHandler(int id);
    bool hasPlatHandler(int id);
    IPlatHandler GetPlatHandler(int id);
    IPlatHandler[] GetAllPlatHandler();
}
