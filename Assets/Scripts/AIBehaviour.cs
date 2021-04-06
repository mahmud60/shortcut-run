using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{

    int collectedWoodenPlank = 0;
    int count = 0;

    public GameObject plank;
    public Transform collectedPlankPos;
    public GameObject woodenPlank;

    List<GameObject> planks;

    AIPathFollower aiPathFollower;

    RaycastHit hit;

    private bool isGrounded;
    private bool spawn;
    private bool spawnOnce = false;

    Animator animator;

    void Start()
    {
        planks = new List<GameObject>();
        aiPathFollower = transform.parent.GetComponent<AIPathFollower>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, 2.5f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            if (hit.transform.tag == "Path")
            {
                isGrounded = true;
                spawn = false;
                aiPathFollower.speed = 7;
            }
            else if (hit.transform.tag == "Water")
            {
                if (collectedWoodenPlank != 0)
                {
                    spawn = true;
                    spawnOnce = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
            else if (hit.transform.tag == "Wood")
            {
                isGrounded = true;
                spawn = false;
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        animator.SetBool("Grounded", isGrounded);


        if (spawn && spawnOnce)
            spawnPlank();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wood")
        {
            collectedWoodenPlank += 7;
            other.gameObject.SetActive(false);
            GameObject spawnedPlank = Instantiate(plank, new Vector3(collectedPlankPos.position.x, collectedPlankPos.position.y + (count * 0.1f), collectedPlankPos.position.z), plank.transform.rotation) as GameObject;
            spawnedPlank.transform.parent = this.transform;
            spawnedPlank.transform.localRotation = Quaternion.Euler(0, 90, 0);
            planks.Add(spawnedPlank);
            count++;
            if (aiPathFollower.speed > 5)
                aiPathFollower.speed -= 0.25f;
            //Debug.Log(collectedWoodenPlank);
        }

        if (other.gameObject.tag == "FinishLine")
        {
            aiPathFollower.gameOver = true;
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    void spawnPlank()
    {
        if (collectedWoodenPlank > 0)
        {
            if (collectedWoodenPlank % 7 == 0)
            {
                planks[count - 1].SetActive(false);
                planks.RemoveAt(count - 1);
                count--;
            }
            Instantiate(woodenPlank, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            collectedWoodenPlank--;
            spawnOnce = false;
            if (aiPathFollower.speed < 10)
                aiPathFollower.speed += 0.25f;
            collectedWoodenPlank--;
        }
    }
}
