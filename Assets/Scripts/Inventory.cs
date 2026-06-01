using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Slot[] inventory = new Slot[8];

    private void Awake()
    {
        for(int i=0;i<inventory.Length;i++)
            inventory[i] = new Slot();
    }

    [SerializeField]
    private Item testitem;
    [SerializeField]
    private Item testitem2;

    /// <summary>
    /// 인벤토리에 아이템을 하나만 추가하는 메서드
    /// </summary>
    /// <param name="_item"></param>
    public void AddItem(Item _item)
    {
        //먼저 같은 아이템이 있는지 찾고 있으면 수량 증가
        int index = FindFirstSameItem(_item.Data);
        if (index!=-1)
            inventory[index].ItemUp(1);
        else
        {
            //같은 아이템이 없다면 빈 슬롯이 있는지 찾아봄, 없으면 꽉 찼다는 뜻
            index = FindFirstEmptySlot();
            if (index != -1)
                inventory[index].SetItem(_item.Data);
            else
                Debug.Log("인벤토리가 꽉 찼습니다!");
        }
        
    }

    


    ///// <summary>
    ///// 해당 인덱스의 아이템을 갯수만큼 줄임. 수량이 0이 되면 해당 인덱스의 SO데이터를 없앰.
    ///// </summary>
    ///// <param name="_index"></param>
    ///// <param name="_count"></param>
    //public void RemoveItemIndex(int _index,int _count)
    //{
    //    inventory[_index].ItemDown(_count);
    //    if (inventory[_index].isEmpty())
    //        inventory[_index].ItemClear();
    //}

    /// <summary>
    /// 해당 아이템이 있는지 체크, 있으면 가장 앞의 슬롯에서 1개 제거
    /// </summary>
    /// <param name="_item"></param>
    public void RemoveItem(Item _item)
    {
        int index=FindLastSameItem(_item.Data);
        if (index == -1)
            Debug.LogError("아이템이 없습니다!");
        else
        {
            inventory[index].ItemDown(1);
            if (inventory[index].isEmpty())
                inventory[index].ItemClear();
        }
        
    }

    private int FindFirstSameItem(ItemSO _data)
    {
        for(int i=0;i<inventory.Length;i++)
        {
            if (inventory[i].CurItemData == _data&&!inventory[i].isFull())
                return i;
        }
        return -1;
    }

    private int FindLastSameItem(ItemSO _data)
    {
        for (int i = inventory.Length-1; i >=0 ; i--)
        {
            if (inventory[i].CurItemData == _data)
                return i;
        }
        return -1;
    }

    private int FindFirstEmptySlot()
    {
        for(int i=0;i<inventory.Length;i++)
        {
            if (!inventory[i].IsItem)
                return i;
        }
        return -1;
    }

    public void AddManyItem(Item _item,int _count)
    {
        // 인벤토리의 빈공간을 체크, 빈공간이 더 많으면 실행, 그렇지 않으면 거부
        if(HowManyItemSpace(_item)>=_count)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                // 아이템이 있는 슬롯을 찾았고 그 슬롯이 꽉 안찼으면
                if (inventory[i].CurItemData == _item.Data && !inventory[i].isFull())
                {
                    // 채우고 끝
                    if (inventory[i].RemainToFull() > _count)
                    {
                        inventory[i].ItemUp(_count);
                        return;
                    }
                    // 아직 더 채워야함
                    else
                    {
                        int remain = inventory[i].RemainToFull();
                        inventory[i].ItemUp(remain);
                        _count -= remain;
                    }
                }
            }
            //코드가 여기를 진행한다는것은 아직 더 채워야 한다는 뜻
            for (int i = 0; i < inventory.Length; i++)
            {
                if (!inventory[i].IsItem)
                {
                    // 채우고 끝
                    if (_item.Data.max >= _count)
                    {
                        inventory[i].SetItem(_item.Data, _count);
                        return;
                    }
                    // 아직 더 채워야함
                    else
                    {
                        inventory[i].SetItem(_item.Data, _item.Data.max);
                        _count -= _item.Data.max;
                    }

                }
            }
        }
        else
        {
            Debug.LogError("인벤토리가 초과되었습니다!");
        }    
        

    }

    // AddManyItem을 발동하기전 빈공간의 수를 리턴하는 메서드
    private int HowManyItemSpace(Item _item)
    {
        int totalSpace = 0;

        for (int i = 0; i < inventory.Length; i++)
        {
           
            if (inventory[i].CurItemData == _item.Data && !inventory[i].isFull())
            {
                totalSpace += inventory[i].RemainToFull();
            }
        }
       
        for (int i = 0; i < inventory.Length; i++)
        {
            if (!inventory[i].IsItem)
            {
                totalSpace += _item.Data.max;

            }
        }

        return totalSpace;
    }


    public Slot GetSlot(int index)
    { return inventory[index]; }


    public void testAdd()
    {
        AddItem(testitem);
    }

    public void testAdd2()
    {
        AddItem(testitem2);
    }

    public void testRemove1()
    {
        RemoveItem(testitem);
    }

    public void testAddMany()
    {
        AddManyItem(testitem, 10);
    }

    public void testClear()
    {
        if (inventory[1].IsItem)
            inventory[1].ItemClear();
    }

    
}
