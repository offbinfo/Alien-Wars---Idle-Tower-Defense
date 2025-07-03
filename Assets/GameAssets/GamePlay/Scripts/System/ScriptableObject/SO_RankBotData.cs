using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RankBotData", menuName = "Data/Arena/SO_RankBotData", order = 0)]
public class SO_RankBotData : SerializedScriptableObject
{

    public List<BotData> botList = new List<BotData>();

    [Button("Add Bot")]
    private void AddBot()
    {
        botList.Clear();
        for (int i = 0; i < 30; i++)
        {
            botList.Add(new BotData
            {
                nameType = (NameUserType)Random.Range(0, System.Enum.GetValues(typeof(NameUserType)).Length),
                indexRank = i + 1,
                wave = Random.Range(1, 100),
                difficulty = (BotDifficulty)Random.Range(0, 3)
            });
        }
        AdjustBotWaves(30, 1);
    }

    public int rank;
    public int wave;
    [Button("Test ShuffleWave")]
    private void TestShuffleWave()
    {
        int indexRankPlayer = rank;
        int wavePlayer = wave;

        AdjustBotWaves(indexRankPlayer, wavePlayer);
    }

    [Button("Test ShuffleBotRanks")]
    private void TestShuffleBotRanks()
    {
        int indexRankPlayer = rank;
        int wavePlayer = wave;

        ShuffleBotRanks();
        AdjustBotWaves(indexRankPlayer, wavePlayer);
    }

    public IEnumerator SwapWaveBotRanks(int indexRankPlayer, int wavePlayer)
    {
        yield return null;
        AdjustBotWaves(indexRankPlayer, wavePlayer);
    }

    public IEnumerator SwapBotRanks(int indexRankPlayer, int wavePlayer)
    {
        yield return null;
        ShuffleBotRanks();
        AdjustBotWaves(indexRankPlayer, wavePlayer);
    }

    private void AdjustBotWaves(int indexRankPlayer, int wavePlayer)
    {
        botList = botList.OrderBy(bot => bot.indexRank).ToList();

        for (int i = 0; i < botList.Count; i++)
        {
            var bot = botList[i];
            if (bot.indexRank < indexRankPlayer)
            {
                // Các bot có indexRank nhỏ hơn (xếp hạng cao hơn) sẽ có wave cao hơn, tịnh tiến dần
                bot.wave = wavePlayer + (indexRankPlayer - bot.indexRank) * 8;
                bot.wave += Random.Range(1, 5);
            }
            else
            {
                // Các bot có indexRank lớn hơn (xếp hạng thấp hơn) sẽ có wave thấp hơn
                bot.wave = wavePlayer - (bot.indexRank - indexRankPlayer) * 8;
                bot.wave -= Random.Range(1, 5);
                if (bot.wave < 1) bot.wave = 0;
            }
        }
    }

    private void ShuffleBotRanks()
    {
        botList = botList.OrderBy(bot => Random.value).ToList();
        for (int i = 0; i < botList.Count; i++)
        {
            botList[i].indexRank = i + 1;
        }
    }
}

[System.Serializable]
public class BotData
{
    public NameUserType nameType;
    public int indexRank;
    public int wave;
    public BotDifficulty difficulty;
}
