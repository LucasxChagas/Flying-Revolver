using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float speed;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        this.transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
