using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeInforData", menuName = "Data/Upgrade/UpgradeInforData", order = 0)]
public class SO_UpgradeInforData : ScriptableObject
{

    public UpgraderGroupID upgraderGroupID;
    public UpgraderID upgraderID;
    public Format format;
    public string TextID;
    public int maxLevel = -1; 
    public float coefficient;

    public Vector3 property = Vector3.one;
    public Vector3 price_base = Vector3.one;
    public Vector3 price_ingame = Vector3.one;
    public UpgraderCategory upgraderCategory;
    //public int levelUnlock = 0;
    //public BuyingType buyingType;

    public float GetProperty(int level)
    {
        float discount = 1f;

        // Map các UpgraderID sang IdSubjectType
        var subjectMap = new Dictionary<UpgraderID, IdSubjectType>
    {
        { UpgraderID.attack_damage, IdSubjectType.ATK_DAMAGE },
        { UpgraderID.attack_range, IdSubjectType.ATTACK_RANGE_ADD },
        { UpgraderID.attack_speed, IdSubjectType.SPEED_ATTACK_ADDED },
        { UpgraderID.critical_shot_damage, IdSubjectType.CRITICAL_DAMAGE },
        { UpgraderID.health, IdSubjectType.HEALTH },
        { UpgraderID.generation, IdSubjectType.HEALTH_REGEN },
        { UpgraderID.bomb_damage, IdSubjectType.BOMB_DAMAGE },
        { UpgraderID.satellite_damage, IdSubjectType.SATELLITE_DAMAGE },
        { UpgraderID.gold_bonus_each_wave, IdSubjectType.GOLD_PER_WAVE },
        { UpgraderID.damage_resistance, IdSubjectType.DAMAGE_RESISTANCE },
        { UpgraderID.lifebox_change, IdSubjectType.LIFEBOX_CHANGE },
        { UpgraderID.lifebox_max_hp, IdSubjectType.LIFEBOX_MAX_HP },
        { UpgraderID.lifebox_hp_amount, IdSubjectType.LIFEBOX_HP_AMOUNT },
        { UpgraderID.gold_per_kill, IdSubjectType.GOLD_PER_KILL },
        { UpgraderID.coin_per_kill, IdSubjectType.COIN_PER_KILL },
        { UpgraderID.coin_per_wave, IdSubjectType.COIN_PER_WAVE },
        { UpgraderID.damage_reduce, IdSubjectType.DAMAGE_REDUCE },
        { UpgraderID.damage_range, IdSubjectType.DAMAGE_RANGE },
        { UpgraderID.impulse_wave_size, IdSubjectType.IMPUSLE_WAVE_SIZE },
    };

        if (subjectMap.TryGetValue(upgraderID, out var subjectID))
        {
            float currentProp = ConfigManager.instance.labCtrl.LapManager
                .GetSingleSubjectById(subjectID).GetCurrentProperty();
            discount = 1f + currentProp / 100f;
        }
        else if (upgraderID == UpgraderID.impulse_wave_size)
        {
            discount = ConfigManager.instance.labCtrl.LapManager
                .GetSingleSubjectById(IdSubjectType.IMPUSLE_WAVE_SIZE).GetCurrentProperty();
        }
        else if (upgraderID == UpgraderID.lifebox_hp_amount)
        {
            discount = ConfigManager.instance.labCtrl.LapManager
                .GetSingleSubjectById(IdSubjectType.LIFEBOX_HP_AMOUNT).GetCurrentProperty();
        }

        Vector3 property = CalculatePropertyUpgrade(upgraderID, level);
        float value = property.x * Mathf.Pow(level, 2) + property.y * level + property.z;

        return value * discount;
    }


