using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class EnemyMovement : Object_Move
{
    private Animator anim;
    private Object_DataInformation _dataInformation;
    private EnemyTakeEffect _takeEffect;
    private GameObject tower;

    private NativeArray<Vector3> enemyPositions;
    private NativeArray<Vector3> enemyDirections;
    private NativeArray<float> enemySpeeds;
    private NativeArray<bool> enemyCanMove;

    private JobHandle moveJobHandle;
    private float stopDistance = 1f;
    private EnemyAttack enemyAttack;
    public bool isSetDirection;

    public override void Awake()
    {
        base.Awake();
        _dataInformation = GetComponent<Object_DataInformation>();
        _takeEffect = GetComponent<EnemyTakeEffect>();
        anim = GetComponentInChildren<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();  
        tower = GPm.Tower.gameObject;
    }

    private void OnEnable()
    {
        isNoMove = false;

        if (!enemyPositions.IsCreated) enemyPositions = new NativeArray<Vector3>(1, Allocator.Persistent);
        if (!enemyDirections.IsCreated) enemyDirections = new NativeArray<Vector3>(1, Allocator.Persistent);
        if (!enemySpeeds.IsCreated) enemySpeeds = new NativeArray<float>(1, Allocator.Persistent);
        if (!enemyCanMove.IsCreated) enemyCanMove = new NativeArray<bool>(1, Allocator.Persistent);

        SetDirectionMove(CalculatorDirection());
        if(isSetDirection)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
    }
    private void OnDestroy()
    {
        moveJobHandle.Complete(); 

        if (enemyPositions.IsCreated) enemyPositions.Dispose();
        if (enemyDirections.IsCreated) enemyDirections.Dispose();
        if (enemySpeeds.IsCreated) enemySpeeds.Dispose();
        if (enemyCanMove.IsCreated) enemyCanMove.Dispose();
    }

    public override void SetDirectionMove(Vector3 direction)
    {
        base.SetDirectionMove(direction);
    }

    private Vector3 CalculatorDirection()
    {
        return tower.transform.position - transform.position;
    }

    private void OnDisable()
    {
        moveJobHandle.Complete();
        if (enemyPositions.IsCreated) enemyPositions.Dispose();
        if (enemyDirections.IsCreated) enemyDirections.Dispose();
        if (enemySpeeds.IsCreated) enemySpeeds.Dispose();
        if (enemyCanMove.IsCreated) enemyCanMove.Dispose();
    }

    public void Update()
    {
        if (isNoMove || Gm.GameState == GameState.EndGame) return;
        //if (!_dataInformation || !_takeEffect) return;

        enemyPositions[0] = transform.position;
        enemyDirections[0] = (tower.transform.position - transform.position).normalized;
        enemySpeeds[0] = _takeEffect.isStun ? 0.1f : base.speedMove * (1 - (_takeEffect.slowPower / 100));
        enemyCanMove[0] = _dataInformation.hpCurrent > 0 && tower.activeSelf;

        var job = new EnemyMoveJob
        {
            positions = enemyPositions,
            directions = enemyDirections,
            speeds = enemySpeeds,
            canMove = enemyCanMove,
            deltaTime = Time.deltaTime
        };

        moveJobHandle = job.Schedule(enemyPositions.Length, 64);
    }

    private void LateUpdate()
    {
        moveJobHandle.Complete();

        if (!enemyCanMove[0] || enemyPositions[0] == Vector3.zero) return;

        float distanceToTower = Vector3.Distance(enemyPositions[0], tower.transform.position);
        if (distanceToTower > stopDistance)
        {
            transform.position = enemyPositions[0];
        }
    }

    /*private void LateUpdate()
    {
        moveJobHandle.Complete();
        if(enemyPositions[0] != Vector3.zero)
        {
*//*            if(enemyAttack.enemyAttackType == EnemyAttackType.Ranged)
            {
                transform.position = enemyPositions[0];
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(enemyPositions[0], tower.transform.position);

                if (distanceToPlayer > stopDistance)
                {
                    transform.position = enemyPositions[0];
                }
            }*//*
            float distanceToPlayer = Vector3.Distance(enemyPositions[0], tower.transform.position);

            if (distanceToPlayer > stopDistance)
            {
                transform.position = enemyPositions[0];
            }
        }
    }*/

    [BurstCompile]
    private struct EnemyMoveJob : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        [ReadOnly] public NativeArray<Vector3> directions;
        [ReadOnly] public NativeArray<float> speeds;
        [ReadOnly] public NativeArray<bool> canMove;
        public float deltaTime;

        public void Execute(int index)
        {
            if (!canMove[index]) return;

            float speed = speeds[index] * deltaTime;
            positions[index] += directions[index] * speed;
        }
    }

}
