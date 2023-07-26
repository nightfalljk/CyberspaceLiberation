using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController), typeof(ProjectileLauncher))]
public class PlayerCharacterController : Entity, IManager
{

    [SerializeField] private PlayerCharacterControllerInput controllerInput;
    [SerializeField] private DashConfig dashConfig;
    [SerializeField] private SecondLifeConfig secondLifeConfig;
    [SerializeField] private WeaponBoostConfig weaponBoostConfig;
    [SerializeField] private HackConfig hackConfig;
    [SerializeField] private SlowFieldConfig slowFieldConfig;
    [SerializeField] private TeleportConfig teleportConfig;
    
    [SerializeField] private GameObject cameraDolly;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private float camMoveMaxDist;
    [SerializeField] private float camDeadZone;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject slowProjectileTarget;

    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private AudioSource hackSound;
    [SerializeField] private AudioSource actionFailedSound;
    [SerializeField] private AudioSource weaponBoostSound;
    [SerializeField] private AudioSource slowFieldSound;


    [FormerlySerializedAs("channelingEffect")] [SerializeField] private VisualEffect channelingEffect_old;
    [SerializeField] private VisualEffect channelingEffect;
    [SerializeField] private Animator channelingAnimator;
    
    public Camera cam;
    private int _layerMask;
    public ProjectileLauncher _projectileLauncher;
    private Vector3 _aimDir;


    private bool _moveLock;
    private bool _aimLock;
    private bool _shootLock;
    private bool _channeling;

    private float _timeSinceLastShot;
    
    private Teleport _teleport;

    private ReactiveProperty<bool> _secondLifeAvailable;
    private ReactiveProperty<bool> _secondLifeEnabled;
    private int _secondLifeCharges;
    
    private Hack _hack;
    
    
    private ReactiveProperty<bool> _slowFieldEnabled;
    private ReactiveProperty<bool> _slowFieldAvailable;
    private ReactiveProperty<float> _slowFieldDuration;
    private ReactiveProperty<float> _slowFieldCooldown;
    private float _slowFieldHeight;
 
    private WeaponBoost _weaponBoost;
    
    private Vector3 _dashDir;
    private Dash _dash;

    private CharacterController _characterController;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _projectileLauncher = GetComponent<ProjectileLauncher>();
        _aimDir = Vector3.zero;
        _dashDir = ApplyCameraRotationToVector(transform.forward).normalized;

        _timeSinceLastShot = 0; 

        _moveLock = true;
        _aimLock = true;
        _shootLock = true;
        _channeling = false;
        _secondLifeCharges = secondLifeConfig.charges;

        _layerMask = (1<<12);

        _weaponBoost = gameObject.AddComponent<WeaponBoost>();
        _weaponBoost.SetWeaponBoostConfig(weaponBoostConfig);
        _weaponBoost.SetProjectileLaucher(_projectileLauncher);
        weaponBoostConfig.ResetTutCondition();
        
        _dash = gameObject.AddComponent<Dash>();
        _dash.SetDashConfig(dashConfig);
        dashConfig.ResetTutCondition();

        _hack = GetComponentInChildren<Hack>();
        _hack.SetHackConfig(hackConfig);

        _teleport = cameraDolly.GetComponent<Teleport>();
        _teleport.SetTeleportConfig(teleportConfig);
        

        _hack.HackEnabled.Value = true;
        _dash.DashEnabled.Value = true;


        _weaponBoost.WeaponBoostEnabled.Value = true;
        
