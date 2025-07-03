using DanielLochner.Assets.SimpleScrollSnap;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupTechSystem : UIPanel, IBoard
{

    [SerializeField]
    private SimpleScrollSnap simpleScrollSnap;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject panelInfor;
    [SerializeField]
    private List<GameObject> tabTechs;
    public int priceBuyX1;
    public int priceBuyX10;

    private List<ItemTechView> itemTabInventory = new();
    private List<ItemTechView> itemTabMerge = new();
    private List<ItemTechView> itemTabShatter = new();

    [SerializeField]
    private Transform contentInventory;
    [SerializeField]
    private Transform contentMerge;
    [SerializeField]
    private Transform contentShatter;

    [SerializeField]
    private ItemTechView itemTechView;
    [SerializeField]
    private TMP_Text txtCountTech;
    public int maxSlotTech = 300;

    public GameObject panelFilterClass;
    public bool isOpenFilterClass;
    public GameObject panelFilterRarity;
    public bool isOpenFilterRarity;

    private static readonly Dictionary<TypeClassTechSystem, List<TypeTech>> _techByClass = new()
    {
        [TypeClassTechSystem.CompositeArmor] = new()
    {
        TypeTech.AegisFortitude,
        TypeTech.TitaniumBastion,
        TypeTech.EarthforgedBulwark,
        TypeTech.PhoenixWard
    },
        [TypeClassTechSystem.HybridPower] = new()
    {
        TypeTech.OblivionsEdge,
        TypeTech.Stormbreaker,
        TypeTech.FangOfTheVoid,
        TypeTech.BlazetongueReape
    },
        [TypeClassTechSystem.Engine] = new()
    {
        TypeTech.ProsperitysGrasp,
        TypeTech.VortexExcavator,
        TypeTech.AlchemistsSatchel,
        TypeTech.MidasTouch
    },
        [TypeClassTechSystem.Cryogenic] = new()
    {
        TypeTech.ArcaneCodex,
        TypeTech.CelestialScepter,
        TypeTech.GrimoireAbyss,
        TypeTech.AstralConduit
    }
    };

    public override UiPanelType GetId()
    {
        return UiPanelType.PopupTechSystem;
    }

    private void OnEnable()
    {
        OnAppear();
    }

    public override void OnAppear()
    {
        if (isInited)
            return;

        base.OnAppear();

        Init();
    }

    public void OpenTab(int indexTab)
    {
        foreach (GameObject tab in tabTechs)
        {
            tab.SetActive(false);
        }
        tabTechs[indexTab].SetActive(true);
        scrollRect.enabled = true;
        simpleScrollSnap.GoToPanel(indexTab);
        scrollRect.enabled = false;
    }

    public void ButtonInfo()
    {

    }

    public void BuyModuleX1()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceBuyX1, OnBuyX1Success);
    }

    public void BuyModuleX10()
    {
        GameDatas.BuyUsingCurrency(CurrencyType.GEM, priceBuyX10, OnBuyX10Success);
    }

    public void OnBuyX1Success(bool success)
    {
        if(success)
        {
            Roll();
        }
    }
    public void OnBuyX10Success(bool success)
    {
        if (success)
        {
            for(int i = 0; i < 10; i++)
            {
                Roll();
            }
        }
    }

    private static readonly System.Random _random = new();

    public static (TypeClassTechSystem classType, TypeTech techType) RollRandomTech()
    {
        // Step 1: Random class
        TypeClassTechSystem classType = GetRandomEnumValue<TypeClassTechSystem>();

        // Step 2: Random tech within the class group
        List<TypeTech> techList = _techByClass[classType];
        TypeTech techType = techList[_random.Next(techList.Count)];

        return (classType, techType);
    }

    private static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(_random.Next(values.Length));
    }

    public void Roll()
    {
        // Roll tech
        var result = RollRandomTech();
        DebugCustom.LogColor($"Class: {result.classType}, Tech: {result.techType}");

        // Roll rarity using pity system
        int rollCount = GameDatas.GetPitySystem();
        rollCount++;
        GameDatas.SetPitySystem(rollCount);

        TypeRarityTech typeRarity;

        // Guaranteed Rare at 100 rolls
        if (rollCount >= 100)
        {
            GameDatas.SetPitySystem(0);
            typeRarity = TypeRarityTech.Rare;
        }
        // Guaranteed Uncommon every 10 rolls
        else if (rollCount % 10 == 0)
        {
            int roll = _random.Next(0, 1000);
            if (roll < 25)
            {
                GameDatas.SetPitySystem(0);
                typeRarity = TypeRarityTech.Rare;
            }
            else
            {
                typeRarity = TypeRarityTech.UnCommon;
            }
        }
        else
        {
            int chance = _random.Next(0, 1000);
            if (chance < 25)
            {
                GameDatas.SetPitySystem(0);
                typeRarity = TypeRarityTech.Rare;
            }
            else if (chance < 315)
            {
                typeRarity = TypeRarityTech.UnCommon;
            }
            else
            {
                typeRarity = TypeRarityTech.Common;
            }
        }

        // Spawn views
        AddTechView(result.techType, result.classType, typeRarity, contentInventory, 1);
        AddTechView(result.techType, result.classType, typeRarity, contentMerge, 1);
        AddTechView(result.techType, result.classType, typeRarity, contentShatter, 1);
    }

    private void AddTechView(TypeTech typeTech, TypeClassTechSystem classType, TypeRarityTech rarity, Transform parent, int level)
    {
        ItemTechView view = Instantiate(itemTechView, parent);

        bool isUniqueEffect = 10f.Chance();
        ItemTechSystemData cellData = new ItemTechSystemData(typeTech, rarity,
            classType, 1, isUniqueEffect, GameDatas.GetUniqueTechSystem());

        GameDatas.SetUniqueTechSystem();
        TechSystemStorage.AddItem(cellData);

        view.SetUp(cellData);

        if (parent == contentInventory)
        {
            itemTabInventory.Add(view);
        }
        if (parent == contentMerge)
        {
            itemTabMerge.Add(view);
        }
        if (parent == contentShatter)
        {
            itemTabShatter.Add(view);
        }

        ChangeTxtCountTech();
    }

    private void CreateTechView(ItemTechSystemData cellData, Transform parent)
    {
        ItemTechView view = Instantiate(itemTechView, parent);
        view.SetUp(cellData);

        if(parent == contentInventory)
        {
            itemTabInventory.Add(view);
        }
        if (parent == contentMerge)
        {
            itemTabMerge.Add(view);
        }
        if (parent == contentShatter)
        {
            itemTabShatter.Add(view);
        }

        ChangeTxtCountTech();
    }

    private void Init()
    {
        LoadDataTech();
    }

    private void LoadDataTech()
    {
        List<ItemTechSystemData> itemTechSystemDatas = TechSystemStorage.LoadData().items;
        if (itemTechSystemDatas.Count > 0)
        {
            foreach(ItemTechSystemData cellData in itemTechSystemDatas)
            {
                CreateTechView(cellData, contentInventory);
                CreateTechView(cellData, contentMerge);
                CreateTechView(cellData, contentShatter);
            }
        }
        ChangeTxtCountTech();
    }

    private void ChangeTxtCountTech()
    {
        txtCountTech.text = $"{TechSystemStorage.LoadData().items.Count}/{maxSlotTech}";
    }

    public void FilterTechByClassTech()
    {
        isOpenFilterClass = !isOpenFilterClass;
        panelFilterClass.SetActive(isOpenFilterClass);
    }

    public void FilterTechByRarityTech()
    {
        isOpenFilterRarity = !isOpenFilterRarity;
        panelFilterRarity.SetActive(isOpenFilterRarity);
    }

    public void TabFilterClass(int index)
    {
        for (int i = 0; i < itemTabInventory.Count; i++)
        {
            itemTabInventory[i].gameObject.SetActive(true);
            itemTabMerge[i].gameObject.SetActive(true);
            itemTabShatter[i].gameObject.SetActive(true);
        }

        if (index != 4)
        {
            TypeClassTechSystem filterType = (TypeClassTechSystem)index;

            for (int i = 0; i < itemTabInventory.Count; i++)
            {
                if (itemTabInventory[i].cellData.typeClassTechSystem != filterType)
                {
                    itemTabInventory[i].gameObject.SetActive(false);
                    itemTabMerge[i].gameObject.SetActive(false);
                    itemTabShatter[i].gameObject.SetActive(false);
                }
            }
        }

    }

    public List<GameObject> selectedFilter = new();

    public void TabFilterRarity(int index)
    {
        TypeRarityTech filterType = (TypeRarityTech)index;

        selectedFilter[index].SetActive(!selectedFilter[index].activeSelf);
        if(!selectedFilter[index].activeSelf)
        {
            for (int i = 0; i < itemTabInventory.Count; i++)
            {
                if (itemTabInventory[i].cellData.typeRarityTech == filterType)
                {
                    itemTabInventory[i].gameObject.SetActive(false);
                    itemTabMerge[i].gameObject.SetActive(false);
                    itemTabShatter[i].gameObject.SetActive(false);
                }
            }
        } else
        {
            for (int i = 0; i < itemTabInventory.Count; i++)
            {
                if (itemTabInventory[i].cellData.typeRarityTech == filterType)
                {
                    itemTabInventory[i].gameObject.SetActive(true);
                    itemTabMerge[i].gameObject.SetActive(true);
                    itemTabShatter[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void OpenPanelInfor()
    {
        panelInfor.SetActive(true);
    }

    public void ClosePanelInfor()
    {
        panelInfor.SetActive(false);
    }

    private void OnDisable()
    {
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }

    protected override void UnregisterEvent()
    {
        base.UnregisterEvent();
    }

    public override void OnDisappear()
    {
        base.OnDisappear();
    }

    public void OnClose()
    {
    }

    public void OnBegin()
    {
    }
}
