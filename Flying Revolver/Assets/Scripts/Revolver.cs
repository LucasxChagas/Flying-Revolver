using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] Transform barrel;
    [SerializeField] float fireRate;
    [SerializeField] GameObject bullet;

    float fireTimer;
    
    void Update()
    {
        if(Input.GetMouseButton(0) && CanShot())
        {
            fireTimer = Time.time + fireRate;

            Instantiate(bullet, barrel.position, barrel.rotation);
            GetComponent<Animator>().SetTrigger("Fire");
            CameraShake.Instance.ShakeCamera(5f, .1f);
        }
    }

    bool CanShot()
    {
        return Time.time > fireTimer;
    }
}
