using System;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAIAgent
{

    public EnemyBehaviour enemyBehaviour;
    private GameObject hostGameObject;
    private Pathfinding pathfinding;
    public AIState state;
    private PerceptParams percPar;
    private IdleParams idlePar = new();
    private PatrolParams patPar = new();
    private AlertParams alertPar = new();
    private AttackParams attackPar = new();
    private GeneralParams genPar = new();

    private EntityBehaviour.Faction faction;



    public enum AIState
    {
        Idle,
        Patrol,
        Alert,
        Attack,
    }

    public EnemyAIAgent(EnemyBehaviour enemyBehaviour)
    {
        this.enemyBehaviour = enemyBehaviour;
        pathfinding = new Pathfinding(this);
        hostGameObject = this.enemyBehaviour.entityGameObject;

        state = AIState.Idle;
        faction = this.enemyBehaviour.faction;

        percPar = new(enemyBehaviour);
    }


    private class GeneralParams
    {
        public float pathfindingInterwalTime = 0.2f;
        public float pathfindingIntervalCurrTime = 0f;//curr time. keep at 0
        public bool pathfindingIntervalTimerSwitch = false;
    }


    private class PerceptParams
    {
        public float perceptViewRange;
        // public float scanInterval = 0.1f;

        public float memoryTime = 3f;
        public float memoryCurrTime = 0f;//curr time. keep at 0
        public bool memoryTimerSwitch = false;

        //public bool isTargetInSight;
        public List<IHittable> entitiesInVicinity = new();
        public List<IHittable> enemiesInVicinity = new();
        //public List<IHittable> visibleEnemies;
        public List<IHittable> selectedTarget = new();
        public List<IHittable> xraySelectedTarget = new();
        public Vector2? targetPosition;
        public Vector2? targetLastKnownPosition;
        public float attackAbilityRange;

        public PerceptParams(EnemyBehaviour enemyBehaviour)
        {
            perceptViewRange = enemyBehaviour.viewRange;
        }

    }


    private class IdleParams
    {

    }

    private class PatrolParams
    {
        public float patrolRange = 3f;

        public float patrolIntervalTime = 3f;
        public float patrolIntervalCurrTime = 0f;

        public float patrolTime = 20f;
        public float patrolCurrTime = 0f;
        public bool patrolTimerSwitch = false;
    }

    private class AlertParams
    {
        public float approachDistance = 10f;
        public float personalSpace = 1f;
    }

    private class AttackParams
    {

    }


    public void AgentAIUpdate()
    {
        //InputUpdate();
        PathfindingTimer();
        MemoryTimer();

        Percept();
        State();
        //Debug.Log(state);
    }



    
    private void Percept()
    {
        // a new set of data about the environment is created for each frame
        PerceptEnvironment();
        PerceptYourself();

        //Debug.Log("Entities detected: " + percPar.entitiesInVicinity.Count);
        //Debug.Log("Enemies detected: " + percPar.enemiesInVicinity.Count);
    }

    private void PerceptEnvironment()
    {
        // creating a list of entities around a certain radius of the entity
        // IMPORTANT: LISTS ARE NOT SORTED.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(hostGameObject.transform.position, percPar.perceptViewRange);
        percPar.entitiesInVicinity.Clear();
        percPar.enemiesInVicinity.Clear();

        //Debug.Log("Colliders detected: " + colliders.Length);

        foreach (var collider in colliders)
        {
            IHittable hittableScript = collider.gameObject.GetComponentInParent<IHittable>();
            Collider2D thisEntityHitBox = enemyBehaviour.hitBoxObject.GetComponent<Collider2D>();

            // add all entities in vicinity to the list
            // no need to check if thisEntityHitBox exists as it must exist if its used by AI
            if (hittableScript != null && collider.gameObject.layer == LayerMask.NameToLayer("HitBox") && collider != thisEntityHitBox)
            {
                percPar.entitiesInVicinity.Add(hittableScript);

                // add entities which are enemies to enemy list
                if (hittableScript.IsEnemyFaction(faction))
                {
                    percPar.enemiesInVicinity.Add(hittableScript);
                }
            }
        }
    }

    public void PerceptYourself()
    {
        if (enemyBehaviour.selectedAbility != null)
        {
            percPar.attackAbilityRange = enemyBehaviour.selectedAbility.baseStats.attackRange;
        }
    }


    // send a ray towards each enemy to check if they are visible
    private RaycastHit2D[] VisibilityScan(GameObject target)
    {
        Vector2 startPoint = hostGameObject.transform.position;
        Vector2 endPoint = target.transform.position;
        Vector2 direction = endPoint - startPoint;

        RaycastHit2D[] hits = Physics2D.RaycastAll(startPoint, direction, direction.magnitude);

        //Debug.Log("VisibilityScan hits: " + hits.Length);

        return hits; 
    }


    // decide whether this return all visible enemies or let it decide which one is closest and set it as enemyTarget
    private void LookForEnemy()
    {
        percPar.selectedTarget.Clear();
        percPar.targetPosition = null;

        // if there are enemies withing a visibility DISTANCE (no matter if they are visible)
        if (percPar.enemiesInVicinity.Count > 0)
        {
            List<IHittable> visibleEnemies = new List<IHittable>();

            // out of enemies in vicinity filter ones that are visible.
            foreach (IHittable enemy in percPar.enemiesInVicinity)
            {
                bool isVisible = true;
                RaycastHit2D[] hits = VisibilityScan(enemy.entityGameObject);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        isVisible = false;
                        break;
                    }
                }

                if (isVisible)
                {
                    visibleEnemies.Add(enemy);
                }

            }

            // MODIFY IT TO ADD THE CLOSES TARGET INSTEAD OF FIRST ONE IN THE LIST
            // out of visible enemies choose one that will become a target
            if (visibleEnemies.Count > 0)
            {
                percPar.selectedTarget.Add(visibleEnemies[0]);
            }

            // if percPar.selectedTarget has a target add it to a memory list and reset the timer
            if (percPar.selectedTarget.Count > 0)
            {
                percPar.xraySelectedTarget.Clear();
                percPar.xraySelectedTarget.Add(percPar.selectedTarget[0]);
                percPar.memoryCurrTime = 0f;
                percPar.memoryTimerSwitch = true;
            }


            if (percPar.xraySelectedTarget.Count > 0)
            {
                Vector2? position = percPar.xraySelectedTarget[0].entityGameObject.transform.position;
                percPar.targetPosition = position;
                // THE RESPONSIBILITY FOR CLEARING LAST KNOWN POSITION IS UP TO THE STATE MACHINE
                percPar.targetLastKnownPosition = position;
            }

            //Debug.Log("Target position: " + percPar.targetPosition);
            //Debug.Log("Target last known position: " + percPar.targetLastKnownPosition);

            // general all purpose targeting. Specific targeting can be overriden in states
            SetAimTarget(percPar.targetPosition);
        }
    }

    private void SetAimTarget(Vector2? targetLocation)
    {
        enemyBehaviour.SetAimTarget(targetLocation);
    }


    private void State()
    {
        switch (state)
        {
            case AIState.Idle:
                IdleBehaviour();
                break;

            case AIState.Patrol:
                PatrolBehaviour();
                break;

            case AIState.Alert:
                AlertBehaviour();
                break;

            case AIState.Attack:
                AttackBehaviour();
                break;

            default:
                // code block
                break;
        }

    }


    private void IdleBehaviour()
    {
        LookForEnemy();

        // if target in sight
        if (percPar.selectedTarget.Count > 0)
        {
            // if target within approach distance
            float distanceToTarget = Distance(hostGameObject.transform.position, percPar.targetPosition);
            //Debug.Log("Distance to target: " + distanceToTarget);
            if (distanceToTarget <= alertPar.approachDistance)
            {
                state = AIState.Alert;
            }
        }
    }

    private void AlertBehaviour()
    {
        LookForEnemy();


        // if target in sight
        if (percPar.selectedTarget.Count > 0)
        {
            // if target within approach distance
            float distanceToTarget = Distance(hostGameObject.transform.position, percPar.targetPosition);
            //Debug.Log("Distance to target: " + distanceToTarget);
            if (distanceToTarget <= alertPar.approachDistance)
            {
                // if target within attack range
                if (distanceToTarget <= (percPar.attackAbilityRange * 0.9))
                {
                    StopMovement();
                    state = AIState.Attack;
                }
                else
                {
                    SetPathfindingPath(percPar.targetPosition);
                }
            }
        }

        // if target not in sight
        else if (percPar.selectedTarget.Count == 0 && percPar.targetLastKnownPosition != null)
        {
            // if last known position within approach range
            float distanceToTarget = Distance(hostGameObject.transform.position, percPar.targetLastKnownPosition);
            //Debug.Log("Distance to last known position: " + distanceToTarget);
            if (distanceToTarget <= alertPar.approachDistance)
            {
                Vector2? start = hostGameObject.transform.position;
                Vector2? end = percPar.targetLastKnownPosition;

                if (Distance(start, end) <= alertPar.personalSpace)
                {
                    percPar.targetLastKnownPosition = null;
                }

                else
                {
                    SetPathfindingPath(percPar.targetLastKnownPosition);
                }
            }

            else
            {
                percPar.targetLastKnownPosition = null;
                state = AIState.Patrol;
            }
        }


        else
        {
            state = AIState.Patrol;
        }
    }

    private void PatrolBehaviour()
    {
        LookForEnemy();

        patPar.patrolTimerSwitch = true;
        PatrolTimer();

        // if target in sight
        if (percPar.selectedTarget.Count > 0)
        {
            // if target within approach distance
            float distanceToTarget = Distance(hostGameObject.transform.position, percPar.targetPosition);
            //Debug.Log("Distance to target: " + distanceToTarget);
            if (distanceToTarget <= alertPar.approachDistance)
            {
                patPar.patrolCurrTime = 0;
                patPar.patrolTimerSwitch = false;
                state = AIState.Alert;
            }
        }


        else if (patPar.patrolTimerSwitch == true)
        {
            patPar.patrolIntervalCurrTime += Time.deltaTime;
            if (patPar.patrolIntervalCurrTime >= patPar.patrolIntervalTime)
            {
                patPar.patrolIntervalCurrTime = 0f;
                SetDirectPath(RandomPointInCircle(hostGameObject.transform.position.x, hostGameObject.transform.position.y, patPar.patrolRange));
            }   
        }

        else
        {
            patPar.patrolCurrTime = 0;
            patPar.patrolTimerSwitch = false;
            state = AIState.Idle;
        }

    }


    private void AttackBehaviour()
    {
        LookForEnemy();

        // if target in sight
        if (percPar.selectedTarget.Count > 0)
        {
            float distanceToTarget = Distance(hostGameObject.transform.position, percPar.targetPosition);
            //Debug.Log("Distance to target: " + distanceToTarget);
            // if target within weapon range
            if (distanceToTarget <= percPar.attackAbilityRange)
            {
                StopMovement();
                enemyBehaviour.isAttack = true;
            }

            // otherwise let it dont interrupt cast but change state to alert
            else
            {
                enemyBehaviour.isAbilityInterrupt = true;
                state = AIState.Alert;
            }
        }

        // if target not in sight or not existing interrupt cast and return to alert state
        else if (percPar.selectedTarget.Count == 0)
        {
            enemyBehaviour.isAbilityInterrupt = true;
            state = AIState.Alert;
        }

        // in any other case
        else
        {
            enemyBehaviour.isAbilityInterrupt = true;
            state = AIState.Alert;
        }
    }

    private void SetDirectPath(Vector2? goal)
    {
        if (goal.HasValue)
        {
            Queue<Vector2> path = new Queue<Vector2>();
            path.Enqueue((Vector2)goal);
            enemyBehaviour.ReplaceQueue(path);
        }
        else
        {
            Debug.LogError("Goal for direct move is null");
        }
    }

    private void SetPathfindingPath(Vector2? goal)
    {
        if (genPar.pathfindingIntervalTimerSwitch == false)
        {
            if (goal.HasValue)
            {
                //Debug.Log("Pathfinding goal: " + goal);
                Queue<Vector2> path = pathfinding.CreatePath((Vector2)goal);
                enemyBehaviour.ReplaceQueue(path);

            }
            else
            {
                Debug.LogError("Goal for pathfinding is null");
            }
        }
        genPar.pathfindingIntervalTimerSwitch = true;
    }

    private void StopMovement()
    {
        SetDirectPath(hostGameObject.transform.position);
    }

    private float Distance(Vector2? start, Vector2? end)
    {
        if (start.HasValue && end.HasValue)
        {
            Vector2 direction = (Vector2)end - (Vector2)start;
            return direction.magnitude;
        }

        else
        {
            Debug.LogError("One of parameters in distance measurement is null");
            return 0f;
        }
    }

    private void PathfindingTimer()
    {
        if (genPar.pathfindingIntervalTimerSwitch)
        {
            genPar.pathfindingIntervalCurrTime += Time.deltaTime;
            if (genPar.pathfindingIntervalCurrTime >= genPar.pathfindingInterwalTime)
            {
                genPar.pathfindingIntervalCurrTime = 0f;
                genPar.pathfindingIntervalTimerSwitch = false;
                //Debug.Log("Ready");
            }
        }
    }

    public void MemoryTimer()
    {
        if (percPar.memoryTimerSwitch)
        {
            percPar.memoryCurrTime += Time.deltaTime;
            if (percPar.memoryCurrTime >= percPar.memoryTime)
            {
                percPar.memoryCurrTime = 0f;
                percPar.memoryTimerSwitch = false;
                percPar.xraySelectedTarget.Clear();
            }
        }
    }


    private void PatrolTimer()
    {
        if (patPar.patrolTimerSwitch)
        {
            patPar.patrolCurrTime += Time.deltaTime;
            if (patPar.patrolCurrTime >= patPar.patrolTime)
            {
                patPar.patrolCurrTime = 0f;
                patPar.patrolTimerSwitch = false;
            }
        }
    }


    // TEMPORARY FOR TESTING
    public void InputUpdate()
    {
        if (GameMaster.mouseLeftDown)
        {
            Queue<Vector2> path = new Queue<Vector2>();
            // enqueue mouse coordinates of a click to move to directly
            path.Enqueue(GameMaster.mouseScreenToWorld);

            enemyBehaviour.ReplaceQueue(path);
        }

        else if (GameMaster.mouseRightDown)
        {

            Queue<Vector2> path = pathfinding.CreatePath(GameMaster.mouseScreenToWorld);

            enemyBehaviour.ReplaceQueue(path);
        }
    }



    //public static Random random = new Random();

    public Vector2 RandomPointInCircle(float originX, float originY, float radius)
    {
        float angle = UnityEngine.Random.Range(0f, 1f) * Mathf.PI * 2;
        float distance = UnityEngine.Random.Range(0f, 1f) * radius;
        float x = (Mathf.Cos(angle) * distance) + originX;
        float y = (Mathf.Sin(angle) * distance) + originY;
        return new Vector2(x, y);
    }

}
 