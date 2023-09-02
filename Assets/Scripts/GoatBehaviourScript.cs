using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatBehaviourScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotSpeed = 45f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private bool isWalkingtofood = false;
    private bool isRotatetofood = false;

    private GameObject[] foods;
    public Transform consumable;
    public bool foodcontact;
    public float lineofSightforFood = 10;

    // Start is called before the first frame update

    void Start()
    {
        consumable = null;
        foodcontact = false;
    }



    // Update is called once per frame

    void Update()

    {

        if (isWandering == false)

        {

            StartCoroutine(Wander());

        }

        if (isRotatingRight == true)

        {

            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);

        }

        if (isRotatingLeft == true)

        {

            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);

        }

        if (isWalking == true)

        {

            transform.position += transform.rotation * new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

        }

        if (isRotatetofood == true)

        {

            Vector3 getclosestfoodp = getclosestfood().position;

            var lookPos = getclosestfoodp - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(0, -90, 0); // this adds a 90 degrees Y rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
        }

        if (isWalkingtofood == true)

        {
            Vector3 getclosestfoodp = getclosestfood().position;

            if (Vector3.Distance(transform.position, getclosestfoodp) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, getclosestfoodp, moveSpeed * Time.deltaTime);
            }

            else
            {
                Destroy(getclosestfoodobj());
            }

        }

    }

    public Transform getclosestfood()
    {
        if(getclosestfoodobj()!=null) return getclosestfoodobj().transform;
        else return null;
    }

    public GameObject getclosestfoodobj()
    {
        foods = GameObject.FindGameObjectsWithTag("grass");
        float closestdistance = Mathf.Infinity;
        GameObject trans = null;
        foreach (GameObject food in foods)
        {
            float currentdistance = Vector3.Distance(transform.position, food.transform.position);
            if (currentdistance < closestdistance)
            {
                closestdistance = currentdistance;
                trans = food;
            }
        }
        return trans;
    }

    public bool isFoodNear()
    {
        bool isfood = false;
        if (getclosestfood() != null)
        {
            if (Vector3.Distance(transform.position, getclosestfood().position) <= lineofSightforFood)
            {
                isfood = true;
            }
        }
        return isfood;
    }

    IEnumerator Wander()

    {

        int rotTime = Random.Range(1, 3);

        int rotateWait = Random.Range(1, 4);

        int rotateLorR = Random.Range(1, 2);

        int walkWait = Random.Range(1, 4);

        int walkTime = Random.Range(1, 2);



        isWandering = true;

        if(isFoodNear())
        {
            yield return new WaitForSeconds(rotateWait);

            isRotatetofood = true;

            yield return new WaitForSeconds(rotTime);

            isRotatetofood = false;

            yield return new WaitForSeconds(walkWait);

            isWalkingtofood = true;

            yield return new WaitForSeconds(walkTime);

            isWalkingtofood = false;
        }

        else
        {

            yield return new WaitForSeconds(walkWait);

            isWalking = true;

            yield return new WaitForSeconds(walkTime);

            isWalking = false;

            yield return new WaitForSeconds(rotateWait);

            if (rotateLorR == 1)

            {

                isRotatingRight = true;

                yield return new WaitForSeconds(rotTime);

                isRotatingRight = false;

            }

            if (rotateLorR == 2)

            {

                isRotatingLeft = true;

                yield return new WaitForSeconds(rotTime);

                isRotatingLeft = false;

            }
        }

        isWandering = false;
    }
}
