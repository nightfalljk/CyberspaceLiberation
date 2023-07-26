using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
 
    public float speed = 0.1f;
    public float projectileDamage = 21;
    public ProjectileLauncher projectileLauncher;
    public bool triggered = false;

    [SerializeField] private Animator animator;
    [SerializeField] protected Animator impactAnimator;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected AudioSource _audioSource;
    //[SerializeField] private AnimatorController animatorController1;
    //[SerializeField] private AnimatorController animatorController2;
    
    private float speedOriginal;

    private void Awake()
    {
        speedOriginal = speed;
    }

    void Update()
    {
        if (!triggered)
        {
            var transform1 = transform;
            transform1.position += Time.deltaTime * speed * transform1.up;
        }
    }
    
    public void SetProjectileLauncher(ProjectileLauncher projectileLauncher)
    {
        this.projectileLauncher = projectileLauncher;
    }
    public void SetProjectileDamage(float dmg)
    {
        projectileDamage = dmg;
    }
    
    public void SetTriggered(bool triggered)
    {
        this.triggered = triggered;
    }

    public void SetTheme(int theme)
    {
//        if (animator == null || animatorController1 == null || animatorController2 == null)
//            return;
//        if (t == 0)
//        {
//            animator.runtimeAnimatorController = animatorController1;
//        }
//        else
//        {
//            animator.runtimeAnimatorController = animatorController2;
//        }
        bool found = false;
        foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
        {
            if (animatorControllerParameter.name == "vapor" || animatorControllerParameter.name == "outrun")
            {
                found = true;
            }
        }
        
        if (!found)
        {
            return;
        }
        
        animator.SetTrigger(theme == 0 ? "vapor" : "outrun");
    }

    protected void ResetSpeed()
    {
        speed = speedOriginal;
    }
    
}
