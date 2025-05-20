using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,  // ���Ƿ� ��ǥ ���� ��� �ڵ����� �����̰� ���ִ� �κ�
    Attacking
}

public class NPC : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent _agent;
    public float detectDistance;
    private AIState _aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public float damage;
    public float attackRate;
    private float _lastAttackTime;
    public float attackDistance;

    private float _playerDistance;

    public float fieldOfView = 120f;

    private Animator _animator;
    private SkinnedMeshRenderer[] _meshRenderers;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        _playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        _animator.SetBool("Moving", _aiState != AIState.Idle);

        switch (_aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState state)  // ���� �����ϱ�
    {
        _aiState = state;  // �Ű������� ���� AIState�� ���¸� ����

        switch (_aiState)
        {
            case AIState.Idle:
                _agent.speed = walkSpeed;
                _agent.isStopped = true;
                break;
            case AIState.Wandering:
                _agent.speed += walkSpeed;
                _agent.isStopped = false;
                break;
            case AIState.Attacking:
                _agent.speed = runSpeed;
                _agent.isStopped = false;
                break;
        }

        _animator.speed = _agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if(_aiState == AIState.Wandering && _agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
    }

    void WanderToNewLocation()
    {
        if ()
    }

    void AttackingUpdate()
    {

    }
}