using UnityEngine;

public class ItemTest : MonoBehaviour
{
    [SerializeField] UserData.Item item;
    [SerializeField] DecapsuleItemData itemData;

    public void SaveItem()
    {
        item.SaveItemData(itemData);
    }

    public void ParseItem()
    {
        itemData.ParseItemData(item);
    }
}
