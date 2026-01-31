using UnityEngine;

public class spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f; // degrees per second

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
