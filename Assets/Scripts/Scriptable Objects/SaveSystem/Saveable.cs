using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Saveable
{
    void SaveData(ref GameData data);
    void LoadData(GameData data);
}
