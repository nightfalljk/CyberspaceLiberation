using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using XNode;

public class BehaviourTree : MonoBehaviour, IManager
{
    private BTNode mRoot;
    private bool startedBehaviour;
    private List<Coroutine> runningBehaviours;

    //[FormerlySerializedAs("NodeGraph")] public XNodeGraph XNodeGraph;
    public XNodeGraph BTXNodeGraphs;
    //public NavMeshAgent NavMeshAgent;
    public List<EnemyBehaviour> EnemyBehaviours;
    
    public bool UpdateLiveValues = true;
    public Dictionary<string, object> Blackboard { get; set; }
    public Dictionary<string, BTNode.Result> NodeStates { get; set; }

    public bool startInStart = false;
    private bool treeBuilt;

    //BTNode.Result result = BTNode.Result.Running;
    
    public BTNode Root
    {
        get { return mRoot; }
    }

    private void Start()
    {
        if(startInStart)
            StartLevel();
    }

    public void CreateBlackboard()
    {
        NodeStates = new Dictionary<string, BTNode.Result>();
        Blackboard = new Dictionary<string, object>();
        runningBehaviours = new List<Coroutine>();
    }

    private void FillBlackboard()
    {
        List<GameObject> spheres = new List<GameObject>(); //TODO trak speheres different -> Sensor?
        Blackboard.Add(ReqString(0,"timeSinceStart"), Time.timeSinceLevelLoad);
        foreach (EnemyBehaviour enemyBehaviour in EnemyBehaviours)
        {
            AddBehaviour(enemyBehaviour, spheres);
        }

        SetBBValue(0, "Spheres", spheres);
    }

    public void AddBehaviour(EnemyBehaviour enemyBehaviour, List<GameObject> spheres, bool addToList=false)
    {
        //Blackboard.Add(ReqString(enemyBehaviour,"f.target"), (Func<GameObject>)(enemyBehaviour.GetTarget));
        //Blackboard.Add(ReqString(enemyBehaviour,"f.MoveTo"), (Func<Vector3,bool>)(enemyBehaviour.gameObject.GetComponent<DirectAgent>().MoveToLocation));
        int instanceId = enemyBehaviour.GetInstanceID();
        Blackboard.Add(ReqString(enemyBehaviour,"NavMeshAgent"), enemyBehaviour.gameObject.GetComponent<NavMeshAgent>());
        SetBBValue(instanceId, "This", enemyBehaviour.gameObject);
        spheres.Add(enemyBehaviour.gameObject);
        Dictionary<string, object> functions = enemyBehaviour.GetFunctions();
        foreach (KeyValuePair<string,object> keyValuePair in functions)
        {
            Blackboard.Add(ReqString(enemyBehaviour,keyValuePair.Key),keyValuePair.Value);
        }
        LoadParametersToBlackboard(enemyBehaviour);
        if(addToList)
            EnemyBehaviours.Add(enemyBehaviour);
    }
    

    public void LoadParametersToBlackboard(Entity entity)
    {
        Dictionary<string, object> parameters = entity.GetParameters();
        foreach (KeyValuePair<string,object> keyValuePair in parameters)
        {
            Blackboard[ReqString(entity, keyValuePair.Key)] = keyValuePair.Value;
        }
    }
    
    private void Update()
    {
        if (!startedBehaviour)
            return;
        UpdateSensors();
    }

    //Consider adding action instead in Start
    private void UpdateSensors()
    {
        Blackboard[ReqString(0,"timeSinceStart")] = Time.timeSinceLevelLoad;
        Blackboard[ReqString(0,"deltaTime")] = Time.deltaTime;
        foreach (EnemyBehaviour enemyBehaviour in EnemyBehaviours)
        {
            //Blackboard.Add($"{enemyBehaviour.GetInstanceID()}.a.target.visible", enemyBehaviour.GetTargetVisible());
            SetBBValue(enemyBehaviour.GetInstanceID(), "targetPosition", enemyBehaviour.GetTarget().transform.position);
            SetBBValue(enemyBehaviour.GetInstanceID(), "stage", enemyBehaviour.GetStage());
            SetBBValue(enemyBehaviour.GetInstanceID(), "shootCooldown", enemyBehaviour.GetWaitBetweenShots());
        }
    }

