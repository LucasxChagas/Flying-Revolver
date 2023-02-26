using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Components")]
    [SerializeField] GameObject canvas;

    [HideInInspector] public bool endGame;

    [Header("HitPause Settings")]
    [SerializeField] [Range(0f, 2f)] float pauseDuration = 1f;
    float pendingPauseDuration = 0f;
    bool isPaused = false;


    private void Awake()
    {
        canvas.SetActive(false);
        endGame = false;

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;

    }

    void Start()
    {
        canvas.SetActive(true);
        Cursor.visible = false;
    }

    void Update()
    {
        if (pendingPauseDuration > 0 && !isPaused)
        {
            StartCoroutine(StartsHitPause());
        }
    }

    public void CallEndGame()
    {
        endGame = true;
        Debug.Log("Cabou o jogo");
    }

    public void HitPause()
    {
        pendingPauseDuration = pauseDuration;

    }

    IEnumerator StartsHitPause()
    {
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
}
