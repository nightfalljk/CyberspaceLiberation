using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class AiDirector : MonoBehaviour, IManager
{
    [SerializeField] private List<EnemyBehaviour> preSpawnEnemies = new List<EnemyBehaviour>();
    
    private List<Entity> enemies = new List<Entity>();
    //[SerializeField] private BehaviourTree behaviourTree;
    [SerializeField] private List<Vector3> spawnPoints;

    //[SerializeField] private int maxEnemies = 4;
    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject target;

    [SerializeField] private DifficultyHolder difficultyHolder;
    public AiConfig aiConfig1;
    public AiConfig aiConfig2;
    public AiConfig aiConfig;
    [SerializeField] private List<BehaviourTree> behaviourTrees;
    
    public ReactiveProperty<bool> enemiesLeft;
    private ReactiveProperty<int> enemiesAlive = new ReactiveProperty<int>(0);
    private ReactiveProperty<int> armcounter = new ReactiveProperty<int>(0);
    private bool active = false;
    private List<IDisposable> subs = new List<IDisposable>();

    private Random rnG;
    private int currentLevel;
    //private List<EnemyBehaviour> prefabEntities;
    //private int spawnPointCounter = 0;
    //private Dictionary<string, BehaviourTree> enemyBTMapping;
    private bool spawnEnemies = true;

    [SerializeField] public bool isFinalLevel;
    [SerializeField] private Vector3 bossSpawnPoint;
    //[SerializeField] private GameObject bossPrefab;

    [SerializeField] private MaterialSwitcher materialSwitcher;
    [SerializeField] private string bossKillMusic;
    
    List<SpawnPoint> shuffledSpawnPoints = new List<SpawnPoint>();
    //int spawnPointCounterTmp = 0;
    
    private void Awake()
    {
        aiConfig = difficultyHolder.selectedDifficulty == 1 ? aiConfig2 : aiConfig1;
    }

    private void Update()
    {
        SetLevelPercentage(CalculateEnemyLifesPercentage());
    }

    public void ResetLevel()
    {
        foreach (IDisposable sub in subs)
        {
            sub.Dispose();
        }
        subs.Clear();
        //throw new System.NotImplementedException();
        active = false;
        enemiesAlive.Value = 0;
        foreach (BehaviourTree tree in behaviourTrees)
        {
            tree.ResetLevel();
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
    }

    public void StartLevel()
    {
        if (rnG == null)
        {
            rnG = new Random();
        }
        SetUpEnemies();//moved from SpawnEnemies // moved from start
        
        IDisposable d = enemiesAlive.Subscribe(a => Debug.Log("alive: " + a));
        subs.Add(d);
        foreach (Vector3 spawnPoint in spawnPoints)
        {
            shuffledSpawnPoints.Add(new SpawnPoint(){available = true, position = spawnPoint});
        }
        rnG.Shuffle(shuffledSpawnPoints);
        if (spawnEnemies)
        {
            SpawnEnemies();
        }
        else
        {
            IntegrateAllPreSpawnEnemies();
        }
        foreach (BehaviourTree tree in behaviourTrees)
        {
            //tree.FillBlackboard();
            tree.StartLevel();
        }
        active = true;
    }

    private void SetUpEnemies()
    {
        //prefabEntities = new List<EnemyBehaviour>();
        foreach (GameObject p in aiConfig.enemyPrefabs)
        {
            //prefabEntities.Add(p.GetComponent<EnemyBehaviour>());
        }
        //prefabEntities.Add(aiConfig.bossPrefab.GetComponent<EnemyBehaviour>());
        //match BTs
//        enemyBTMapping = new Dictionary<string, BehaviourTree>();
//        foreach (EnemyBehaviour enemyBehaviour in prefabEntities)
//        {
//            foreach (BehaviourTree tree in behaviourTrees)
//            {
//                if (enemyBehaviour.btGraph == tree.BTXNodeGraphs)
//                {
//                    enemyBTMapping.Add(enemyBehaviour.gameObject.name, tree);   
//                }
//            }
//        }
        //TODO check if matching worked?
//        foreach (BehaviourTree tree in behaviourTrees)
//        {
//            
//        }
    }

    private List<GameObject> PrepareEntityList(int tokensLeft)
    {
        if (currentLevel == 0)
        {
            List<GameObject> l = new List<GameObject>();
            l.Add(aiConfig.enemyPrefabs[0]);
            return l;
        }
        else
        {
            return aiConfig.enemyPrefabs;
        }
    }
    
    private void SpawnEnemies()
    {
        int spawnTokensLeft = aiConfig.tokens[currentLevel];
        
        if (isFinalLevel)
        {
            EnemyBehaviour e = SpawnBoss();
            spawnTokensLeft -= e.GetStages()[0];
        }
        else
        {
           SpawnEnemies(spawnTokensLeft, false);
        }
        
        Debug.Log($"{-spawnTokensLeft} over available tokens");
        
        IDisposable d2 = enemiesAlive.Where(x => x == 0).Subscribe(_=>
        {
            if (active)
            {
                enemiesLeft.Value = false;
                Debug.Log("No enemies Left!");
                CustomEvent.Trigger(this.gameObject, "noEnemiesLeft");
            }
        });
        subs.Add(d2);
        enemiesLeft.Value = true;
    }

    private void SpawnEnemies(int tokens, bool postLoad)
    {
        activeSpawning = true;
        int spawnTokensLeft = tokens;
        //spawnPointCounter = 0;
        //Select Entitys
        while (spawnTokensLeft > 0)
        {
            //TODO overrides?
            List<GameObject> tmpList = PrepareEntityList(spawnTokensLeft);
            if (tmpList.Count == 0)
                break;
                
            double sigmaType = 1d;
            int enemyTypeCount = tmpList.Count;
            float gauss = rnG.NextGaussianReroll(0, enemyTypeCount, aiConfig.easyVsHardType * enemyTypeCount, sigmaType);
            int selectedType = (int) (gauss);//TODO check if correct
            EnemyBehaviour selectedBaseConfig = tmpList[selectedType].GetComponent<EnemyBehaviour>();
            List<int> stages = selectedBaseConfig.GetStages();
            int selectedStage = 0;
            ProcedualHelper.SelectStage(stages.Count, aiConfig.amountVsAbility, out selectedStage, rnG);
            spawnTokensLeft -= stages[selectedStage];
            //Spawn Entity
//            if (spawnPointCounter >= spawnPoints.Count)
//            {
//                Debug.LogWarning("Not enough spawnpoints");
//                break;
//            }
            //GameObject g = Instantiate(tmpList[selectedType], spawnPoints[spawnPointCounter], Quaternion.identity);
            GameObject g = SpawnEntity(tmpList[selectedType], false);
            if (g == null)
            {
                break;
            }
            //g.name = tmpList[selectedType].name + "_"+spawnPointCounter;
            EnemyBehaviour e = g.GetComponent<EnemyBehaviour>();
            e.SetStage(selectedStage);
            //e.type_prefab = tmpList[selectedType].name;
            IntegrateEnemy(e, postLoad);
            Debug.Log($"Spawned enemy {g.name} with stage {selectedStage} and cost {stages[selectedStage]}");
            
            //spawnPointCounter++;
        }

        activeSpawning = false;
    }

    private GameObject SpawnEntity(GameObject prefab, bool takeReserved)
    {
        int index = GetNextFreeSpawnPoint(takeReserved);
        if (index == -1)
        {
            Debug.LogWarning("Not enough spawnpoints");
            return null;
        }

        Vector3 sp = shuffledSpawnPoints[index].position;
        shuffledSpawnPoints[index].available = false;
        GameObject g = Instantiate(prefab, sp, Quaternion.identity);
        IDisposable d5 = g.GetComponent<EnemyBehaviour>().IsAlive.Subscribe(b =>
        {
            if (!b && !activeSpawning)
            {
                shuffledSpawnPoints[index].available = true;
            } 
        });
        subs.Add(d5);
        g.name = prefab.name + index;//spawnPointCounterTmp;
        return g;
    }

    private BossBehaviour bossBehaviour;
    private EnemyBehaviour SpawnBoss()
    {
        activeSpawning = true;
        GameObject g = Instantiate(aiConfig.bossPrefab, bossSpawnPoint, Quaternion.identity);
        EnemyBehaviour e = g.GetComponent<EnemyBehaviour>();
        bossBehaviour = e as BossBehaviour;
        bossBehaviour.ConectAiDirector(this);
        IntegrateEnemy(e);
        IDisposable d3 = armcounter.Where(x => x == 0).Subscribe(_=>bossBehaviour.AllArmsDead());
        subs.Add(d3);
        IDisposable d6 = bossBehaviour.IsAlive.Subscribe(b =>
        {
            if (!b && !activeSpawning)
            {
                BossDead();
            }
        });
        subs.Add(d6);
        activeSpawning = false;
        return e;
    }

    private void BossDead()
    {
        KillAllEnemies();
        AudioManager.Instance.StopAllSounds(true,false);
        AudioManager.Instance.Play(bossKillMusic,smooth:true);
    }

    private void KillAllEnemies()
    {
        foreach (Entity entity in enemies)
        {
            entity.Kill();
        }
    }

    private int GetNextFreeSpawnPoint(bool takeReserved)
    {
        int availableCounter = 0;
        foreach (SpawnPoint point in shuffledSpawnPoints)
        {
            availableCounter += point.available ? 1 : 0;
        }

        if (takeReserved || availableCounter - 1 >= aiConfig.amountArms)
        {
            for (var i = 0; i < shuffledSpawnPoints.Count; i++)
            {
                SpawnPoint point = shuffledSpawnPoints[i];
                if (point.available)
                {
                    return i;
                }
            }
        }
        

        return -1;
    }
    
    //private List<bool> occupiedArmSpawn = new List<bool>();
    public void SpawnArms(GameObject prefab)
    {
        //bool tmpActive = false;
        activeSpawning = true;
        int amount = aiConfig.amountArms;
        //List<Vector3> shuffledSpawnPoints = new List<Vector3>(spawnPoints);
        
        for (int i = 0; i < amount; i++)
        {
            
            GameObject g = SpawnEntity(prefab, true);
            if(g==null)
                break;
            
            EnemyBehaviour e = g.GetComponent<EnemyBehaviour>();
            //active = false;
            IntegrateEnemy(e, true);
            IDisposable d4 = e.IsAlive.Subscribe(b =>
            {
                if(activeSpawning) return;
                if (b) armcounter.Value++;
                else armcounter.Value--;
            });
            subs.Add(d4);
            
            //active = true;
        }

        activeSpawning = false;

    }

    private void IntegrateEnemy(EnemyBehaviour e, bool postLoad = false)
    {
        IDisposable d1 = e.IsAlive.Subscribe(b =>
        {
            if(activeSpawning) return;
            if (b) enemiesAlive.Value++;
            else enemiesAlive.Value--;
        });
        subs.Add(d1);
        e.SetTarget(target);
        //enemyBTMapping[e.type_prefab].EnemyBehaviours.Add(e);
        bool found = false;
        foreach (BehaviourTree tree in behaviourTrees)
        {
            if (tree.BTXNodeGraphs.name == e.btGraph.name)
            {
                if (postLoad)
                {
                    tree.AddBehaviour(e,new List<GameObject>(), true);
                    tree.StartBehaviour(e,new WaitForSeconds(0));
                }
                else
                {
                    tree.EnemyBehaviours.Add(e);
                }
                found = true;
            }
        }

        if (!found)
        {
            Debug.Log("Could not find corresponding BT of: "+e.btGraph.name);
        }
        
        enemies.Add(e);
        if (postLoad)
        {
            FindObjectOfType<UIManager>().AddEnemy(e);
        }
    }

    public void IntegrateAllPreSpawnEnemies()
    {
        foreach (EnemyBehaviour enemyBehaviour in preSpawnEnemies)
        {
            IntegrateEnemy(enemyBehaviour);
        }
    }
    
    public List<Entity> GetEnemies(bool alive = false)
    {
        List<Entity> entities = new List<Entity>();
        if (alive)
        {
            foreach (Entity entity in enemies)
            {
                if (entity.IsAlive.Value)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }
        return enemies;
    }

    public void SetBossSpawn(Vector3 t)
    {
        bossSpawnPoint = t;
    }
    
    public void SetEnemieSpawns(List<Vector3> points)
    {
        if (points == null)
            return;
        spawnPoints = points;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetSpawnEnemies(bool b)
    {
        spawnEnemies = b;
    }
    
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 0,aiConfig.tokens.Count-1);
        if (level > currentLevel)
        {
            Debug.LogWarning("No more tokens defined for level "+ currentLevel +$" and above. (requesting level {level}");
        }
    }

    private int oldLayer;
    private bool activeSpawning;

    public void HackEnemy(GameObject hackedEnemy)
    {
        //Do all the stuff so that hacked enemy targets target enemy
        //Bullets in Projectile Launcher already set to playerbullets
        EnemyBehaviour hb = hackedEnemy.GetComponent<EnemyBehaviour>();
        oldLayer = hackedEnemy.layer;
        hackedEnemy.layer = target.layer;
        hb._collider.gameObject.layer = target.layer;
        //hb.SetTarget(targetEnemy);
        hb.Hacked.Value = true;
        Coroutine ch = StartCoroutine(CoGetTarget(hb));
    }

    public void ResetHack(GameObject hackedEnemy)
    {
        //UNdo all the stuff so that hacked enemy targets player again
        EnemyBehaviour hb = hackedEnemy.GetComponent<EnemyBehaviour>();
        hackedEnemy.layer = oldLayer;
        hb._collider.gameObject.layer = oldLayer;
        hb.SetTarget(target);
        hb.Hacked.Value = false;
    }

    public void SetRandomPoints(List<Vector3> points)
    {
        foreach (BehaviourTree tree in behaviourTrees)
        {
            tree.CreateBlackboard();
            tree.SetRandomPoints(points);
        }
    }

    public bool IsLastLevel(int level)
    {
        if (level == aiConfig.tokens.Count - 1)
        {
            isFinalLevel = true;
        }

        return isFinalLevel;
    }

    private void SetLevelPercentage(float p)
    {
        if(materialSwitcher== null)
            return;
        materialSwitcher.SetSwitch(p);
    }
    
    private float CalculateEnemyLifesPercentage()
    {
        float health = 0;
        float max = 0;
        if (isFinalLevel)
        {
            if (bossBehaviour == null)
            {
                return 0;
            }
            health = bossBehaviour.Health.Value;
            max = bossBehaviour.GetMaxHealth();
        }
        else
        {
            foreach (Entity entity in enemies)
            {
                health += Mathf.Clamp(entity.Health.Value,0,entity.Health.Value);
                max += entity.GetMaxHealth();
            }
        }
        

        if (max == 0)
            return 0;
        return 1 - (health / max);
    }

//    public void SlowEnemy(EnemyBehaviour eb, bool b, float amount)
//    {
//        eb.SlowEnemy(b, amount);
//    }

    public IEnumerator CoGetTarget(EnemyBehaviour hacked)
    {
        while (hacked.Hacked.Value)
        {
            GameObject eb = GetClosest(hacked)?.gameObject;
            hacked.SetTarget(eb);
            //Debug.Log($"Target of {hacked.name} is {eb}");
            yield return new WaitForSeconds(0.5f);
        }
    }

    public EnemyBehaviour GetClosest(EnemyBehaviour hacked)
    {
        List<Entity> potentialTargets = GetEnemies(true);
        EnemyBehaviour closestTarget = null;
        float closestDist = -1;
        for (int i = 0; i < potentialTargets.Count; i++)
        {
            var hackedPos = hacked.gameObject.transform.position;
            var targetPos = potentialTargets[i].gameObject.transform.position;
            if (targetPos == hackedPos)
            {
                continue;
            }

            var dist = (hackedPos - targetPos).magnitude;
            if (closestDist < 0)
            {
                closestDist = dist;
            }
            
            if (dist <= closestDist)
            {
                closestDist = dist;
                closestTarget = potentialTargets[i] as EnemyBehaviour;
            }
        }

        return closestTarget;
    }

    public void SpawnMinions()
    {
        int tokens = aiConfig.amountMinionTokens;
        SpawnEnemies(tokens, true);
    }
}

public class SpawnPoint
{
    public bool available;
    public Vector3 position;
}