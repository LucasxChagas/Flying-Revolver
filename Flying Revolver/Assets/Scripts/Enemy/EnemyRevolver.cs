using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolver : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float fireRate;
    [SerializeField] GameObject bullet;

    float fireTimer;

    void Update()
    {
        if (!GameManager.Instance.endGame)
        {
            if (CanShot() && Enemy.Instance.inPlayerRange)
            {
                fireTimer = Time.time + fireRate;

                Instantiate(bullet, barrel.position, barrel.rotation);
                GetComponent<Animator>().SetTrigger("Fire");
            }
        }
    }
    bool CanShot()
    {
        return Time.time > fireTimer;
    }
}
