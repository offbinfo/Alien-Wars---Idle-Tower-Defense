using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneCellView : MonoBehaviour
{

    [SerializeField]
    private Image imgItem;
    [SerializeField]
    private TMP_Text txtItem;
    [SerializeField]
    private GameObject panelReceived;
    private MilestoneReward milestoneRewardData;
    [SerializeField]
    private RectTransform rect;
    private bool isReceived;
    [SerializeField]
    private GameObject lockItemReward;
    public int waveItem;
    public WorldType typeWorld;
    private bool isUnlock;
    public TypeMileStoneCateGory typeMileStoneCateGory;
    [SerializeField]
    private GameObject objBlur;

    private void Start()
    {
        EventDispatcher.AddEvent(EventID.OnClaimAllItemReward, OnClaimAllItemReward);
    }

    public void SetData(MilestoneReward milestoneReward, Sprite icon, int wave, WorldType typeWorld)
    {
        waveItem = wave;
        milestoneRewardData = milestoneReward;
        this.typeWorld = typeWorld;
        txtItem.text = milestoneReward.amount != 0 ?
            milestoneReward.amount + " " : "" + GetName();
        //imgItem.sprite = icon;

        SetImage(icon);

        if (icon == null) imgItem.gameObject.SetActive(false);
        OnChangePosYText();
    }

    void SetImage(Sprite sprite)
    {
        if (sprite == null || imgItem == null) return;

        imgItem.sprite = sprite;
        float originalWidth = sprite.texture.width;
        float originalHeight = sprite.texture.height;

        float aspectRatio = originalWidth / originalHeight;

        RectTransform rt = imgItem.GetComponent<RectTransform>();

        if (aspectRatio > 1) 
        {
            rt.sizeDelta = new Vector2(120, 120 / aspectRatio);
        }
        else
        {
            rt.sizeDelta = new Vector2(120 * aspectRatio, 120);
        }
    }

    private string GetName()
    {
        switch (milestoneRewardData.typeMilestone)
        {
            case TypeMilestone.UNLOCK_UPGRADER_INFOR:
                return Extensions.FormatEnumName(milestoneRewardData.upgraderCategory);
            case TypeMilestone.UNLOCK_LAB_INFOR:
                return Extensions.FormatEnumName(milestoneRewardData.labCategory);
            default:
                return Extensions.FormatEnumName(milestoneRewardData.typeMilestone);
        }
    }

    public void CheckUnLockItemReward(int wave)
    {
        if (wave >= waveItem)
        {
            this.isUnlock = true;
            if(typeMileStoneCateGory == TypeMileStoneCateGory.PREMIUM)
            {
                lockItemReward.SetActive(!GameDatas.IsUsingPremiumMileStones);
            } else
            {
                lockItemReward.SetActive(false);
            }
        }
        else
        {
            this.isUnlock = false;
            lockItemReward.SetActive(true);
        }

        if(!lockItemReward.activeSelf)
        {
            objBlur.SetActive(true);
        } else
        {
            objBlur.SetActive(false);
        }
    }

    public void CheckItemRewardReceived(ItemData itemData)
    {
        if (itemData.IndexWave == waveItem && itemData.World == typeWorld
            && itemData.Category == typeMileStoneCateGory)
        {
            this.isReceived = true;
            panelReceived.SetActive(isReceived);
            lockItemReward.SetActive(false);
        }
        else
        {
            lockItemReward.SetActive(true);
        }
    }

    private bool CheckReceivedItemPremium()
    {
        if (typeMileStoneCateGory == TypeMileStoneCateGory.PREMIUM)
        {
            if (GameDatas.IsUsingPremiumMileStones) return false;
            return true;
        }
        return false;
    }

    private void OnChangePosYText()
    {
        if (!imgItem.gameObject.activeSelf)
        {
            txtItem.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    public void OnClick()
    {
        if (CheckReceivedItemPremium()) return;
        if (!isUnlock || isReceived) return;
        if (milestoneRewardData == null) return;
        panelReceived.SetActive(true);
        HandleMilestoneReward(milestoneRewardData.typeMilestone);
        ItemData item = new(waveItem, typeMileStoneCateGory, typeWorld);

        GameDatas.SaveAllItems(item);
        isReceived = true;

        PanelClaimReward.instance.gameObject.SetActive(true);
    }
    
    public void HandleMilestoneReward(TypeMilestone type)
    {
        switch (type)
        {
            case TypeMilestone.POWER_STONE:
                HandlePowerStone();
                break;
            case TypeMilestone.GOLD:
                HandleGold();
                break;
            case TypeMilestone.GEM:
                HandleGem();
                break;

            case TypeMilestone.DAILY_GIFT:
            case TypeMilestone.LUCKY_DRAW:
            case TypeMilestone.RANKING:
            case TypeMilestone.CARDS:
            case TypeMilestone.LABS:
            case TypeMilestone.AREA:
                HandleUnlockFeatures(type);
                break;

            case TypeMilestone.UNLOCK_LAB_INFOR:
                HandleLabRewards();
                break;

            case TypeMilestone.UNLOCK_UPGRADER_INFOR:
                HandleUnlockWorkShopRewards();
                break;

            case TypeMilestone.AVATAR_CYBERNOVA:
            case TypeMilestone.AVATAR_PHOTON_BLASTER:
            case TypeMilestone.AVATAR_QUANTUM_CANNON:
                HandleUnlockAvatar(type);
                break;

            case TypeMilestone.WORLD_2:
            case TypeMilestone.WORLD_3:
            case TypeMilestone.WORLD_4:
            case TypeMilestone.WORLD_5:
            case TypeMilestone.WORLD_6:
                HandleWorldRewards(type);
                break;

            default:
                break;
        }
    }


    private void HandlePowerStone()
    {
        int amount = milestoneRewardData.amount;
        GameDatas.PowerStone += amount;
        ActiveAnimFlyCurrency(CurrencyType.POWER_STONE);
    }
    private void HandleGold()
    {
        int amount = milestoneRewardData.amount;
        GameAnalytics.LogEvent_EarnGold("reward_milestone", amount);
        GameDatas.Gold += amount;
        GameDatas.SetDataProfile(IDInfo.TotalGoldEarned, GameDatas.GetDataProfile(IDInfo.TotalGoldEarned) + amount);
        ActiveAnimFlyCurrency(CurrencyType.GOLD);
    }
    private void HandleGem()
    {
        int amount = milestoneRewardData.amount;
        GameDatas.Gem += amount;
        GameDatas.SetDataProfile(IDInfo.TotalGemsEarned, GameDatas.GetDataProfile(IDInfo.TotalGemsEarned) + amount);
        ActiveAnimFlyCurrency(CurrencyType.GEM);
    }

    private void ActiveAnimFlyCurrency(CurrencyType currency)
    {
        Transform posEnd = null;
        TopUI_Currency topUI_Currency = TopUI_Currency.instance;
        switch (currency)
        {
            case CurrencyType.GOLD:
                posEnd = topUI_Currency.goldIcon.transform;
                break;
            case CurrencyType.GEM:
                posEnd = topUI_Currency.gemIcon.transform;
                break;
            case CurrencyType.POWER_STONE:
                posEnd = topUI_Currency.powerStoneIcon.transform;
                break;
            default:
                break;
        }
        ObjectUI_Fly_Manager.instance.Get(20, rect.transform.position, posEnd.position, currency);
    }

    private void HandleUnlockFeatures(TypeMilestone type)
    {
        switch (type)
        {
            case TypeMilestone.DAILY_GIFT:
                GameDatas.isUnlockDailyGift = true;
                break;
            case TypeMilestone.LUCKY_DRAW:
                GameDatas.isUnlockLuckyDraw = true;
                break;
            case TypeMilestone.RANKING:
                GameDatas.isUnlockRanking = true;
                break;
            case TypeMilestone.AREA:
                GameDatas.IsUnlockFeatureArena = true;
                break;
            case TypeMilestone.CARDS:
                GameDatas.IsUnlockFeatureCard = true;
                break;
            case TypeMilestone.LABS:
                StartCoroutine(DelayOpenTutorialLab());
                break;
            default:
                break;
        }
        EventDispatcher.PostEvent(EventID.OnRefeshUIHome, 0);
    }

    private IEnumerator DelayOpenTutorialLab()
    {
        ControlAllButton.DeactiveAllButton();
        yield return null;
        GameDatas.isUnlockLab = true;
        yield return Yielders.Get(0.5f);
        Board_UIs.instance.DicBoards[UiPanelType.PopupMileStones].gameObject.SetActive(false);
        if (GameDatas.isUnlockLab && !GameDatas.isTutLab)
        {
            TutorialManager.instance.StartTutorialLab();
            GameDatas.isTutLab = true;
        }
        EventDispatcher.PostEvent(EventID.OnRefeshUIHome, 0);
    }

    private void HandleUnlockWorkShopRewards()
    {
        GameDatas.UnlockClusterUpgrader(milestoneRewardData.upgraderCategory);
        //EventDispatcher.PostEvent(EventID.OnRefeshUIUpgrade, 0);

        TabUpgraderManager.instance.OnRefreshClusterUpgrader();
    }

    private void HandleUnlockAvatar(TypeMilestone type)
    {
        
        GameDatas.UnlockAvatar(ConvertToTypeAvatar(type));
        EventDispatcher.PostEvent(EventID.OnRefeshAvatarUnlock, 0);
    }

    public static TypeAvatar ConvertToTypeAvatar(TypeMilestone milestone)
    {
        return milestone switch
        {
            TypeMilestone.AVATAR_QUANTUM_CANNON => TypeAvatar.AVATAR_QUANTUM_CANNON,
            TypeMilestone.AVATAR_PHOTON_BLASTER => TypeAvatar.AVATAR_PHOTON_BLASTER,
            TypeMilestone.AVATAR_CYBERNOVA => TypeAvatar.AVATAR_CYBERNOVA,
            _ => throw new ArgumentException($"Không thể chuyển {milestone} sang TypeAvatar"),
        };
    }

    private void HandleLabRewards()
    {
        GameDatas.UnlockClusterLabInfor(milestoneRewardData.labCategory);
        EventDispatcher.PostEvent(EventID.OnRefeshUILabs, 0);
    }

    private void HandleWorldRewards(TypeMilestone type)
    {
        int world = GetWorldNumber(type) - 1;
        GameDatas.SetHighestWorld(world);
        EventDispatcher.PostEvent(EventID.OnRefeshUIHome, 0);
    }

    //log event unlock new World
    public static void LogEventLongToUnlockNewWorld(int world)
    {
        DateTime startTime = DateTime.Parse(GameDatas.StartTimeFirstInGame);
        DateTime now = DateTime.UtcNow;
        TimeSpan elapsedTime = now - startTime;

        GameAnalytics.LogEventLongToUnlockNewWorld(elapsedTime ,world);
    }

    public int GetWorldNumber(TypeMilestone world)
    {
        string worldName = world.ToString();

        string numericPart = worldName.Substring("WORLD_".Length);

        int worldNumber;
        if (int.TryParse(numericPart, out worldNumber))
        {
            return worldNumber;
        }
        else
        {
            Debug.LogWarning("Failed to parse world number from enum");
            return -1;
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventID.OnClaimAllItemReward, OnClaimAllItemReward);
    }

    private void OnClaimAllItemReward(object o)
    {
        OnClick();
    }
}
