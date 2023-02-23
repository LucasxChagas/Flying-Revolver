using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Camera cam_;
    [SerializeField] float cameraLimit;

    Transform player;
    Vector3 targetPosition;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (!GameManager.Instance.endGame)
        {
            targetPosition = (player.position + cam_.ScreenToWorldPoint(Input.mousePosition)) / 2f;

            targetPosition.x = Mathf.Clamp(targetPosition.x, -cameraLimit + player.position.x, cameraLimit + player.position.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -cameraLimit + player.position.y, cameraLimit + player.position.y);

            this.transform.position = targetPosition;
        }
    }
}
