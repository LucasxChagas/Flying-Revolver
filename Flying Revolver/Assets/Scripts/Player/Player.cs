using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;

    [Header("Crosshair Settings")]
    public SpriteRenderer crossHair;

    [Header("Player Settings")]
    public float movementSpeed = 6f;
    public GameObject weaponHolder;
    public Color32 damageColor;
    public Color32 normalColor;

    [Header("HitPause Settings")]
    [SerializeField] [Range(0f, 2f)] float pauseDuration = .3f;
    float pendingPauseDuration = 0f;
    bool isPaused = false;

    [Header("HUD Settings")]
    float playerMaxHealth = 5f;
    float playerCurrentHealth;
    public Image healthBarImage;
    public Image healthBarImageEffect;

    [Header("Others")]
    Vector3 mousePosition;
    Vector2 movementInputs;

    public static bool isDead;

    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        isDead = false;
        GameManager.Instance.endGame = false;
    }

    void Update()
    {
        if(!GameManager.Instance.endGame && !isDead)
        {
            UpdatePlayerMovement();
            UpdatePlayerRotation();

            if (Input.GetKeyDown(KeyCode.P) && !TransitionSettings.inTransition) GameManager.Instance.PauseGame();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.endGame && !isDead)
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

    public void SufferDamage()
    {
        playerCurrentHealth --;
        healthBarImage.fillAmount = playerCurrentHealth / playerMaxHealth;
        StartCoroutine(StartsHitPause(pauseDuration));
        StartCoroutine(HUDHurt());

        if (healthBarImageEffect.fillAmount <= healthBarImage.fillAmount)
        {
            healthBarImageEffect.fillAmount = healthBarImage.fillAmount;
        }

        if (playerCurrentHealth <= 0)
        {
            playerCurrentHealth = 0;
            StartCoroutine(Death());
        }

    }

    IEnumerator StartsHitPause(float pauseDuration)
    {
        pendingPauseDuration = pauseDuration;
        isPaused = true;
        float originalTimeScale = Time.timeScale;
        Player.Instance.DamageFeedback(true);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseDuration);
        CameraShake.Instance.ShakeCamera(8f, .3f);
        Player.Instance.DamageFeedback(false);
        Time.timeScale = originalTimeScale;
        pendingPauseDuration = 0;
        isPaused = false;
    }

    IEnumerator HUDHurt()
    {
        while (healthBarImageEffect.fillAmount > healthBarImage.fillAmount)
        {
            healthBarImageEffect.fillAmount -= 0.009f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Death()
    {
        Destroy(weaponHolder);
        this.GetComponent<BoxCollider2D>().enabled = false;
        isDead = true;
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(3f);
        Debug.Log("Chama EndGame");
        GameManager.Instance.CallDeathScreen();
    }

    public void DamageFeedback(bool isDamage)
    {
       if(isDamage)
            GetComponent<SpriteRenderer>().color = damageColor;
       else
            GetComponent<SpriteRenderer>().color = normalColor;
    }
}
