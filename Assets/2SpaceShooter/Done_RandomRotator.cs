using UnityEngine;

public class Done_RandomRotator : MonoBehaviour
{
    public float tumble;

    private void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}