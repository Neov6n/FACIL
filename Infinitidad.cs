using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinitidad : MonoBehaviour
{
    public newPlayerMovementScript jugadora;
    public Transform posicion;

    // Start is called before the first frame update
    void Start()
    {
        jugadora = FindObjectOfType<newPlayerMovementScript>();
        posicion = jugadora.transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(posicion.position.x, gameObject.transform.position.y, posicion.position.z);
    }
}
