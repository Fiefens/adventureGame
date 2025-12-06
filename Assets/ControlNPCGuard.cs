using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPCGuard : MonoBehaviour
{
    public enum GUARD_TYPE { IDLE = 0, PATROLLER = 1, CHASER = 2 };

    public GUARD_TYPE guardType;
    public GameObject player;
    public List<GameObject> wayPoints = new List<GameObject>();

    bool playerActivated;

    private bool hasAttacked = false;
    private int wayPointIndex = 0;
    [HideInInspector] public Animator anim;

    private AnimatorStateInfo info;
    private NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        switch (guardType)
        {
            case GUARD_TYPE.IDLE:
                anim.SetBool("isIdleGuard", true);
                break;
            case GUARD_TYPE.PATROLLER:
                anim.SetBool("isPatrolGuard", true);
                break;
            case GUARD_TYPE.CHASER:
                anim.SetBool("isChaserGuard", true);
                break;
        }
    }

    private bool IsNearPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position) < 2.0f;
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetBool("isWithinAttackingRange", IsNearPlayer());

        if (info.IsName("Patrol"))
        {
            HandlePatrol();
        }
        else if (info.IsName("Chase"))
        {
            HandleChase();
        }
        else if(info.IsName("PaladinAttacking"))
        {
            HandleAttack();
        }
        else
        {
            hasAttacked = false; // Reset when not attacking
        }

        if (info.IsName("Idle")) { if (IsNearPlayer()) SetGuardType(GUARD_TYPE.CHASER); }

        if (!playerActivated)
        {
            player = GameObject.Find("Player"); playerActivated = true;
        }
    }


    void SetGuardType(GUARD_TYPE newType)
    {
        guardType = newType;
        anim.SetBool("isIdleGuard", false);
        anim.SetBool("isPatrolGuard", false);
        anim.SetBool("isChaserGuard", false);

        switch (guardType)
        {
            case GUARD_TYPE.IDLE:
                anim.SetBool("isIdleGuard", true);
                break;
            case GUARD_TYPE.PATROLLER:
                anim.SetBool("isPatrolGuard", true);
                break;
            case GUARD_TYPE.CHASER:
                anim.SetBool("isChaserGuard", true);
                break;
            default:
                anim.SetBool("isIdleGuard", true);
                break;
        }
    }


    bool isVeryNearPlayer() { if (Vector3.Distance(transform.position, player.transform.position) < 1.5f) return true; else return false; }

    private void HandlePatrol()
    {
        if (wayPoints.Count == 0) return;

        agent.isStopped = false;

        if (Vector3.Distance(transform.position, wayPoints[wayPointIndex].transform.position) < 1.5f)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Count)
                wayPointIndex = 0;
        }

        agent.SetDestination(wayPoints[wayPointIndex].transform.position);
    }

    private void HandleChase()
    {
        agent.speed = 2.5f;
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    public void Dies()
    {
        anim.SetTrigger("isDying");
    }

    void HandleAttack()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(targetPosition);
        agent.isStopped = true;

        if (!hasAttacked && info.normalizedTime >= 0.98f && isVeryNearPlayer())
        {
            player.GetComponent<ControlPlayer>().DecreaseHealth(10);
            hasAttacked = true;
        }
    }

}
