using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class BaseAircaft : GameMonoBehaviour
{
    private Transform center;
    private Coroutine rotateRoutine;
    private float elapseTime;
    [SerializeField]
    protected TypeBot typeBot;

    private const float SPEED_ROTATE = 50f;
    private float AMPLITUDE => (Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.attack_range) / 10) - 0.6f;
    private const float FREQUENCY = 0.3f;
    private float duration = 2f;
    private float coolDown = 2f;
    private float ranged = 2f;

    protected float minScale = 1f;
    protected float maxScale = 5.5f;
    protected CircleCollider2D circleCollider;
    private TowerData towerData;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    private Vector3 TargetPos;

    public virtual float Duration
    {
        get { return duration; }
        set
        {
            duration = value;
        }
    }

    public virtual float Cooldown
    {
        get { return coolDown; }
        set
        {
            coolDown = value;
        }
    }

    public virtual float Ranged
    {
        get { return ranged; }
        set
        {
            ranged = value;
        }
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        circleCollider.enabled = false;
        towerData = TowerCtrl.instance.GetComponent<TowerData>();
        TargetPos = towerData.transform.position + (towerData.attackRange + 0.5f).GetRandomPosition();
        SetUp();
    }

    private void OnEnable()
    {
        MoveOutAndStartOrbit();
    }

    public abstract void SetUp();  

    private void OnDisable()
    {
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
    }

    private void MoveOutAndStartOrbit()
    {
        center = GPm.Tower.transform;
        transform.position = center.position;

        elapseTime = Random.Range(0f, 10f);
        Vector3 dir = new Vector3(-1f, 1f, 0f).normalized;
        Vector3 target = center.position + dir * (AMPLITUDE + 0.6f);

        transform.DOMove(target, 0.7f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            if (isActiveAndEnabled)
                rotateRoutine = StartCoroutine(OrbitLoop());
        });
    }

    float updateTargetInterval = 1f;
    float updateTimer = 0f;

    private IEnumerator OrbitLoop()
    {

        while (true)
        {
            ActiveSkill(false);
            float time = 0f;

            while (time < coolDown)
            {
                time += Time.deltaTime;
                elapseTime += Time.deltaTime;
                updateTimer += Time.deltaTime;

                if (updateTimer >= updateTargetInterval)
                {
                    TargetPos = towerData.transform.position + (towerData.attackRange + 0.5f).GetRandomPosition();
                    updateTimer = 0f;
                }

                transform.position += transform.up * moveSpeed * Time.deltaTime;
                var dir = TargetPos - transform.position;

                var angle = Vector2.SignedAngle(Vector2.up, dir);
                var rotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                yield return null;
            }

            ActiveSkill(true);
            yield return Yielders.Get(duration);
        }
    }

    public virtual void ActiveSkill(bool Active)
    {
        circleCollider.enabled = Active;
    }
}
