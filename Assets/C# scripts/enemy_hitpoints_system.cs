using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hitpoints_system : MonoBehaviour
{
    public int hitpoints = 5;


    void Update()
    {
        if (hitpoints <= 0)
        {
            Destroy(gameObject);
            print("An enemy is killed!");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;

                float knockbackForce = 1f; 
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            TakeDamage();
        }
    }

    void TakeDamage()
    {
        hitpoints--;
        print("An enemy took damage!");
    }
}