    /*    public float GetProperty(int level)
        {
            var discount = 1f;
            switch (upgraderID)
            {
                case UpgraderID.attack_damage:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.ATK_DAMAGE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.attack_range:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.ATTACK_RANGE_ADD).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.attack_speed:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.SPEED_ATTACK_ADDED).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.critical_shot_damage:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.CRITICAL_DAMAGE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.health:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.HEALTH).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.generation:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.HEALTH_REGEN).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.bomb_damage:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.BOMB_DAMAGE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.satellite_damage:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.SATELLITE_DAMAGE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.gold_bonus_each_wave:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.GOLD_PER_WAVE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.impulse_wave_size:
                    discount = ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.IMPUSLE_WAVE_SIZE).GetCurrentProperty();
                    break;
                case UpgraderID.damage_resistance:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.DAMAGE_RESISTANCE).GetCurrentProperty() / 100);
                    break;
                case UpgraderID.lifebox_change:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.LIFEBOX_CHANGE).GetCurrentProperty() / 100f);
                    break;
                case UpgraderID.lifebox_hp_amount:
                    discount = ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.LIFEBOX_HP_AMOUNT).GetCurrentProperty();
                    break;
                case UpgraderID.lifebox_max_hp:
                    discount = 1f + (ConfigManager.instance.labCtrl.LapManager.
                        GetSingleSubjectById(IdSubjectType.LIFEBOX_MAX_HP).GetCurrentProperty() / 100);
                    break;
            }

            Vector3 property = CalculatePropertyUpgrade(upgraderID, level);
            var a = property.x;
            var b = property.y;
            var c = property.z;

            var value = a * Mathf.Pow(level, 2) + b * level + c;
            return value * discount;
        }*/

    private Vector3 CalculatePropertyUpgrade(UpgraderID id, int level)
    {
        float a = property.x, b = property.y, c = property.z;

        void AddValues(ref float val, float per10, float per50)
        {
            val += (level / 10) * per10;
            val += (level / 50) * per50;
        }

        void AddValuesB(ref float val, float per5, float per20)
        {
            val += (level / 5) * per5;
            val += (level / 20) * per20;
        }

        void AddValuesC(ref float val, float per10, float per30)
        {
            val += (level / 10) * per10;
            val += (level / 30) * per30;
        }

        switch (id)
        {
            case UpgraderID.attack_damage:
                AddValues(ref a, 0.004f, 0.02f);
                AddValuesB(ref b, 0.2f, 0.8f);
                AddValuesC(ref c, 0.1f, 2f);
                break;

            case UpgraderID.health:
                AddValues(ref a, 0.1f, 0.5f);
                AddValuesB(ref b, 5f, 20f);
                AddValuesC(ref c, 20f, 40f);
                break;

            case UpgraderID.generation:
                AddValues(ref a, 0.05f, 0.25f);
                AddValuesB(ref b, 0.5f, 2f);
                AddValuesC(ref c, 0.1f, 0.2f);
                break;

            case UpgraderID.damage_reduce:
                AddValues(ref a, 0.003f, 0.015f);
                AddValuesB(ref b, 0.05f, 0.2f);
                AddValuesC(ref c, 0.02f, 0.04f);
                break;
        }

        return new Vector3(a, b, c);
    }

    public int GetPriceItemUpgradeBase(int level)
    {
        var a = price_base.x;
        var b = price_base.y;
        var c = price_base.z;

        if(upgraderID != UpgraderID.damage_reduce)
        {
            //tinh tien gold
            a += (level / 30) * 0.5f;
            a += (level / 50) * 0.6f;

            b += (level / 10) * 0.4f;
            b += (level / 30) * 0.5f;

            c += (level / 10) * 0.3f;
            c += (level / 25) * 0.6f;
        }

        var value = (int)(a * Mathf.Pow(level, 2) + b * level + c);

        //discount
        var discount = 0f;
        switch (upgraderGroupID)
        {
            case UpgraderGroupID.ATTACK:
                discount = 1f - (ConfigManager.instance.labCtrl.LapManager.GetSingleSubjectById
                    (IdSubjectType.UPGRADE_ATTACK_DISCOUNT).GetCurrentProperty() / 100f);
                break;
            case UpgraderGroupID.DEFENSE:
                discount = 1f - (ConfigManager.instance.labCtrl.LapManager.GetSingleSubjectById
                    (IdSubjectType.UPGRADE_DEFENSE_DISCOUNT).GetCurrentProperty() / 100f);
                break;
            case UpgraderGroupID.RESOURCE:
                discount = 1f - (ConfigManager.instance.labCtrl.LapManager.GetSingleSubjectById
                    (IdSubjectType.UPGRADE_GOLD_DISCOUNT).GetCurrentProperty() / 100f);
                break;
            default:
                discount = 1;
                break;
        }

        var price = value * discount;
        return (int)price;
    }

    public int GetPriceItemUpgradeInGame(int level)
    {
        var a = price_ingame.x;
        var b = price_ingame.y;
        var c = price_ingame.z;
        return (int)(a * Mathf.Pow(level, 2) + b * level + c);
    }

   /* public bool isUnlock()
    {
        return levelUnlock <= GameDatas.GetLevelUpgraderGroup(groupID);
    }*/

    public float GetCoefficient(int level)
    {
        return coefficient * level;
    }

}
