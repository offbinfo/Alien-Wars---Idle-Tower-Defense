using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface I_Nexus_Fly
{
    void Fly(Vector3 target, float radius);
}

public class NexusPrefab : GameMonoBehaviour, I_Nexus_Fly
{

    private float pullSpeed = 2f;      // Tốc độ hút vào
    private float orbitSpeed = 150f;   // Tốc độ xoay quanh Nexus
    private float orbitDecay = 0.98f;  // Giảm bán kính quỹ đạo

    private HashSet<Transform> enemies = new HashSet<Transform>();
    private Dictionary<Transform, float> angles = new Dictionary<Transform, float>();
    private Object_Pool object_Pool;
    private CircleCollider2D circleCollider;

    private float timer = 0f;
    private bool isCounting = false;
    private float delay = 1f;
    [SerializeField]
    private Transform sizeVoid;

    private float goldBonusNexus => 2 + Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.VOID_NEXUS_GOLD_BONUS).GetCurrentProperty();

    [SerializeField]
    private GameObject fxNexus;
    Damager damager
    {
        get
        {
            var dmg = new Damager();
            dmg.damage = Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.VOID_NEXUS_DAMAGE).GetCurrentProperty();
            dmg.objAttack = gameObject;
            dmg.type = DamageType.NORMAL;
            return dmg;
        }
    }

    private void StartDelay()
    {
        fxNexus.SetActive(false);
        timer = 0f;
        isCounting = true;
    }

    private void OnEnable()
    {
        StartDelay();
    }

    private void OnDisable()
    {
        fxNexus.transform.localScale = Vector3.zero;
    }

    private void CheckActiveFx()
    {
        if (!isCounting) return;

        timer += Time.deltaTime;
        if (timer >= delay)
        {
            fxNexus.SetActive(true);
            isCounting = false;
        }
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        object_Pool = GetComponent<Object_Pool>();
    }

    public void Fly(Vector3 target, float radius)
    {
        float size = radius - 2f;

        circleCollider.radius = size;
        sizeVoid.localScale = new Vector3(size, size, size);

        var timeFly = Vector3.Distance(target, transform.position) / 2f;
        transform.DOMove(target, timeFly).SetEase(Ease.OutQuint);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.TAG_ENEMIES))
        {
            EventChallengeListenerManager.EnemiesInVoidNexus(1);
            enemies.Add(other.transform);
            angles[other.transform] = Random.Range(0f, 360f);

            EnemyTakeDamage takeDamage = other.GetComponent<EnemyTakeDamage>();
            takeDamage.IncreareGoldBonus(goldBonusNexus);
            takeDamage.TakeDamage(damager);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.TAG_ENEMIES))
        {
            enemies.Remove(other.transform);
            angles.Remove(other.transform);
        }
    }

    private void Update()
    {
        CheckActiveFx();

        if (enemies.Count == 0) return;
        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(enemy.position, transform.position);

            float newRadius = distance * orbitDecay;

            angles[enemy] += orbitSpeed * Time.deltaTime;
            float radian = angles[enemy] * Mathf.Deg2Rad;

            // Xác định vị trí mới (quay quanh Nexus + hút vào)
            Vector3 targetPos = transform.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * newRadius;
            enemy.position = Vector3.Lerp(enemy.position, targetPos, pullSpeed * Time.deltaTime);
        }
    }
}
