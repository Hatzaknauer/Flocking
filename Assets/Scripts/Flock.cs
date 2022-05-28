using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    float speed;

    void Start()
    {
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
}
