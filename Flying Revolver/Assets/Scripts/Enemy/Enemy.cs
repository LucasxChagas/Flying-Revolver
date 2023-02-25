using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] Animator anim;

    [Header("AI Settings")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] targetPoints;

    [HideInInspector] public Transform currentTarget;
    int targetPointIndex;


    bool callUpdateDestination = false;

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        UpdateDestination();
    }

    void Update()
    {
        Movement();
        LookAtTarget();
    }

    void Movement()
    {
        if (currentTarget.name != "Player") // Patrulha padrão
        {
            if (Vector3.Distance(transform.position, currentTarget.position) < 1)
            {
                if(!callUpdateDestination)
                {
                    anim.SetBool("isWalking", false);
                    StartCoroutine(CallUpdateDestination());
                }
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
        }
        else // Achou o player
        {

        }
    }

    void LookAtTarget()
    {
        anim.SetFloat("X", currentTarget.position.x - transform.position.x);
        anim.SetFloat("Y", currentTarget.position.y - transform.position.y);

        if (this.transform.position.x < currentTarget.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = true;

        }
    }

    void UpdateDestination()
    {
        currentTarget = targetPoints[targetPointIndex];

        agent.SetDestination(currentTarget.position);
        Debug.Log(currentTarget.name);
    }

    void IterateTargetPointIndex()
    {
        targetPointIndex++;

        if(targetPointIndex >= targetPoints.Length)
        {
            targetPointIndex = 0;
        }
    }

    IEnumerator CallUpdateDestination()
    {
        callUpdateDestination = true;

        yield return new WaitForSeconds(3f);
        UpdateDestination();
        IterateTargetPointIndex();

        callUpdateDestination = false;

    }


}
