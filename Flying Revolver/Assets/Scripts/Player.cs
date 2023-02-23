using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;

    [Header("Crosshair Settings")]
    public SpriteRenderer crossHair;

    [Header("Player Settings")]
    public float movementSpeed = 6f;

    [Header("Others")]
    Vector3 mousePosition;
    Vector2 movementInputs;


    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(!GameManager.Instance.endGame)
        {
            UpdatePlayerMovement();
            UpdatePlayerRotation();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.endGame)
        {
            rb.velocity = new Vector2(movementInputs.x, movementInputs.y) * movementSpeed;
        }
        else
        {
            anim.SetBool("isWalking", false);
            rb.velocity = Vector3.zero;
        }

        UpdateCrosshair();
    }

    void UpdatePlayerMovement()
    {
        movementInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (rb.velocity.magnitude > 0) anim.SetBool("isWalking", true);
        else anim.SetBool("isWalking", false);
    }

    void UpdateCrosshair()
    {
        crossHair.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
    void UpdatePlayerRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        anim.SetFloat("MouseX", mousePosition.x - transform.position.x);
        anim.SetFloat("MouseY", mousePosition.y - transform.position.y);

        if (this.transform.position.x < mousePosition.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = true;

        }
    }
}
