using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider HealthSlider;
    public Transform target;
    public Vector3 offset;
    public Image fill;
    public Color normal;
    public Color hacked;
    
    // Start is called before the first frame update
    void Awake()
    {
        HealthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(float health)
    {
        HealthSlider.value = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        HealthSlider.maxValue = maxHealth;
    }

    public void SetHacked(bool b)
    {
        if (b)
        {
            fill.color = hacked;
        }
        else
        {
            fill.color = normal;
        }
    }
    
//        public void UIOnDamageTaken(float restHealth)
//    {
//        playerHb.SetHealth(restHealth);
//    }
}
