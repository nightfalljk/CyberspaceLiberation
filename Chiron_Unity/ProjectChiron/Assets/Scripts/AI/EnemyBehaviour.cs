using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UniRx;

public class EnemyBehaviour : Entity
{
    
    [FormerlySerializedAs("player")] [SerializeField] protected Transform target;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform bulletSpawnpoint;
    [SerializeField] protected LayerMask visibilityMask;
    [SerializeField] protected LayerMask laserMask;
    [SerializeField] protected Animator animator;
    
    [SerializeField] private SpawnHole _spawnHole;
    [SerializeField] protected SkinnedMeshRenderer _meshRenderer;
    [SerializeField] public Collider _collider;
    [SerializeField] private Animator dyingAnimator;
    [SerializeField] private AudioSource shootSound;
    
    //[SerializeField] private AnimatorController dying1;
    //[SerializeField] private AnimatorController dying2;

    public int theme = 0;
    public XNodeGraph btGraph;
    public string type_prefab;
    protected float WalkCooldown { get; private set; }
    protected ProjectileLauncher _projectileLauncher;
    //private Dictionary<Transform, LineRenderer> _lineRenderers = new Dictionary<Transform, LineRenderer>();

    //private float laserDamageCooldown = 0;

    [SerializeField] protected int stage;
    //private Vector3 targetDirection;
    protected NavMeshAgent NavMeshAgent;

    protected override void Awake()
    {
        _projectileLauncher = GetComponent<ProjectileLauncher>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Hide();
        IsAlive.Subscribe(b =>
        {
            if(NavMeshAgent.isOnNavMesh)
                NavMeshAgent.isStopped = !b;
        });
        base.Awake();
        _projectileLauncher.SetTheme(theme);
        SetTheme(theme);
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
        EnemyBaseConfig ebc = (EnemyBaseConfig) config;
        WalkCooldown = ebc.walkCooldown;
        NavMeshAgent.speed = ebc.moveSpeed;
        NavMeshAgent.acceleration = ebc.acceleration;
        //shootingTimer = config.ShootingCooldown;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void SetTheme(int theme)
    {
//        if (theme == 0)
//        {
//            dyingAnimator.runtimeAnimatorController = dying1;
//        }
//        else
//        {
//            dyingAnimator.runtimeAnimatorController = dying2;
//        }
    }
    
    public virtual bool Shoot()
    {
        if (!IsAlive.Value)
        {
            Debug.LogWarning("Shooting while dead");
            return false;
        }
        //targetDirection = target.position - bulletSpawnpoint.position;
        Quaternion shootingDirection = Quaternion.FromToRotation(bulletPrefab.transform.up,GetTargetDirection(target));
        _projectileLauncher.Fire(shootingDirection);
        shootSound.Play();
        return true;
    }

    private Vector3 GetTargetDirection(Transform t)
    {
        return (t.transform.position - bulletSpawnpoint.position).normalized;
    }
    
    public GameObject GetTarget()
    {
        return target.gameObject;
    }

    public void SetTarget(GameObject t)
    {
        if(t!= null)
            target = t.transform;
    }
    
    public bool GetTargetVisibleG(GameObject t)
    {
        RaycastHit hit;
        bool isTarget = false;
        if (t == null)
        {
            t = target.gameObject;
            isTarget = true;
        }
        Vector3 direction = GetTargetDirection(t.transform);
        LayerMask lm = isTarget ? visibilityMask : laserMask;
        if (Physics.Raycast(bulletSpawnpoint.position, direction, out hit, Mathf.Infinity, lm))
        {

            GameObject go = hit.collider.gameObject;
            Entity entity = hit.collider.gameObject.GetComponent<Entity>();
            if (go != null && go == t && entity.IsAlive.Value)
            {
                Debug.DrawRay(bulletSpawnpoint.position, direction * hit.distance, Color.red);
                return true;
            }
            else
            {
                Debug.DrawRay(bulletSpawnpoint.position, direction * hit.distance, Color.yellow);
                return false;
            }
        }
        else
        {
            Debug.DrawRay(bulletSpawnpoint.position, direction * 1000, Color.white);
        }
        return false;
    }
    
    public bool GetTargetVisible()
    {
        return GetTargetVisibleG(null);
    }

    public List<int> GetStages()
    {
        EnemyBaseConfig c = config as EnemyBaseConfig;
        return c.difficultyCost;
    }

    public void SetStage(int stage)
    {
        this.stage = stage;
    }

    public bool TriggerAnimation(string name)
    {
        if (animator == null)
        {
            Debug.LogWarning("no animator set for "+gameObject.name);
            return false;
        }
        animator.SetTrigger(name);
        return true;
    }

    public override bool TakeDamage(float damage)
    {
        bool b = base.TakeDamage(damage);
        TriggerAnimation("hit");
        return b;
    }

    protected override bool Die()
    {
        bool b = base.Die();
        if (b)
        {
            TriggerAnimation("death");
            dyingAnimator.gameObject.SetActive(true);
            dyingAnimator.SetTrigger(theme == 0 ? "vapor" : "outrun");
            _collider.enabled = false;
        }

        return b;
    }

    
    public virtual bool Hide()
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;
        ShowingHealthbar.Value = false;
        dyingAnimator.gameObject.SetActive(false);
        return true;
    }
    public virtual bool Show()
    {
        _meshRenderer.enabled = true;
        _collider.enabled = true;
        ShowingHealthbar.Value = true;
        return true;
    }

    public bool OpenHole()
    {
        StartCoroutine(_spawnHole.CoScale());
        return true;
    }

    public bool CloseHole()
    {
        StartCoroutine(_spawnHole.CoScale(true));
        return true;
    }

    public ProjectileLauncher ProjectileLauncher => _projectileLauncher;

    
    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
        functions.Add("f.TargetVisibleG",(Func<GameObject, bool>)(GetTargetVisibleG));
        functions.Add("f.TargetVisible",(Func<bool>)(GetTargetVisible));
        functions.Add("f.Target", (Func<GameObject>)(GetTarget));
        functions.Add("f.Shoot",(Func<bool>)(Shoot));
        functions.Add("f.TriggerAnimation",(Func<string,bool>) TriggerAnimation);
        functions.Add("f.open",(Func<bool>)(OpenHole));
        functions.Add("f.close",(Func<bool>)(CloseHole));
        functions.Add("f.show",(Func<bool>)(Show));
        functions.Add("f.hide",(Func<bool>)(Hide));
        return functions;
    }

    private void OnDestroy()
    {
        Destroy(_projectileLauncher);
    }

    public int GetStage()
    {
        return stage;
    }

    public float GetWaitBetweenShots()
    {
        return ProjectileLauncher.GetFireRate();
    }

    private bool slowed = true;

    public void SlowEnemy(bool b, float amountAttack, float amountMovement)
    {
        if (b && !slowed)
        {
            NavMeshAgent.speed *= amountMovement;
            ProjectileLauncher.Slow(amountAttack);
            slowed = true;
        }
        else if(!b && slowed)
        {
            EnemyBaseConfig ebc = (EnemyBaseConfig) config;
            NavMeshAgent.speed = ebc.moveSpeed;
            ProjectileLauncher.SlowReset();
            slowed = false;
        }
        
    }
    
}
