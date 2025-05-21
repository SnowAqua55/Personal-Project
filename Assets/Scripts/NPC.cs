using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngineInternal;

public enum AIState
{
    Idle,
    Wandering,  // ���Ƿ� ��ǥ ���� ��� �ڵ����� �����̰� ���ִ� �κ�
    Attacking
}

public class NPC : MonoBehaviour, IDamagable
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
    public int damage;
    public float attackRate;
    private float _lastAttackTime;
    public float attackDistance;

    private float _playerDistance;

    public float fieldOfView = 120f;

    private Animator _animator;
    private SkinnedMeshRenderer[] _meshRenderers;
    private Camera _camera;

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

        if(_playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (_aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        _agent.SetDestination(GetWanderLocation());  // SetDestination : �̵� ��ǥ ���� ����
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        // NavMesh.SamplePosition((����)�̵� ���� ����, (out)��� ������ NavMeshHit ����, �ִ� �Ÿ�, ���̾� ���͸�)
        // Random.onUnitSphere : �������� 1�� �� ��� ����
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
        
        int i = 0;

        while(Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    void AttackingUpdate()
    {
        if(_playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            _agent.isStopped = true;
            if(Time.time - _lastAttackTime > attackRate)
            {
                _lastAttackTime = Time.time;
                CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
                _animator.speed = 1;
                _animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (_playerDistance < detectDistance)
            { 
                _agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (_agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))  // CalculatePath : ��θ� ����ؼ� �� �� �ִ� ���̸� true �׷��� ������ false ��ȯ
                {
                    _agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    _agent.SetDestination(transform.position);
                    _agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                _agent.SetDestination(transform.position);
                _agent.isStopped = true;
                SetState(AIState.Wandering);
            }

        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position; ;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView;
    }

    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for(int i = 0; i < _meshRenderers.Length; i++)
            _meshRenderers[i].material.color = new Color(1.0f, .6f, .6f);

        yield return new WaitForSeconds(0.1f);

        for(int i = 0; i < _meshRenderers.Length; i++)
            _meshRenderers[i].material.color = Color.white;
    }
}