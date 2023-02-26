using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    [Header("General Settings")]
    [SerializeField] Animator anim;

    [Header("AI Settings")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] targetPoints;

    [HideInInspector] public Transform currentTarget;
    [HideInInspector] public Collider2D playerReference;
    [HideInInspector] public bool inPlayerRange = false;

    [Header("Damage Settings")]
    public int lifePoints = 3;
    public Color32 normalColor;
    public Color32 damageColor;
    public GameObject weaponHolder;

    [Header("Overlap Sphere")]
    public float circleRadius = 12f;
    public LayerMask layerMask;

    int targetPointIndex;

    bool callUpdateDestination = false;
    bool isDead = false;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        Instance = this;
    }

    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        UpdateDestination();
    }

    void Update()
    {
        if(!isDead)
        {
            Movement();
            LookAtTarget();
            CheckPlayerInRange();
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    void Movement()
    {
        if (!inPlayerRange) // Patrulha padrão
        {
            if (Vector3.Distance(transform.position, currentTarget.position) < 1)
            {
                if (!callUpdateDestination)
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
            if (Vector3.Distance(transform.position, currentTarget.position) <= agent.stoppingDistance)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }
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
        if(!isDead)
        {
            currentTarget = targetPoints[targetPointIndex];

            agent.SetDestination(currentTarget.position);
        }
    }

    void IterateTargetPointIndex()
    {
        targetPointIndex++;

        if (targetPointIndex >= targetPoints.Length)
        {
            targetPointIndex = 0;
        }
    }

    void CheckPlayerInRange()
    {
        playerReference = Physics2D.OverlapCircle(this.transform.position, circleRadius, layerMask);

        if (playerReference != null)
        {
            agent.stoppingDistance = 7.5f;
            inPlayerRange = true;

            currentTarget = playerReference.transform;
            agent.SetDestination(currentTarget.position);
        }
        else
        {
            agent.stoppingDistance = 0;
            inPlayerRange = false;
            if (!callUpdateDestination)
            {
                anim.SetBool("isWalking", false);
                StartCoroutine(CallUpdateDestination());
            }
        }
    }

    public void SufferDamage()
    {
        lifePoints--;

        if(lifePoints <= 0)
        {
            isDead = true;
            agent.ResetPath();
            StartCoroutine(Death());
        }
        else
        {
            StartCoroutine(SufferingDamage());
        }
    }

    IEnumerator SufferingDamage()
    {
        this.GetComponent<SpriteRenderer>().color = damageColor;
        yield return new WaitForSeconds(.2f);
        this.GetComponent<SpriteRenderer>().color = normalColor;
    }

    IEnumerator Death()
    {
        Destroy(weaponHolder);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;

        anim.SetBool("isDead", true);
        TaskManager.Instance.UpdateTask(1);
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    IEnumerator CallUpdateDestination()
    {
        callUpdateDestination = true;

        yield return new WaitForSeconds(3f);
        UpdateDestination();
        IterateTargetPointIndex();

        yield return new WaitForSeconds(3f);
        callUpdateDestination = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, circleRadius);
    }

}
