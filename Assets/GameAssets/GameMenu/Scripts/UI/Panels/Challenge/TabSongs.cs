using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSongs : GameMonoBehaviour
{
    [SerializeField]
    private List<ItemSongChallenge> itemSongChallenges = new();
    [SerializeField]
    private ThemeSongSO themeSongSO;

    private void Start()
    {
        BuildData();
    }

    public void BuildData()
    {
        itemSongChallenges[0].SetUp(TypeSong.IntheEnd, themeSongSO.themeSongDicts[TypeSong.IntheEnd]);
        itemSongChallenges[1].SetUp(TypeSong.Movement, themeSongSO.themeSongDicts[TypeSong.Movement]);
        itemSongChallenges[2].SetUp(TypeSong.Nothing, themeSongSO.themeSongDicts[TypeSong.Nothing]);
    }

}
