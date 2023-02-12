using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    private void Awake()
    {
        canvas.SetActive(false);
    }

    void Start()
    {
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
