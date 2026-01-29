
using UnityEngine;

[CreateAssetMenu(fileName = "LongVariable", menuName = "Variables/Long Variable")]
public class LongVariable : ScriptableObject
{
    public long value;

    public void Set(long vl) { value = vl; }
    public void Set(int vl) { value = vl; }
    public void Append(long vl) { value += vl; }
    public void Append(int vl) { value += vl; }
}
