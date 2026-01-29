[System.Serializable]
public class LongReference
{
    public bool useConstant = true;
    public long constantValue;
    public LongVariable variable;

    public long value
    {
        get
        {
            return useConstant ? constantValue : variable.value;
        }
        set
        {
            if (!useConstant) variable.value = value;
        }
    }
}
