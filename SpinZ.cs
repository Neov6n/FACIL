using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinZ : MonoBehaviour
{
    public bool spin = true;
    public float speed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (spin)
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.eulerAngles.z + speed);
        }
    }
}