    private IEnumerator RunBehaviour(int instanceID, WaitForSeconds waitForSeconds)
    {
        bool active = true; 
        GetBBValue(instanceID, "active", out active);
        while (active)
        {
            yield return null;
            //SetStateValue(instanceID, )
            //NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  Root.Execute(instanceID);
            Root.Execute(instanceID);
        }
    }

    public BTNode BuildTreeFromNodeGraph()
    {
        BTXNodeGraphs.bt = this;
        XNodeBtRoot xNodeRoot = BTXNodeGraphs.GetRoot();

        if (xNodeRoot == null)
        {
            Debug.Log("No root found for building BT");
            return null;
        }
        BTNode b = xNodeRoot.GetBtNode(this);
        return b;
    }

    public bool GetBBValue<T>(int instanceID, string request, out T result, bool supresswarning = false)
    {
        bool success = false;
        result = default(T);
        object o;
        success = Blackboard.TryGetValue(ReqString(instanceID, request), out o);
        if (success)
        {
            if (o is T)
            {
                result = (T)o ;
                return true;
            }

            try
            {
                result = (T)Convert.ChangeType(o, typeof(T));
                return true;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Debug.LogWarning($"Casting from Blackboard with type {typeof(T)} and request {request} not possible");
                //throw;
            }
        }
        else
        {
            if (!supresswarning)
            {
                Debug.LogWarning($"No {request} from id {instanceID} could be found");
            }
        }

        return false;
    }

    public bool SetBBValue<T>(int instanceID, string request, T value)
    {
        Blackboard[ReqString(instanceID, request)] = value;
        return true;
    }
    
    public bool GetStateValue(int instanceID, BTNode node, out BTNode.Result result)
    {
        result = BTNode.Result.Inactive;
        if (instanceID == 0)
            return false;
        string s = ReqString(instanceID, node);
        if (NodeStates.TryGetValue(s, out result))
        {
            return true;
        }
        else
        {
            result = BTNode.Result.Inactive;
            //Debug.LogWarning($"Node {node.GetHashCode()} from id {instanceID} could not be found");
        }

        return false;
    }

    public bool SetStateValue(int instanceId, BTNode node, BTNode.Result result)
    {
        NodeStates[ReqString(instanceId, node)] = result;
        return true;
    }
    
    
    public string ReqString(Entity entity, string s)
    {
        //return new Tuple<int, string>(eb.GetInstanceID(),s);
        return entity.GetInstanceID() + "." + s;
    }

    public string ReqString(int ebInstanceID, BTNode btNode)
    {
        return btNode.GetHashCode() + "."+ebInstanceID;
    }
    
    public string ReqString(int ebInstanceID, string s)
    {
        //return new Tuple<int, string>(ebInstanceID,s);
        return ebInstanceID + "." + s;
    }

    public void ResetLevel()
    {
        if(!startedBehaviour)
            return;
        foreach (Coroutine coroutine in runningBehaviours)
        {
            StopCoroutine(coroutine);
        }
        Blackboard.Clear();
        NodeStates.Clear();
        EnemyBehaviours.Clear();
        runningBehaviours.Clear();
        startedBehaviour = false;
//        if (mRoot != null)
//        {
//            mRoot.Tree = null;
//        }
//
//        mRoot = null;
//        treeBuilt = false;
    }

    public void StartLevel()
    {
        if (startInStart)
        {
            CreateBlackboard();
        }
        FillBlackboard();
        if (!treeBuilt)
        {
            mRoot = BuildTreeFromNodeGraph();
            treeBuilt = true;
        }

        XNodeBtRoot xNodeRoot = BTXNodeGraphs.GetRoot();
        xNodeRoot.UpdateEntity();
        
        startedBehaviour = false;
        if (!startedBehaviour)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
            foreach (EnemyBehaviour enemyBehaviour in EnemyBehaviours)
            {
               StartBehaviour(enemyBehaviour, waitForSeconds);
            }
            startedBehaviour = true;
        }
    }

    public void StartBehaviour(EnemyBehaviour enemyBehaviour, WaitForSeconds waitForSeconds)
    {
        int instanceId = enemyBehaviour.GetInstanceID();
        SetBBValue(instanceId, "active", enemyBehaviour.gameObject.activeSelf);
        Coroutine c = StartCoroutine(RunBehaviour(instanceId, waitForSeconds));
        runningBehaviours.Add(c);
    }
    
    public void SetRandomPoints(List<Vector3> points)
    {
        SetBBValue(0, "randomPoints", points);
    }
}