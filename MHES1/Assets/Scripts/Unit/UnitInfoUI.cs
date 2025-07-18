using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text statsText;
    public Image ownerColorImage;

    public Color[] playerColors;

    public void Show(Unit unit)
    {
        if (unit == null)
        {
            Hide();
            return;
        }

        nameText.text = unit.name;
        hpText.text = $"HP: {unit.stats.currentHealth} / {unit.stats.maxHealth}";
        statsText.text = $"DMG: {unit.stats.damage}\nDEF: {unit.stats.defense}";

        Debug.Log(ownerColorImage != null && unit.ownerPlayerID < playerColors.Length);

        if (ownerColorImage != null && unit.ownerPlayerID < playerColors.Length)
            ownerColorImage.color = playerColors[unit.ownerPlayerID];

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}