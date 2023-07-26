using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlowProjectile : Projectile
{
    
    [SerializeField] private SlowFieldConfig _slowFieldConfig;
    private Rigidbody _rigidbody;
    private float _height;
    private Transform _target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    void Update()
    {
        
    }
    

    public void FireProjectile(Transform target, float height)
    {
        _target = target;
        _height = height;
        _rigidbody.useGravity = true;
        _rigidbody.velocity = CalculateVelocity();

    }

    private Vector3 CalculateVelocity()
    {
        var targetPosition = _target.position;
        var rbPosition = _rigidbody.position;
        
        var verticalDisplacement = targetPosition.y - rbPosition.y;
        var horizontalDisplacement = new Vector3(targetPosition.x - rbPosition.x, 0, targetPosition.z - rbPosition.z);

        var verticalVel = Vector3.up * Mathf.Sqrt(-2 * _slowFieldConfig.gravity * _height);
        var horizontalVel = horizontalDisplacement / 
                            (Mathf.Sqrt(-2 * _height / _slowFieldConfig.gravity) +
                             Mathf.Sqrt(2 * (verticalDisplacement - _height) / _slowFieldConfig.gravity));

        return horizontalVel + verticalVel;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") 
            || other.gameObject.layer == LayerMask.NameToLayer("SeeThrough")
            || other.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            _rigidbody.isKinematic = true;
            speed = 0;
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(Lifetime());
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(_slowFieldConfig.duration);
        Destroy(gameObject);
    }
}
