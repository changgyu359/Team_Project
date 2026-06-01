using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private SlotUI slotPF;
    [SerializeField]
    private GameObject inventoryPannel;
    [SerializeField]
    private Inventory inven;

    private SlotUI[] slotUIArray = new SlotUI[8];


    private void Start()
    {
        for(int i = 0; i < slotUIArray.Length; i++)
        {
            SlotUI slotUI = Instantiate(slotPF,inventoryPannel.transform);
            slotUIArray[i] = slotUI;
        }
        Redraw();
    }

    public void Redraw()
    {
        for(int i= 0; i < slotUIArray.Length; i++)
        {
            slotUIArray[i].ShowSlot(inven.GetSlot(i));
        }
    }

}
