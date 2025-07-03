using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RankData", menuName = "Data/Arena/SO_RankData", order = 0)]
public class SO_RankData : SerializedScriptableObject
{
    public Dictionary<TypeRank, List<RankData>> rewardRankDicts = new();
    public List<string> textRank = new();


    private int GetIndexRange(int indexRank)
    {
        if (indexRank == 1) return 0;
        if (indexRank == 2) return 1;
        if (indexRank >= 3 && indexRank <= 4) return 2;
        if (indexRank >= 5 && indexRank <= 6) return 3;
        if (indexRank >= 7 && indexRank <= 8) return 4;
        if (indexRank >= 9 && indexRank <= 10) return 5;
        if (indexRank >= 11 && indexRank <= 12) return 6;
        if (indexRank >= 13 && indexRank <= 15) return 7;
        if (indexRank >= 16 && indexRank <= 22) return 8;
        if (indexRank >= 23 && indexRank <= 30) return 9;

        return 0;
    }

    /*[Button("Test GetData")]
    public void GetData()
    {
        int indexRank = 7;
        TypeRank typeRank = TypeRank.silver;

        RankData results = GetRankDataByIndex(GetIndexRange(indexRank), typeRank);
        DebugCustom.LogColor(results.gem);
    }*/

    public RankData GetRankDataByIndex(int indexRank, TypeRank typeRank)
    {
        int index = GetIndexRange(indexRank);
        if(rewardRankDicts.ContainsKey(typeRank))
        {
            return rewardRankDicts[typeRank][index];
        }
        return null;
    }

    public List<RankData> GetAllRankDataByIndex(TypeRank typeRank)
    {
        if (rewardRankDicts.ContainsKey(typeRank))
        {
            return rewardRankDicts[typeRank];
        }
        return null;
    }

/*    [Button("Add Data")]
    public void AdData()
    {
        int[,] data =
        {
            { 100, 20, 200, 40, 300, 80, 400, 160, 600, 320, 800, 425 },
            { 80, 18, 150, 35, 250, 70, 350, 140, 500, 300, 700, 400 },
            { 65, 16, 100, 30, 200, 60, 300, 120, 400, 280, 600, 375 },
            { 50, 12, 75, 20, 150, 40, 250, 70, 350, 200, 500, 350 },
            { 45, 19, 65, 19, 125, 30, 225, 65, 325, 175, 475, 325 },
            { 40, 9, 60, 18, 100, 28, 200, 60, 300, 150, 450, 300 },
            { 30, 8, 55, 17, 90, 26, 175, 56, 275, 125, 425, 275 },
            { 20, 7, 50, 16, 80, 24, 150, 53, 250, 100, 400, 250 },
            { 15, 6, 45, 14, 70, 22, 125, 50, 200, 90, 375, 225 },
            { 10, 5, 40, 12, 50, 20, 100, 20, 150, 20, 200, 120 }
        };

        foreach (TypeRank type in System.Enum.GetValues(typeof(TypeRank)))
        {
            rewardRankDicts[type] = new List<RankData>();
        }

        for (int i = 0; i < data.GetLength(0); i++)
        {
            rewardRankDicts[TypeRank.Recruit].Add(new RankData(data[i, 0], data[i, 1]));
            rewardRankDicts[TypeRank.Private].Add(new RankData(data[i, 2], data[i, 3]));
            rewardRankDicts[TypeRank.Sergeant].Add(new RankData(data[i, 4], data[i, 5]));
            rewardRankDicts[TypeRank.Captain].Add(new RankData(data[i, 6], data[i, 7]));
            rewardRankDicts[TypeRank.Major].Add(new RankData(data[i, 8], data[i, 9]));
            rewardRankDicts[TypeRank.General].Add(new RankData(data[i, 10], data[i, 11]));
        }
    }*/
}

[Serializable]
public class RankData
{
    public int gem;
    public int powerStone;

    public RankData(int gem, int powerStone)
    {
        this.gem = gem;
        this.powerStone = powerStone;
    }
}
