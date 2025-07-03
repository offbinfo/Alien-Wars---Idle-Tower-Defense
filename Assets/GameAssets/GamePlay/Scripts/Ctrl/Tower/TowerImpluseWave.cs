using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_ImpulseWave
{
    void ImpulseWaveDeffense(Collider2D other);
}

public class TowerImpluseWave : GameMonoBehaviour, I_ImpulseWave
{
    
    private float frequency => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.impulse_wave_frequency);
    private float sizeImpulseWave => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.impulse_wave_size);

    private float pushImpulseWaveSpeed = 5f;
    protected virtual bool canDeffense => objData.isAlive;
    protected Object_DataInformation objData;
    private CircleCollider2D circleCollider;

    private float baseRanged = 1f;
    private float sizeImpulseInGame;
    private float timeActiveImpulseWave;
    public float duration = 0.5f;

    private float timer = 0f;
    private bool isExpanding = false;
    private bool isShrinking = false;
    private float startTime;

    [SerializeField]
    private ParticleSystem fxImpulseWave;

    public virtual void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        objData = GetComponentInParent<Object_DataInformation>();
    }

    private void Start()
    {
        Init(null);
        circleCollider.radius = 0f;
        EventDispatcher.AddEvent(EventID.OnUpgradeInGame, Init);
        DebugCustom.LogColor("frequency "+ frequency);
    }

    void Update()
    {
        if (frequency == 0) return;
        ActiveImpulseWave();
    }

    private void ActiveImpulseWave()
    {
        if (Time.timeScale == 0f)
            return;

        float deltaTime = Time.unscaledDeltaTime;
        float currentTime = Time.unscaledTime;

        timer += deltaTime;

        if (timer >= timeActiveImpulseWave && !isExpanding && !isShrinking)
        {
            isExpanding = true;
            startTime = currentTime;
        }

        if (isExpanding)
        {
            float t = (currentTime - startTime) / duration;
            if (t >= 1f)
            {
                t = 1f;
                isExpanding = false;
                isShrinking = true;
                startTime = currentTime;
            }

            fxImpulseWave.gameObject.SetActive(true);
            circleCollider.radius = Mathf.Lerp(0f, sizeImpulseInGame, t);
        }
        else if (isShrinking)
        {
            float t = (currentTime - startTime) / duration;
            if (t >= 1f)
            {
                t = 1f;
                isShrinking = false;
                timer = 0f;
            }

            circleCollider.radius = Mathf.Lerp(sizeImpulseInGame, 0f, t);
        }

        if (circleCollider.radius < 0.1f)
        {
            fxImpulseWave.gameObject.SetActive(false);
        }
    }

    private void Init(object o)
    {
        float bonusSizeLabs = GameDatas.IsClusterUnlockLabInfor(LabCategory.WORKSHOP_IMPLUSE_WAVE) == true ? 0 :
            Cfg.labCtrl.LapManager.GetSingleSubjectById(IdSubjectType.IMPUSLE_WAVE_SIZE).GetCurrentProperty();

        timeActiveImpulseWave = /*1 / */frequency;
        sizeImpulseInGame = (baseRanged + sizeImpulseWave) + bonusSizeLabs;

        float targetDiameter = (sizeImpulseInGame / 2f) - 0.2f;

        fxImpulseWave.transform.localScale = new Vector3(targetDiameter, targetDiameter, 1f);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTags.TAG_ENEMIES))
        {
            ImpulseWaveDeffense(collision);
        }
    }

    public void ImpulseWaveDeffense(Collider2D other)
    {
        if (!other.GetComponent<EnemyTakeDamage>().isBoss)
        {
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;
            other.transform.position += pushDirection * pushImpulseWaveSpeed * Time.deltaTime;
        }
    }


}
