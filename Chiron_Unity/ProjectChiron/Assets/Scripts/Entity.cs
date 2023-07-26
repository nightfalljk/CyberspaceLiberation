using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [SerializeField] protected EntityConfig config;
    public ReactiveProperty<float> Health;// = new ReactiveProperty<float>();
    public ReactiveProperty<bool> IsAlive;// = new ReactiveProperty<bool>(true);
    public ReactiveProperty<bool> ShowingHealthbar;
    public ReactiveProperty<bool> Hacked;
    public ReactiveProperty<bool> hackable;
    public Transform center;
    //public UnityEvent entityDeath;
    protected bool CanDie { get; set; }
    public float MoveSpeed { get; set; }

    protected virtual void Awake()
    {
        if (center == null)
            center = transform;
        LoadConfig();
        ShowingHealthbar.Value = true;
        Hacked.Value = false;
    }

    protected virtual void Start()
    {
        IsAlive.Value = gameObject.activeInHierarchy;
    }

    protected virtual void LoadConfig()
    {
        if (config == null)
        {
            Debug.LogError("No config attached to "+gameObject.name);
            return;
        }
        CanDie = config.canDie;
        Health.Value = config.maxHealth;
        MoveSpeed = config.moveSpeed;
    }
    
    public virtual bool TakeDamage(float damage)
    {
        Health.Value -= damage;
        if (Health.Value <= 0)
        {
            Die();
        }

        return true;
    }

    public void Kill()
    {
        Die();
    }
    
    protected virtual bool Die()
    {
        if(!CanDie)
            return false;
        //gameObject.SetActive(false);
        IsAlive.Value = false;
        //entityDeath.Invoke();
        CustomEvent.Trigger(this.gameObject, "entityDeath");
        CanDie = false;
        return true;
    }

    public bool Revive()
    {
        LoadConfig();
        IsAlive.Value = true;
        gameObject.SetActive(true);
        return true;
    }
    
    /// <summary>
    /// Getter for reactive property
    /// </summary>
    /// <returns></returns>
    public bool GetIsAlive()
    {
        return IsAlive.Value;
    }
    
    public virtual Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>();
        functions.Add("f.Die",(Func<bool>)(Die));
        functions.Add("f.TakeDamage",(Func<float, bool>)(TakeDamage));
        functions.Add("f.IsAlive",(Func<bool>)(GetIsAlive));
        return functions;
    }

    public virtual Dictionary<string, object> GetParameters()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        System.Reflection.FieldInfo[] fields = config.GetType().GetFields();
        foreach (var item in fields)
        {
            var o = item.GetValue(config);
            parameters.Add(item.Name,o);
        }
        return parameters;
        
    }

    public T GetParameter<T>(string name)
    {
        if (config == null || string.IsNullOrEmpty(name))
            return default;
        System.Reflection.FieldInfo field = config.GetType().GetField(name);
        if (field != null)
        {
            object o = field.GetValue(config);
            return (T) o;
        }

        return default;
    }

    public void SetParameter(string name, object o)
    {
        if (config==null || string.IsNullOrEmpty(name))
        {
            return;
        }
        System.Reflection.FieldInfo field = config.GetType().GetField(name);
        if (field != null)
        {
            field.SetValue(config,o);
        }
    }

    public EntityConfig GetConfig()
    {
        return config;
    }

    public void SetConfig(EntityConfig entityConfig)
    {
        config = entityConfig;
    }

    public float GetMaxHealth()
    {
        return config.maxHealth;
    }
}
