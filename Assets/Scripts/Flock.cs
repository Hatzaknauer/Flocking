using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    float speed;
    bool turning = false;

    void Start()
    {
        //Inicia cada peixe com uma velocidade aleat�ria
        speed = Random.Range(myManager.minSpeed,
            myManager.maxSpeed);    
    }

    void Update()
    {
        //Define o terreno limite do agente
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        //Declara raycast
        RaycastHit hit = new RaycastHit();

        //Recebe a dire��o a ser movida com base na posi��o do obst�culo subtraindo a posi��o do agente
        Vector3 direction = myManager.transform.position - transform.position;

        //Caso o ponto esteja fora dos limites
        if (!b.Contains(transform.position))
        {
            //Atualiza a vari�vel booleana fazendo curva para verdadeiro
            turning = true;
            //Atualiza a dire��o do vetor de dire��o para voltar para os limites
            direction = myManager.transform.position - transform.position;
        }
        //Caso o Raycast colida
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            //Atualiza a vari�vel booleana fazendo curva para verdadeiro
            turning = true;
            //Atualiza a dire��o do vetor de dire��o para a dire��o refletida do vetor
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else 
        { 
            //N�o est� virando
            turning = false;
        }

        //Caso esteja virando
        if (turning)
        {
            //Move o agente com o comando que faz curva
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                Quaternion.LookRotation(direction),
                                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //Caso o n�mero gerado seja menor que 10
            if (Random.Range(0, 100) < 10)
                //Define nova velocidade aleat�ria
                speed = Random.Range(myManager.minSpeed,
                                    myManager.maxSpeed);
            //Caso seja um n�mero menor que 20
            if (Random.Range(0, 100) < 20)
                //Chama m�todo para aplicar regras
                ApplyRules();
        }
        //Define o eixo z para ser movido com base na velocidade
        transform.Translate(0, 0, Time.deltaTime * speed);
    }


    void ApplyRules()
    {
        //Cria array de game objects
        GameObject[] gos;
        //Adiciona os peixes no array de game object
        gos = myManager.allFish;

        //Vari�veis de vetor para armazenar centro do game object
        Vector3 vcentre = Vector3.zero;
        //E um ponto vazio de dist�ncia entre 1 peixe e outro
        Vector3 vavoid = Vector3.zero;

        //speed do game object
        float gSpeed = 0.01f;
        //Float usada para �ncia entre um peixe e outro
        float nDistance;

        //Vari�vel pra armazenar quantidade de peixes
        int groupSize = 0;

        //Para cada game object dentro do array de game objects
        foreach(GameObject go in gos)
        {
            //Se um gameobject for diferente do outro
            if (go != this.gameObject)
            {
                //Recebe a dist�ncia entre eles
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //Se a dist�ncia atual � menor (ou igual) do que a definida no manager
                if (nDistance <= myManager.neighbourDistance)
                {
                    //Adiciona a posi��o do game object na vari�vel para localiza��o futura
                    vcentre += go.transform.position;
                    //Aumenta o grupo em 1
                    groupSize++;

                    //Se a dist�ncia entre eles for menor do que 1
                    if (nDistance < 1.0f)
                    {
                        //Atualiza o valor da vari�vel somando a posi��o do game object atual com a posi��o do alvo comparado
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
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            //Divide a velocidade pela quantidade de game objects no array
            speed = gSpeed / groupSize;

            //Define a dire��o que ser� movida com base no vetor central + o vetor de dire��o para desvio - a posi��o atual
            Vector3 direction = (vcentre + vavoid) - transform.position;
            //Se a vari�vel direction for diferente de 0
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