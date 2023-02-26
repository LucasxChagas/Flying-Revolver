using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Components")]
    [SerializeField] GameObject canvas;

    [HideInInspector] public bool endGame;

    [Header("Others")]
    public Animator EndGameScreen;
    public Animator DeathScreen;
    public GameObject PauseScreen;

    bool alreadyClickedButton = false;

    bool gameIsPaused = false;

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
        canvas.GetComponent<Animator>().Play("Canvas - Fade Out");
        Cursor.visible = false;
    }

    public void SocialLinks()
    {
        Application.OpenURL("https://lucasxchagas.carrd.co");
    }

    public void PlayAgain()
    {
        if (!alreadyClickedButton)
        {
            alreadyClickedButton = true;
            StartCoroutine(ReloadScene(.9f));
            canvas.GetComponent<Animator>().Play("Canvas - Fade In");
        }
    }


    public void PauseGame()
    {
        Debug.Log("Pause");
        gameIsPaused = !gameIsPaused;

        if(gameIsPaused)
        {
            PauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            PauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CallEndGame()
    {
        endGame = true;
        EndGameScreen.gameObject.SetActive(true);
        EndGameScreen.Play("DeathScreenAnimation");
    }

    public void CallDeathScreen()
    {
        DeathScreen.gameObject.SetActive(true);
        DeathScreen.Play("DeathScreenAnimation");
    }

    IEnumerator ReloadScene(float timer)
    {
        yield return new WaitForSeconds(timer);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
