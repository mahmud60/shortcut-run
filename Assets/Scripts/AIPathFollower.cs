using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class AIPathFollower : MonoBehaviour
{
    public PathCreator pathCreator;

    public float speed = 8;

    float distanceTravelled;

    [SerializeField] private Animator animator = null;

    public bool gameOver = false;

    public GameManager gameManager;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }


    private void FixedUpdate()
    {
        if (!gameOver && gameManager.gameStart)
        {
            animator.SetBool("Grounded", true);
            animator.SetFloat("MoveSpeed", 1);
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }

    }
}