        _secondLifeAvailable = new ReactiveProperty<bool>();
        _secondLifeEnabled = new ReactiveProperty<bool>();
        _secondLifeAvailable.Value = true;
        _secondLifeEnabled.Value = false;
        
        
        _slowFieldEnabled = new ReactiveProperty<bool>();
        _slowFieldAvailable = new ReactiveProperty<bool>();
        _slowFieldCooldown = new ReactiveProperty<float>();
        _slowFieldDuration = new ReactiveProperty<float>();
        _slowFieldCooldown.Value = 1;
        _slowFieldDuration.Value = 1;
        _slowFieldEnabled.Value = false;
        _slowFieldAvailable.Value = true;
        slowFieldConfig.ResetTutCondition();
        _slowFieldHeight = slowFieldConfig.maxHeight;
    }

    protected override void Start()
    {
        base.Start();
        //MOVING
        controllerInput.Move
            .Subscribe(input =>
            {
                if (input == Vector2.zero)
                {
                    animator.SetBool("Driving", false);
                }
                else
                {

                    if (!_moveLock && !_channeling)
                    {
                        var inputVelocity = input.normalized * MoveSpeed;
                        var characterVelocity =
                            ApplyCameraRotationToVector(new Vector3(inputVelocity.x, -1, inputVelocity.y));
                        var distance = characterVelocity * Time.deltaTime;
                        _dashDir = characterVelocity.normalized;
                        animator.SetBool("Driving", true);
                        _characterController.Move(distance);
                    }
                }
            }).AddTo(this);
        
        //AIMING
        controllerInput.Aim
            .Where(v => v != Vector2.zero)
            .Subscribe(input =>
            {
                if (!_aimLock && !_channeling)
                {
                    var playerPos = transform.position;
                    var pos = cam.WorldToScreenPoint(playerPos);
                    var aimDir2D = ((new Vector2(pos.x, pos.y) - input) * (-1)).normalized;
                    _aimDir = ApplyCameraRotationToVector(new Vector3(aimDir2D.x, 0, aimDir2D.y));
                    var angle = Mathf.Atan2(aimDir2D.y, aimDir2D.x) * Mathf.Rad2Deg -
                                cam.transform.rotation.eulerAngles.y;
                    transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
                    
                    Ray ray = cam.ScreenPointToRay(input);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000f, _layerMask))
                    {
                        var camPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        var camDist = (camPos - playerPos).magnitude;
                        
                        if(camDist < camDeadZone)
                            cameraTarget.transform.position = playerPos;
                        else 
                        {
                            cameraTarget.transform.position = camPos;
                            cameraTarget.transform.localPosition = cameraTarget.transform.localPosition.normalized * camMoveMaxDist;
                        }

                        if (_slowFieldAvailable.Value && camDist < slowFieldConfig.range)
                        {
                            slowProjectileTarget.transform.position = camPos;
                            var dist = new Vector2(camPos.x - playerPos.x, camPos.z - playerPos.z).magnitude;
                            _slowFieldHeight = dist / slowFieldConfig.maxHeight;
                        }
                        else if(_slowFieldAvailable.Value)
                        {
                            slowProjectileTarget.transform.position = camPos;
                            slowProjectileTarget.transform.localPosition =
                                slowProjectileTarget.transform.localPosition.normalized * slowFieldConfig.range;
                            _slowFieldHeight = slowFieldConfig.maxHeight;
                        }
                    }
                }
            }).AddTo(this);
        

        //SHOOTING
        controllerInput.Shoot
            .Subscribe(input =>
            {
                if (!_shootLock && !_channeling)
                {
                    if (_projectileLauncher.Ammo != 0)
                    {
                        _timeSinceLastShot = 0.5f;
                        animator.SetBool("Shoot", true);
                        shootSound.Play();
                    }

                    _projectileLauncher.Fire(_aimDir);
                }
            }).AddTo(this);
        
        //SLOW ABILITY
        controllerInput.SlowField
            .Subscribe(input =>
            {
                if (_slowFieldEnabled.Value)
                {
                    if (_slowFieldAvailable.Value)
                    {
                        if (slowFieldConfig.tutExecutionCount.Value < slowFieldConfig.tutConditionCount)
                        {
                            slowFieldConfig.tutExecutionCount.Value++;
                            if(slowFieldConfig.tutExecutionCount.Value == slowFieldConfig.tutConditionCount)
                                slowFieldConfig.tutCondition.Value = true;
                        }

                        _slowFieldAvailable.Value = false;
                        var angle = Mathf.Atan2(_aimDir.z, _aimDir.x) * Mathf.Rad2Deg - 90f;
                        var rotation = Quaternion.AngleAxis(-angle, Vector3.up);
                        //var rotationX = new Vector3(slowFieldConfig.firingAngle, 0, 0);
                        var finalRotation = rotation.eulerAngles;// + rotationX;
                        var projectile = Instantiate(slowFieldConfig.slowProjectile, _projectileLauncher.BulletSpawn.position, Quaternion.Euler(finalRotation));
                        
                        projectile.GetComponent<SlowProjectile>().FireProjectile(slowProjectileTarget.transform, _slowFieldHeight);
                        
                        slowFieldSound.Play();
                        StartCoroutine(SlowfieldCooldown());
                    }
                    else
                    {
                        actionFailedSound.Play();
                    }
                    
                }
            }).AddTo(this);

        //WEAPON BOOST
        controllerInput.WeaponBoost
            .Subscribe(input =>
            {
                if (_weaponBoost.WeaponBoostEnabled.Value)
                {
                    if (_weaponBoost.BoostingWeaponAvailable.Value)
                    {
                        _channeling = true;
                        weaponBoostSound.Play();
                        StartCoroutine(_weaponBoost.BoostingWeapon());
                        StartCoroutine(ChannelingDuration(_weaponBoost.ChannelingTime));
                    }
                    else
                    {
                        actionFailedSound.Play();
                    }
                }
            }).AddTo(this);
        
        //HACK
        controllerInput.Hack
            .Subscribe(input =>
            {
                if (_hack.HackEnabled.Value)
                {
                    if (_hack.HackAvailable.Value)
                    {
                        bool success = _hack.HackEnemy(_projectileLauncher.Projectile);
                        if (success)
                        {
                            hackSound.Play();
                        }
                        else
                        {
                            actionFailedSound.Play();
                        }
                    }
                    else
                    {
                        actionFailedSound.Play();
                    }
                    
                }
                
            }).AddTo(this);
       
        //DASH
        controllerInput.Dash
            .Where(b => b == true)
            .Subscribe(input =>
            {
                if(_dash.DashEnabled.Value)
                {
                    if (_dash.DashRdy.Value)
                    {
                        _moveLock = input;
                        _dash.Dashing = input;
                        dashSound.Play();
                        StartCoroutine(_dash.FirstDash());
                        StartCoroutine(MoveLockDuration(_dash.DashDuration));
                    }
                    else if (_dash.DoubleDashRdy.Value && _moveLock == false)
                    {
                        _moveLock = input;
                        _dash.Dashing = input;
                        dashSound.Play();
                        StartCoroutine(_dash.DoubleDash());
                        StartCoroutine(MoveLockDuration(_dash.DashDuration));

                    }
                }
            }).AddTo(this);
        
        //TELEPORT
        controllerInput.TeleportEnable
            .Subscribe(input =>
            {
                if (input && _teleport.TeleportEnabled.Value)
                {
                    if (_teleport.TeleportAvailable.Value)
                    {
                        _moveLock = true;
                        _aimLock = true;
                        _shootLock = true;
                        _teleport.TeleportActive = true;
                        cameraDolly.GetComponentInChildren<CinemachineVirtualCamera>().Follow = null;
                        _teleport.ToggleTeleportIndicator();
                        Time.timeScale = _teleport.GetTimescaleFactor();
                        controllerInput.ToggleTeleportInput(true);
                    }
                    else
                    {
                        actionFailedSound.Play();
                    }
                }
            }).AddTo(this);

    }

    private void Update()
    {
        if (_dash.Dashing)
        {
            _characterController.Move(_dashDir * (Time.deltaTime * dashConfig.speed));
        }

        if (_timeSinceLastShot > 0)
        {
            _timeSinceLastShot -= Time.deltaTime;
        }
        else if (_timeSinceLastShot < 0)
        {
            _timeSinceLastShot = 0;
            animator.SetBool("Shoot", false);
        }

        if (_projectileLauncher.Ammo == 0)
        {
            reloadSound.Play();
            animator.SetBool("Shoot", false);
        }
    }

    private Vector3 ApplyCameraRotationToVector(Vector3 vector)
    {
        var camRot = cam.transform.rotation.eulerAngles;
        return Quaternion.AngleAxis(camRot.y, Vector3.up) * vector;
    }

    private IEnumerator SlowfieldCooldown()
    {
        yield return new WaitForCooldown(slowFieldConfig.cooldown, _slowFieldCooldown);
        _slowFieldAvailable.Value = true;
    }
    
    private IEnumerator MoveLockDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _moveLock = false;
    }

    private IEnumerator ChannelingDuration(float duration)
    {
        //channelingEffect_old.Play();
        //channelingAnimator.enabled = true;
        channelingEffect.Play();
        yield return new  WaitForSeconds(duration);
        channelingAnimator.enabled = false;
        //channelingEffect_old.Stop();
        channelingEffect.Stop();
        _channeling = false;
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
        PlayerConfig pc = (PlayerConfig) config;
    }

    public void SetPlayerConfig(PlayerConfig config)
    {
        this.config = config;
    }

    public void SetConfigs(PlayerConfig playerConfig, WeaponConfig playerWeaponConfig, DashConfig dashConfig,
        HackConfig hackConfig, SecondLifeConfig secondLifeConfig, SlowFieldConfig slowFieldConfig,
        TeleportConfig teleportConfig, WeaponBoostConfig weaponBoostConfig)
    {
        config = playerConfig;
        _projectileLauncher.SetWeaponConfig(playerWeaponConfig);
        _dash.SetDashConfig(dashConfig);
        _hack.SetHackConfig(hackConfig);
        this.secondLifeConfig = secondLifeConfig;
        this.slowFieldConfig = slowFieldConfig;
        _teleport.SetTeleportConfig(teleportConfig);
        _weaponBoost.SetWeaponBoostConfig(weaponBoostConfig);
    }

    public void EnableDash()
    {
        _dash.DashEnabled.Value = true;
        _teleport.TeleportEnabled.Value = false;
    }

    public void EnableTeleport()
    {
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = true;
    }

    public void EnableHack()
    {
        _hack.HackEnabled.Value = true;
        _slowFieldEnabled.Value = false;
    }

    public void EnableSlowField()
    {
        _hack.HackEnabled.Value = false;
        _slowFieldEnabled.Value = true;
    }

    public void EnableSecondLife()
    {
        _secondLifeEnabled.Value = true;
        _weaponBoost.WeaponBoostEnabled.Value = false;
    }

    public void EnableWeaponBoost()
    {
        _secondLifeEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = true;
    }
    
    public void TutEnableDash()
    {
        _dash.DashEnabled.Value = true;
        _teleport.TeleportEnabled.Value = false;
        _slowFieldEnabled.Value = false;
        _hack.HackEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _secondLifeEnabled.Value = false;
    }

    public void TutEnableTeleport()
    {
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = true;
        _slowFieldEnabled.Value = false;
        _hack.HackEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _secondLifeEnabled.Value = false;
    }

    public void TutEnableHack()
    {
        _hack.HackEnabled.Value = true;
        _slowFieldEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _secondLifeEnabled.Value = false;
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = false;
    }

    public void TutEnableSlowField()
    {
        _hack.HackEnabled.Value = false;
        _slowFieldEnabled.Value = true;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _secondLifeEnabled.Value = false;
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = false;
    }

    public void TutEnableSecondLife()
    {
        _secondLifeEnabled.Value = true;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _slowFieldEnabled.Value = false;
        _hack.HackEnabled.Value = false;
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = false;
    }

    public void TutEnableWeaponBoost()
    {
        _secondLifeEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = true;
        _slowFieldEnabled.Value = false;
        _hack.HackEnabled.Value = false;
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = false;
    }

    public void EnterTutorial()
    {
        _moveLock = true;
        _aimLock = true;
        _shootLock = true;
        _secondLifeEnabled.Value = false;
        _weaponBoost.WeaponBoostEnabled.Value = false;
        _slowFieldEnabled.Value = false;
        _hack.HackEnabled.Value = false;
        _dash.DashEnabled.Value = false;
        _teleport.TeleportEnabled.Value = false;
    }

    public ReactiveProperty<bool> DashAvailable => _dash.DoubleDashRdy;
    public ReactiveProperty<bool> DashEnabled => _dash.DashEnabled;
    public ReactiveProperty<float> DashCooldownPercentage => _dash.CooldownPercentage;
    public ReactiveProperty<float> DashDurationPercentage => _dash.DurationPercentage;
    public ReactiveProperty<bool> HackAvailable => _hack.HackAvailable;
    public ReactiveProperty<bool> HackEnabled => _hack.HackEnabled;
    public ReactiveProperty<float> HackCooldownPercentage => _hack.HackCooldownPercentage;
    public ReactiveProperty<float> HackDurationPercentage => _hack.HackDurationPercentage;
    public ReactiveProperty<bool> SecondLifeEnabled => _secondLifeEnabled;
    public ReactiveProperty<bool> SecondLifeAvailable => _secondLifeAvailable;
    public ReactiveProperty<bool> SlowFieldAvailable => _slowFieldAvailable;
    public ReactiveProperty<bool> SlowFieldEnabled => _slowFieldEnabled;
    public ReactiveProperty<float> SlowFieldCooldown => _slowFieldCooldown;
    public ReactiveProperty<bool> TeleportAvailable => _teleport.TeleportAvailable;
    public ReactiveProperty<bool> TeleportEnabled => _teleport.TeleportEnabled;
    public ReactiveProperty<float> TeleportCooldownPercentage => _teleport.TeleportCooldownPercentage;
    public ReactiveProperty<float> TeleportDurationPercentage => _teleport.TeleportDurationPercentage;
    public ReactiveProperty<bool> WeaponBoostAvailable => _weaponBoost.BoostingWeaponAvailable;
    public ReactiveProperty<bool> WeaponBoostEnabled => _weaponBoost.WeaponBoostEnabled;
    public ReactiveProperty<float> WeaponBoostCooldown => _weaponBoost.WeaponBoostCooldown;
    public ReactiveProperty<float> WeaponBoostDuration => _weaponBoost.WeaponBoostDuration;

    protected override bool Die()
    {
        if (_secondLifeEnabled.Value  && _secondLifeCharges  > 0)
        {
            _secondLifeCharges--;
            Health.Value = secondLifeConfig.rebirthHealth;
            if (_secondLifeCharges < 1)
            {
                _secondLifeAvailable.Value = false;
            }
            else
            {
                _secondLifeAvailable.Value = true;
            }
            return false;
        }
        return base.Die();
    }

    public bool MoveLock
    {
        get => _moveLock;
        set => _moveLock = value;
    }

    public bool AimLock
    {
        get => _aimLock;
        set => _aimLock = value;
    }

    public bool ShootLock
    {
        get => _shootLock;
        set => _shootLock = value;
    }

    public void Move(Vector3 dir, float speed)
    {
        var distance = dir * (speed * Time.deltaTime);
        _characterController.Move(distance);
    }

    //TODO: Use ResetLevel Method
    //TODO: It is possible that SecondLife only works once a playthrough (which is intended), but also does not reset
    //after dying (which is not); additionally players can probably abuse it by swapping SecondLife after every level
    //Just make available once per level right away? Is there a ResetOnGameOver vs ResetLevel?
    public void ResetLevel()
    {
        _moveLock = true;
        _aimLock = true;
        _shootLock = true;
        _slowFieldAvailable.Value = true;
        _slowFieldCooldown.Value = 1;
        _slowFieldDuration.Value = 1;
        
        StopAllCoroutines();
        _dash.ResetOnNewLevel();
        _hack.ResetOnNewLevel();
        _teleport.ResetOnNewLevel();
        _weaponBoost.ResetOnNewLevel();
        
        _projectileLauncher.Reset();
        controllerInput.Reset();
    }

    public void ResetAfterTutorial()
    {
        dashConfig.ResetTutCondition();
        slowFieldConfig.ResetTutCondition();
        weaponBoostConfig.ResetTutCondition();
    }

    public void StartLevel()
    {
        _moveLock = false;
        _aimLock = false;
        _shootLock = false;
    }

    public void SetPosition(Vector3 position)
    {
        float yOffset = 0.58f;
        Vector3 delta = position - transform.position;
        FindObjectOfType<CinemachineVirtualCamera>().OnTargetObjectWarped(cameraTarget.transform, delta);
        transform.position = position + new Vector3(0,yOffset,0);
    }
}
