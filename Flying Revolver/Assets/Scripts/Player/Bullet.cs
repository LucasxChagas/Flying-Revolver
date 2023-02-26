using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] float speed;
    bool hasHit = false;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        if (!hasHit) this.transform.Translate(Vector2.right * speed * Time.deltaTime);
        else this.transform.Translate(Vector3.zero);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasHit = true;
            this.GetComponent<Animator>().SetTrigger("BulletHit");
            collision.GetComponent<Enemy>().SufferDamage();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            hasHit = true;
            this.GetComponent<Animator>().SetTrigger("BulletHit");
            collision.GetComponent<Player>().SufferDamage();
        }
    }

    public void ANIM_DestroyOnHit()
    {
        Destroy(this.gameObject);
    }


}
