using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolver : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float fireRate;
    [SerializeField] GameObject bullet;
    Enemy enemy;

    float fireTimer;

    private void Start()
    {
        enemy = this.GetComponentInParent<Enemy>();
    }

    void Update()
    {
        if (!GameManager.Instance.endGame)
        {
            if (CanShot() && enemy.inPlayerRange)
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
