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
        //Chama método para que as regras sejam sempre atualizadas
        ApplyRules();
        //Move o peixe
        transform.Translate(0, 0, Time.deltaTime * speed); 
    }

    void ApplyRules()
    {
        //Cria array de game objects
        GameObject[] gos;
        //Adiciona os peixes no array de game object
        gos = myManager.allFish;

        //Variáveis de vetor para armazenar centro do game object
        Vector3 vcentre = Vector3.zero;
        //E um ponto vazio de distância entre 1 peixe e outro
        Vector3 vavoid = Vector3.zero;

        //speed do game object
        float gSpeed = 0.01f;
        //Float usada para ância entre um peixe e outro
        float nDistance;

        //Variável pra armazenar quantidade de peixes
        int groupSize = 0;

        //Para cada game object dentro do array de game objects
        foreach(GameObject go in gos)
        {
            //Se um gameobject for diferente do outro
            if (go != this.gameObject)
            {
                //Recebe a distância entre eles
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //Se a distância atual é menor (ou igual) do que a definida no manager
                if (nDistance <= myManager.neighbourDistance)
                {
                    //Adiciona a posição do game object na variável para localização futura
                    vcentre += go.transform.position;
                    //Aumenta o grupo em 1
                    groupSize++;

                    //Se a distância entre eles for menor do que 1
                    if (nDistance < 1.0f)
                    {
                        //Atualiza o valor da variável somando a posição do game object atual com a posição do alvo comparado
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //Recebe o componente flock do outro gameobject
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //Adiciona a velocidade do outro agente nesse agente
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }


        //Se o grupo for maior que 0
        if (groupSize > 0)
        {
            //Divide o centro do game object pela quantidade de game objects no array
            vcentre = vcentre / groupSize;
            //Divide a velocidade pela quantidade de game objects no array
            speed = gSpeed / groupSize;

            //Define a direção que será movida com base no vetor central + o vetor de direção para desvio - a posição atual
            Vector3 direction = (vcentre + vavoid) - transform.position;
            //Se a variável direction for diferente de 0
            if (direction != Vector3.zero)
            {
                //Move o peixe
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
