using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float turnSpeed = 200;
    [SerializeField] private float jumpForce = 4;

    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody rigidBody = null;

    private bool isGrounded;

    float horizontal = 0;

    int collectedWoodenPlank = 0;
    bool spawn;

    public GameObject woodenPlank;
    public Transform woodenPlankPosition;
    int count = 0;

    public float rotatespeed = 10f;
    private float _startingPosition;

    RaycastHit hit;

    bool gameOver = false;

    public GameManager gameManager;

    public Transform collectedPlankPos;
    public GameObject plank;

    List<GameObject> planks;

    bool spawnOnce = false;

    bool canJump = true;

    float tempSpeed;

    float left = 0;
    float right = 0;

    private void Awake()
    {
        if (!animator) { gameObject.GetComponent<Animator>(); }
        if (!rigidBody) { gameObject.GetComponent<Animator>(); }

    }

    void Start()
    {
        planks = new List<GameObject>();
        tempSpeed = moveSpeed;
        animator.SetBool("Grounded", isGrounded);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startingPosition = touch.position.x;
                    break;
                case TouchPhase.Moved:

                    if (_startingPosition > touch.position.x)
                    {
                        if (_startingPosition - touch.position.x > 2.5f)
                        {
                            DOTween.To(() => left, x => left = x, -1, 0.5f);
                            transform.Rotate(0, turnSpeed * left * Time.deltaTime, 0);
                        }
                    }
                    else if (_startingPosition < touch.position.x)
                    {
                        if (touch.position.x - _startingPosition > 2.5f)
                        {
                            DOTween.To(() => right, x => right = x, 1, 0.5f);
                            transform.Rotate(0, turnSpeed * right * Time.deltaTime, 0);
                        }
                    }
                    _startingPosition = touch.position.x;
                    break;

                case TouchPhase.Ended:
                    Debug.Log("Touch Phase Ended.");
                    left = 0;
                    right = 0;
                    break;
            }
        }
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
                moveSpeed = tempSpeed;
            }
            else if (hit.transform.tag == "Water")
            {
                if (collectedWoodenPlank != 0)
                {
                    spawn = true;
                    spawnOnce = true;
                    canJump = true;
                }
                else if (canJump)
                {
                    jump();
                    canJump = false;
                }
                else
                {
                    //isGrounded = false;
                    //gameOver = true;
                    //gameManager.gameOver = true;
                }
            }
            else if (hit.transform.tag == "Wood")
            {
                isGrounded = true;
                spawn = false;
                moveSpeed = tempSpeed;
            }
            //Debug.Log(hit.transform.tag);
        }
        else
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        animator.SetBool("Grounded", isGrounded);

        if (!gameOver && gameManager.gameStart)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            horizontal = Input.GetAxis("Horizontal");

            transform.Rotate(0, turnSpeed * horizontal * Time.deltaTime, 0);

            animator.SetFloat("MoveSpeed", 1);
        }

        if (spawn && spawnOnce)
            spawnPlank();
    }

    void jump()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 1, 1) * 5f, ForceMode.Impulse);
        moveSpeed = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            gameOver = true;
            gameManager.gameOver = true;
            gameManager.PlayerDied();
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Path")
        {
            isGrounded = true;
            spawn = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collectedWoodenPlank == 0)
            isGrounded = false;
        else
        {
            spawn = true;
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wood")
        {
            collectedWoodenPlank += 5;
            other.gameObject.SetActive(false);
            GameObject spawnedPlank = Instantiate(plank, new Vector3(collectedPlankPos.position.x, collectedPlankPos.position.y+(count*0.1f), collectedPlankPos.position.z), plank.transform.rotation) as GameObject;
            spawnedPlank.transform.parent = this.transform;
            spawnedPlank.transform.localRotation = Quaternion.Euler(0, 90, 0);
            planks.Add(spawnedPlank);
            count++;
            if(moveSpeed>5)
                moveSpeed -= 0.25f;
            //Debug.Log(collectedWoodenPlank);
        }

        if (other.gameObject.tag == "FinishLine")
        {
            gameOver = true;
            gameManager.gameOver = true;
            gameManager.gameFinished();
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    void spawnPlank()
    {
        if (collectedWoodenPlank > 0)
        {
            if(collectedWoodenPlank%5==0)
            {
                planks[count - 1].SetActive(false);
                planks.RemoveAt(count-1);
                count--;
                if (moveSpeed < 8)
                    moveSpeed += 0.25f;
            }
            Instantiate(woodenPlank, new Vector3(transform.position.x,transform.position.y, transform.position.z), transform.rotation);
            collectedWoodenPlank--;
            spawnOnce = false;
        }
    }
}
