using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Update = UnityEngine.PlayerLoop.Update;

public class UIManager : MonoBehaviour, IManager
{
    public GameObject HealthbarPrefab;
    public GameObject hackIndicator;
    public GameObject PlayerHealthBar;
    public GameObject BossHealthBar;
    public GameObject EnemyHealthHolder;
    private List<Healthbar> healthbars = new List<Healthbar>();
    private Healthbar playerHb;
    private Healthbar bossHb;
    private Camera cam;
    [SerializeField] private PlayerCharacterController pcc;
    [SerializeField] private AbilitySelect abilitySelect;
    [SerializeField] private IngameUi ingameUi;
    [SerializeField] private GameObject bossApproaching;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWon;
    [SerializeField] private TMP_Text ammoDisplay;
    [SerializeField] private GameObject damage;
    [SerializeField] private float damageEffectLength = 0.5f;
    
    [SerializeField] private List<Entity> enemies;
    // Start is called before the first frame update
    void Start()
    {
        if (damage != null)
        {
            _imageHitEffect = damage.GetComponent<Image>();
            _imageHitEffect.material.SetFloat("progress", 0);
        }
        cam = Camera.main;
        ForwardProperties();
    }

    private void Update()
    {
        if(pcc!=null)
            ammoDisplay.text = pcc._projectileLauncher.Ammo.ToString();
    }

    private void OnDisable()
    {
        if(_imageHitEffect != null)
            _imageHitEffect.material.SetFloat("progress", 0);
    }

    private void ForwardProperties()
    {
        if (pcc != null)
        {
            abilitySelect.SelfSet(pcc, this);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach (Healthbar healthbar in healthbars)
        {
            Vector3 pos = healthbar.target.position + healthbar.offset;
            //Vector3 pos = ApplyCameraRotationToVector(healthbar.target.position + healthbar.offset);
            healthbar.transform.position = cam.WorldToScreenPoint(pos);
        }
    }

    private Vector3 ApplyCameraRotationToVector(Vector3 vector)
    {
        var camRot = cam.transform.rotation.eulerAngles;
        return Quaternion.AngleAxis(camRot.y, Vector3.up) * vector;
    }
    
    public void SpawnHealthBar(Entity target)
    {
        if (target is BossBehaviour)
        {
            LinkHealthBarBoss(target);
        }
        else
        {
            Healthbar hb = Instantiate(HealthbarPrefab, EnemyHealthHolder.transform).GetComponent<Healthbar>();
            hb.target = target.center;
            hb.SetMaxHealth(target.Health.Value);
            target.Health.Subscribe(h => hb.SetHealth(h));
            target.Health.Where(h => h <= 0).Subscribe(_ => hb.gameObject.SetActive(false));
            target.ShowingHealthbar.Subscribe(b=>hb.gameObject.SetActive(b));
            target.Hacked.Subscribe(h => hb.SetHacked(h));
            healthbars.Add(hb);

            GameObject hackableIndicator = Instantiate(hackIndicator, hb.gameObject.transform);
            target.hackable.Subscribe(h => hackableIndicator.SetActive(h));
        }
        
    }

    public void LinkHealthBarPlayer(Entity target)
    {
        playerHb = PlayerHealthBar.GetComponent<Healthbar>();
        playerHb.target = target.center;
        playerHb.SetMaxHealth(target.Health.Value);
        target.Health.Subscribe(h => playerHb.SetHealth(h));
        target.Health.Subscribe( _ => HitEffect());
    }

    public void LinkHealthBarBoss(Entity target)
    {
        BossHealthBar.SetActive(true);
        bossHb = BossHealthBar.GetComponent<Healthbar>();
        bossHb.target = target.center;
        bossHb.SetMaxHealth(target.Health.Value);
        target.Health.Subscribe(h => bossHb.SetHealth(h));
    }
    
    public void ResetLevel()
    {
        for (int i = healthbars.Count-1; i >= 0; i--)
        {
            Healthbar hb = healthbars[i];
            healthbars.RemoveAt(i);
            Destroy(hb.gameObject);
        }
    }

    public void StartLevel()
    {
        ingameUi.SelfSet(pcc);
        foreach (Entity enemy in enemies)
        {
            SpawnHealthBar(enemy);
        }
    }

    public void SetEnemies(List<Entity> enemies)
    {
        this.enemies = enemies;
    }

    public void AddEnemy(Entity e)
    {
        enemies.Add(e);
        SpawnHealthBar(e);
    }
    
    public void ExitAbilityMenu()
    {
        abilitySelect.gameObject.SetActive(false);
        CustomEvent.Trigger(this.gameObject, "AbilitySelectFinished");
    }

    public void OpenAbilitySelect()
    {
        abilitySelect.gameObject.SetActive(true);
    }
    
    public void OpenInGameUi()
    {
        ingameUi.gameObject.SetActive(true);
    }
    
    public void CloseInGameUi()
    {
        ingameUi.gameObject.SetActive(false);
    }

    public void ToggleBossAppraoching(bool on)
    {
        bossApproaching.SetActive(on);
    }
    
    public void ToggleGameOver(bool on)
    {
        gameOver.SetActive(on);
    }
    
    public void ToggleGameWon(bool on)
    {
        gameWon.SetActive(on);
    }
    
    public void ExitToMainMenu()
    {
        CustomEvent.Trigger(this.gameObject, "ExitToMainMenu");
    }


    private void HitEffect()
    {
        hitEffectValue = 0;
        StartCoroutine(CoHitEffect());
    }
    private float hitEffectValue = 0;
    private Image _imageHitEffect;

    private IEnumerator CoHitEffect()
    {
        while (hitEffectValue < 1)
        {
            hitEffectValue += Time.deltaTime / damageEffectLength;
            _imageHitEffect.material.SetFloat("progress",hitEffectValue);
            yield return null;
        }
    }
}
