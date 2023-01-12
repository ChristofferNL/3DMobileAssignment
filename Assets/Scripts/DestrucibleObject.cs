using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrucibleObject : MonoBehaviour
{
    public int HitPoints;

    public ParticleSystem ParticleSystem;

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void TakeDamage()
    {
        HitPoints--;
        if (HitPoints <= 0)
        {
            EventManager.Instance.ParticlePlayEvent(ParticleSystem, transform.position);
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
