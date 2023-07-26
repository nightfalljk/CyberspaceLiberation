using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Level_Generation;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour, IManager
{
    [SerializeField] private Entity Player;
    [SerializeField] private UIManager UiManager;
    [SerializeField] private AiDirector AiDirector;
    [SerializeField] private Camera Camera;
    [SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera;
    [SerializeField] private LevelGenerator LevelGenerator;
    [SerializeField] private NavmeshBuilder NavmeshBuilder;

    private bool working = false;
    private List<IDisposable> subs = new List<IDisposable>();
    
    private void Awake()
    {
        if(Checkup())
            return;

        Player.GetComponent<PlayerCharacterController>().cam = Camera;
        CinemachineVirtualCamera.Follow = Player.transform;
    }

    void Start()
    {
        UiManager.LinkHealthBarPlayer(Player);
        StartLevel();
    }

    private void Update()
    {
        if (!((KeyControl) Keyboard.current["L"]).wasPressedThisFrame)
            return;
        ResetLevel();
        StartLevel();
    }

    public void ResetLevel()
    {
        foreach (IDisposable sub in subs)
        {
            sub.Dispose();
        }
        subs.Clear();
        UiManager.ResetLevel();
        AiDirector.ResetLevel();
        NavmeshBuilder.ResetLevel();
        LevelGenerator.ResetLevel();
    }

    public void StartLevel()
    {
        if (!working)
        {
            working = true;
            StartCoroutine(CoStartLevel());
        }
    }

    private IEnumerator CoStartLevel()
    {
        //TODO activate UILoading Overlay
        if (LevelGenerator != null)
        {
            LevelGenerator.StartLevel();
            Player.gameObject.transform.position = LevelGenerator.GetPlayerSpawnPoint();
        }

        //Continue on next frame for navmesh generation
        yield return null;
        if (LevelGenerator != null)
            NavmeshBuilder.SetNavmeshSurfaces(LevelGenerator.GetSurfaces());
        NavmeshBuilder.StartLevel();
        
        if (LevelGenerator != null)
            AiDirector.SetEnemieSpawns(LevelGenerator.GetEnemySpawnPoints());
        AiDirector.SetTarget(Player.gameObject);
        AiDirector.StartLevel();
        IDisposable d1 = AiDirector.enemiesLeft.Where(x => x == false).Subscribe(_ =>
        {
            StartCoroutine(LevelFinished());
        });
        subs.Add(d1);
        IDisposable d2 = Player.IsAlive.Where(x => x == false).Subscribe(x =>
        {
            Player.Revive();
            StartCoroutine(LevelFinished());
        });
        subs.Add(d2);
        //TODO activate UILoading Overlay

        UiManager.SetEnemies(AiDirector.GetEnemies());
        UiManager.StartLevel();
        working = false;
    }

    private IEnumerator LevelFinished()
    {
        if (working)
            yield return null;
        //working = true;
        yield return new WaitForSeconds(1);
        ResetLevel();
        StartLevel();
    }
    
    private bool Checkup()
    {
        bool r = false;
        if (Player == null)
        {
            Debug.LogWarning("Player not linked or missing");
            r = true;
        }
        if (UiManager == null)
        {
            Debug.LogWarning("UiManager not linked or missing");
            r = true;
        }
        if (AiDirector == null)
        {
            Debug.LogWarning("AiDirector not linked or missing");
            r = true;
        }
        if (Camera == null)
        {
            Debug.LogWarning("Camera not linked or missing");
            r = true;
        }
        if (CinemachineVirtualCamera == null)
        {
            Debug.LogWarning("CinemachineVirtualCamera not linked or missing");
            r = true;
        }
        if (LevelGenerator == null)
        {
            Debug.LogWarning("LevelGenerator not linked or missing");
            r = true;
        }
        if (NavmeshBuilder == null)
        {
            Debug.LogWarning("NavmeshBuilder not linked or missing");
            r = true;
        }
        return r;
    }
    
}
