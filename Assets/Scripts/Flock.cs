using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    float speed;

    void Start()
    {
        //Inicia cada peixe com uma velocidade aleatória
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);    
    }

    void Update()
    {
        //Move o peixe
        transform.Translate(0, 0, Time.deltaTime * speed); 
    }
}
