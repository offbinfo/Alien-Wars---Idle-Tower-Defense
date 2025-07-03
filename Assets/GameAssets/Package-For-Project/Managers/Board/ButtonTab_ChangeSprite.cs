using UnityEngine;
using UnityEngine.UI;

public class ButtonTab_ChangeSprite : ButtonTab
{
    [SerializeField] Sprite normalSprite, selectedSprite;

    Image image;

    public override void Activate (bool value)
    {
        image.sprite = value ? selectedSprite : normalSprite;
    }

    private void Awake ()
    {
        image = GetComponent<Image> ();
    }
}
