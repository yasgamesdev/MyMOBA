using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
    enum PlayerState
    {
        Idle,
        Move,
        Attack,
        Recall,
    }
    PlayerState state = PlayerState.Idle;

    Hero parent;
    NavMeshAgent agent;
    public byte anime { get; private set; }

    //move
    Vector2 groundPos;
    bool moveStart;
    //attack
    Transform target;
    float attackRange;
    float attackRate;
    float realAttackTime;
    float cooldownTime;
    float startMotion;
    float endMotion = 0;
    //recall
    float recallTimer;

    void Start () {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.radius = 0.3f;
        agent.height = 1.6f;
        agent.speed = 3.5f;
        agent.acceleration = 60.0f;

        GameObject.Find("UICanvas").GetComponent<UIScript>().SetPlayer(this);
	}

    public void SetParent(Hero parent)
    {
        this.parent = parent;
        attackRange = parent.GetAttackRange();
        attackRate = parent.GetAttackRate();
        realAttackTime = attackRate * 0.6f;
        cooldownTime = attackRate - realAttackTime;
    }

    void Update()
    {
        endMotion -= Time.deltaTime;

        if (state == PlayerState.Idle)
        {
        }
        else if (state == PlayerState.Move)
        {
            if (moveStart)
            {
                if (agent.remainingDistance != 0)
                {
                    moveStart = false;
                }
            }
            else
            {
                if (agent.remainingDistance == 0)
                {
                    agent.ResetPath();
                    state = PlayerState.Idle;

                    parent.SetIdleAnime();
                    anime = (byte)AnimeState.Idle;
                }
            }
        }
        else if (state == PlayerState.Attack)
        {
            if(this.parent.Distance(target.GetComponent<Parent>().parent) <= attackRange)
            {
                agent.ResetPath();
                parent.SetAttackAnime();
                anime = (byte)AnimeState.Attack;
                
                if(endMotion <= 0)
                {
                    startMotion += Time.deltaTime;
                    if(startMotion >= realAttackTime)
                    {
                        GameObject.Find("Network").GetComponent<NetworkScript>().SendAutoAttack(target.gameObject.GetComponent<Parent>().parent.GetObjID());
                        startMotion = 0;
                        endMotion = cooldownTime;
                    }
                }
            }
            else
            {
                agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));
                parent.SetWalkAnime();
                anime = (byte)AnimeState.Walk;

                startMotion = 0;
            }
        }
        else if(state == PlayerState.Recall)
        {
            recallTimer += Time.deltaTime;
            GameObject.Find("UICanvas").GetComponent<UIScript>().SetRecallSlider(recallTimer);
            if (recallTimer >= 5.0f)
            {
                if (parent.GetFaction() == 0)
                {
                    transform.position = new Vector3(-48.0f, 0, -48.0f);
                    transform.rotation = new Quaternion(0, 0, 0, 1.0f);
                    state = PlayerState.Idle;
                }
                else
                {
                    transform.position = new Vector3(48.0f, 0, 48.0f);
                    transform.rotation = new Quaternion(0, 0, 0, 1.0f);
                    state = PlayerState.Idle;
                }
                GameObject.Find("UICanvas").GetComponent<UIScript>().SetRecallActive(false);
            }
        }
    }

    public void Move(Vector2 groundPos)
    {
        CancelRecall();

        if (Vector2.Distance(groundPos, new Vector2(transform.position.x, transform.position.z)) <= 0.01f)
        {
            return;
        }

        this.groundPos = groundPos;
        transform.LookAt(new Vector3(groundPos.x, transform.position.y, groundPos.y));
        agent.SetDestination(new Vector3(groundPos.x, transform.position.y, groundPos.y));
        moveStart = true;
        state = PlayerState.Move;

        parent.SetWalkAnime();
        anime = (byte)AnimeState.Walk;
    }

    public void Attack(Transform target)
    {
        CancelRecall();

        if (state == PlayerState.Attack && this.target == target)
        {
            return;
        }

        this.target = target;
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        state = PlayerState.Attack;

        startMotion = 0;
    }

    public int GetEnemyLayerMask()
    {
        return parent.GetEnemyLayerMask();
    }

    public void UnSetTarget(Transform target)
    {
        if (state == PlayerState.Attack && this.target == target)
        {
            this.target = null;

            agent.ResetPath();
            state = PlayerState.Idle;

            parent.SetIdleAnime();
            anime = (byte)AnimeState.Idle;
        }
    }

    public void Recall()
    {
        recallTimer = 0;
        state = PlayerState.Recall;

        agent.ResetPath();
        parent.SetIdleAnime();
        anime = (byte)AnimeState.Idle;

        GameObject.Find("UICanvas").GetComponent<UIScript>().SetRecallActive(true);
    }

    public void CancelRecall()
    {
        if(state == PlayerState.Recall)
        {
            state = PlayerState.Idle;

            GameObject.Find("UICanvas").GetComponent<UIScript>().SetRecallActive(false);
        }
    }

    public ushort GetMP()
    {
        return parent.GetMP();
    }
}
