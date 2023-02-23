using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Components")]
    [SerializeField] GameObject canvas;

    [HideInInspector] public bool endGame;

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
    }

    public void CallEndGame()
    {
        endGame = true;
        Debug.Log("Cabou o jogo");
    }
}
