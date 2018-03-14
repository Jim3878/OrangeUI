using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrange {

    void RegistPlatHandler(int id, IPlatHandler platHandler);
    void RemovePlatHandler(int id);
    IPlatHandler GetPlatHandler(int id);
    IPlatHandler[] GetAllPlatHandler();
}
