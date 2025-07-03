using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabThemeChange : GameMonoBehaviour
{
    [SerializeField]
    private List<ItemMusicChange> itemMusicChanges = new();
    [SerializeField]
    private ThemeSongSO themeSongSO;
    public GameObject curSelectMusic;

    private void Start()
    {
        BuildData();
    }

    private void BuildData()
    {
        for (int i = 0; i < itemMusicChanges.Count; i++)
        {
            TypeSong typeSong = (TypeSong)i + 1;
            itemMusicChanges[i].OnSelected = ItemSelected;
            itemMusicChanges[i].SetUp(typeSong, themeSongSO.themeSongDicts[typeSong]);
        }
    }

    public void ItemSelected(GameObject selected)
    {
        if (curSelectMusic != null)
        {
            curSelectMusic.SetActive(false);
        }
        curSelectMusic = selected;
        curSelectMusic.SetActive(true);
    }
}
