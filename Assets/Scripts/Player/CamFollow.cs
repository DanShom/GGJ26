using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public GameObject target;
    public float horizontalSnapingStrangth = 5f;
    public float verticalSnapingStrangth = 10f;

    private Vector3 offset;
    private bool _isTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isTarget = target != null;
        if(_isTarget)
        {
            offset = Vector3.forward * (transform.position.z - target.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isTarget)
        {
            transform.position = new Vector3()
            { 
                x = Mathf.Lerp(transform.position.x, target.transform.position.x, Time.deltaTime *  horizontalSnapingStrangth),
                y = Mathf.Lerp(transform.position.y, target.transform.position.y, Time.deltaTime * verticalSnapingStrangth),
                z = offset.z
            };
            //transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime * horizontalSnapingStrangth);
            //transform.position = target.transform.position + offset;
        }
    }
}
