
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variables/Int Variable")]
public class IntVariable : ScriptableObject
{
    public int value;

    public void Set(int vl) { value = vl; }
    public void Append(int vl) { value += vl; }
}
