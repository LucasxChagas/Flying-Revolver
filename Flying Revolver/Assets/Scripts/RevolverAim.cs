using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverAim : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] Transform owner;

    Vector3 direction;
    Vector3 playerToMouseDirection;

    float angle;

    void Update()
    {
        if (!GameManager.Instance.endGame && !Player.isDead)
        {
            direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);

            playerToMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - owner.position;
            playerToMouseDirection.z = 0;
            transform.position = owner.position + (offset * playerToMouseDirection.normalized);

            if (angle > 90 || angle < -90) transform.localScale = new Vector3(1, -1, 1);
            else transform.localScale = new Vector3(1, 1, 1);

            if (angle > 17.5f)
            {
                foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.sortingOrder = -1;
                }
            }
            else
            {
                foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.sortingOrder = 1;
                }
            }
        }
    }
}
