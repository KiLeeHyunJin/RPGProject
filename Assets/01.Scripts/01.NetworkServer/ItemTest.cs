using UnityEngine;

public class ItemTest : MonoBehaviour
{
    [SerializeField] UserData.Item item;
    [SerializeField] ClientItemData itemData;

    public void SaveItem()
    {
        item.CapsuleItemData(itemData);
    }

    public void ParseItem()
    {
        itemData.ParseItemData(item);
    }
}
