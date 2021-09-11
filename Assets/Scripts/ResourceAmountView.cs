using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAmountView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _icon;

    public void Show(Sprite icon, int count)
    {
        _label.text = count.ToString();
        _icon.sprite = icon;
    }
}
