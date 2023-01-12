using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrucibleObject : MonoBehaviour
{
    public int HitPoints;

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void TakeDamage()
    {
        HitPoints--;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            TakeDamage();
        }
    }
}
