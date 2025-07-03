using UnityEngine;
using UnityEngine.UI;

public class ButtonTab_ChangeColor : ButtonTab
{
    [SerializeField] Color normalColor, selectedColor;

    Image image;

    public override void Activate (bool value)
    {
        image.color = value ? selectedColor : normalColor;
    }

    private void Awake ()
    {
        image = GetComponent<Image> ();
    }
}
