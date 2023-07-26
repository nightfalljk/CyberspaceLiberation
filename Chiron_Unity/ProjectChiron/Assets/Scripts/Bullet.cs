using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Projectile
{
    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        SlowArea sa = other.GetComponent<SlowArea>();
        if (sa != null)
        {
            return;
        }
        triggered = true;
        Entity entity = other.gameObject.GetComponentInParent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(projectileDamage);
        }

        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _audioSource.Play();
        impactAnimator.gameObject.SetActive(true);
        StartCoroutine(CoResetBullet());
    }

    private IEnumerator CoResetBullet()
    {
        yield return new WaitForSeconds(1);
        impactAnimator.gameObject.SetActive(false);
        gameObject.SetActive(false);
        projectileLauncher.Enqueue(this);
    }

    public void Slowed(bool b, float amount)
    {
        if (b)
        {
            speed *= amount;
        }
        else
        {
            ResetSpeed();
        }
    }
}
