using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "Subject_SO", menuName = "Data/Labs/Subject_SO", order = 0)]
public class Subject_SO : ScriptableObject
{

    public SubjectType subjectType;
    public IdSubjectType id;
    public Format format;
    public int currentLevel => GameDatas.GetLevelSubjectLab(id);
    public int levelMax;
    public Vector3 property;
    public Vector3 priceGold;
    public Vector3 time;
    public LabCategory labCategory;
    public BuyingType buyingType;
    public SubjectTypeSupport subjectTypeSupport;

    [ShowIf(nameof(IsSubjectSupport))]
    public float priceBuy;
    [ShowIf(nameof(IsSubjectSupport))]
    public float timeUpgrade;
    [ShowIf(nameof(IsSubjectBonus))]
    public List<InforLabSupport> inforLabSupport;

    private bool IsSubjectSupport() => subjectTypeSupport == SubjectTypeSupport.SUPPORT;
    private bool IsSubjectBonus() => subjectTypeSupport == SubjectTypeSupport.BONUS;

    public bool isUpgrading => DateTime.Now < timeTargetFinish;
    public DateTime timeTargetFinish => GameDatas.GetTimeTargetSubject(id);
    public long Price(int level)
    {
        var a = priceGold.x;
        var b = priceGold.y;
        var c = priceGold.z;

        //tinh tien gold
        a += (level / 30) * 0.5f;
        a += (level / 50) * 0.6f;

        b += (level / 10) * 0.4f;
        b += (level / 30) * 0.5f;

        c += (level / 10) * 0.3f;
        c += (level / 25) * 0.6f;

        var value = (level.Pow(2) * a + level * b + c);

        var discount = (1 - ConfigManager.instance.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.LAB_DISCOUNT).GetCurrentProperty() / 100f);
        value = value * discount;

        return (long)value;
    }
    public float Property(int level)
    {
        var value = (level.Pow(2) * property.x + level * property.y + property.z);
        return value;
    }
    public float Time(int level)
    {
        if (subjectTypeSupport == SubjectTypeSupport.SUPPORT)
        {
            return TimeSupport();
        }
        if (subjectTypeSupport == SubjectTypeSupport.BONUS)
        {
            return TimeSubjectBonus();
        }
        var value = (level.Pow(2) * time.x + level * time.y + time.z);
        return DisCountUpgrdaeLab(value);
    }

    public float TimeSupport()
    {
        var value = timeUpgrade;
        return DisCountUpgrdaeLab(value);
    }

    public float TimeSubjectBonus()
    {
        InforLabSupport inforLab = inforLabSupport[GameDatas.GetLevelSubjectLab(id) - 1];
        var value = inforLab.time;
        return DisCountUpgrdaeLab(value);
    }

    private float DisCountUpgrdaeLab(float value)
    {
        var discount = (1f - ConfigManager.instance.labCtrl.LapManager.
            GetSingleSubjectById(IdSubjectType.LAB_SPEED).GetCurrentProperty() / 100f);
        discount = ConfigManager.instance.relicsCtrl.PercentDeductRelic(BonusTypeRelic.LabSpeed, discount);
        value = value * discount;

        return value;
    }

    public float GetCurrentProperty()
    {
        if(buyingType == BuyingType.REWARD)
        {
            if (!GameDatas.IsClusterUnlockLabInfor(labCategory)) return 0f;
        } else
        {
            if (GameDatas.GetLevelSubjectLab(id) == 0) return 0f;
        }
        return Property(currentLevel);
    }
    public float GetCurrentTime()
    {
        return Time(currentLevel);
    }

    public float GetNextProperty()
    {
        float property = Property(currentLevel + 1);
        return property == 0 ? 1 : property;
    }
    public float GetNextTime()
    {
        return Time(currentLevel + 1);
    }
    public long GetCurrentPrice()
    {
        if(subjectTypeSupport == SubjectTypeSupport.SUPPORT)
        {
            return (long)priceBuy;
        } 
        if(subjectTypeSupport == SubjectTypeSupport.BONUS)
        {
            InforLabSupport inforLab = inforLabSupport[GameDatas.GetLevelSubjectLab(id) - 1];
            return (long)inforLab.price;
        } 
        return Price(currentLevel);
    }
}

[Serializable]
public class InforLabSupport
{
    public string name;
    public float price;
    public float time;
}
