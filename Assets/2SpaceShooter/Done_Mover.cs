using UnityEngine;

public class Done_Mover : MonoBehaviour
{
    public float speed;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}