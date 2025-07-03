using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerKillBeam : GameMonoBehaviour
{

    float kill_beam_cooldown => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.kill_beam_cooldown);
    float kill_beam_duration => Cfg.upgraderCtrl.GetStatCurrent(UpgraderID.kill_beam_duration);

    [SerializeField]
    private Transform beamRotate;
    [SerializeField]
    private GameObject killBeam;
    [SerializeField]
    private GameObject killBeam2;
    private float change_double_killbeam => (ConfigManager.instance.labCtrl.LapManager.
                    GetSingleSubjectById(IdSubjectType.DOUBLE_KILL_BEAM).GetCurrentProperty() / 100f);

    private float countdownTime;
    private float currentTime;
    private float durationTime;
    private float curDurationTime;

    private float rotationSpeed = 90f;

    private void Start()
    {
        Init(null);
        EventDispatcher.AddEvent(EventID.OnUpgradeInGame, Init);
    }

    private void Init(object o)
    {
        currentTime = kill_beam_cooldown;
        countdownTime = kill_beam_cooldown;

        durationTime = kill_beam_duration;
        curDurationTime = kill_beam_duration;
    }

    void Update()
    {
        if (!GameDatas.IsUnlockClusterUpgrader(UpgraderCategory.WORKSHOP_KILL_BEAM))
            return;
        ActiveKillBeam();
    }

    private void ActiveKillBeam()
    {
        if (currentTime > 0)
        {
            killBeam2.SetActive(false);
            killBeam.SetActive(false);
            currentTime -= Time.deltaTime;
        }
        else
        {
            if(change_double_killbeam.Chance())
            {
                killBeam2.SetActive(true);
                killBeam.SetActive(true);
            } else
            {
                killBeam.SetActive(true);
            }
            beamRotate.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            if (curDurationTime > 0)
            {
                curDurationTime -= Time.deltaTime;
            }
            else
            {
                killBeam2.SetActive(false);
                killBeam.SetActive(false);
                currentTime = countdownTime; //reset
                curDurationTime = durationTime; //reset
            }
        }
    }
}
