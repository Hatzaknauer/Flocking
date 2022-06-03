using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);

    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    void Start()
    {
        //Aloca peixes num game object
        allFish = new GameObject[numFish];

        //Enquanto não estiver no limite de peixes
        for (int i =0; i < numFish; i++)
        {
            //Define até onde os peixes podem ser instanciados
            Vector3 pos = this.transform.position +
                new Vector3(
                Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y),
                Random.Range(-swinLimits.z, swinLimits.z));
            //Instancia novos peixes
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);

            //Adiciona à todos os peixes esse objeto como o manager
            allFish[i].GetComponent<Flock>().myManager = this;
        }
    }

}
