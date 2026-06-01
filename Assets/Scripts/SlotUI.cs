using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
   
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI itemCount;

    public void ShowSlot(Slot _slot)
    {
        if(_slot.CurItemData == null)
        {
            image.gameObject.SetActive(false);
            itemCount.text = "";
        }
        else
        {
            image.gameObject.SetActive(true);
            image.sprite = _slot.CurItemData.itemImage;
            itemCount.text = _slot.CurItemMany.ToString();
        }


         
    }

    
}
