using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static ServerData;


[RequireComponent(typeof(PlayerInput))]
public class UserCharacterController : MonoBehaviour// NetworkBehaviour
{
    internal KeyController KeyController { get { return keyController; } }
    internal InventoryController Inventory { get { return inventory; } }

    [SerializeField] KeyController keyController;
    [SerializeField] InventoryController inventory;

    [SerializeField] AbilityController ability;
    [SerializeField] CharacterServerData characterData;

    [SerializeField] string userId;
    [SerializeField] string characterId;
    


    private void Start()
    {
        keyController = new(GetComponent<PlayerInput>().actions, this);
    }

    [ContextMenu("Do Swap")]
    public void Test()
    {
        keyController.TestCode();
    }
    [ContextMenu("Do Something")]
    public void Load()
    {
        Manager.FireBase.LoadCharacter(userId, characterId, LoadCharacterData);
    }

    void LoadCharacterData(CharacterServerData _characterData)
    {
        if(_characterData == null)
        {
            Message.LogWarning($"characterData Load Failed");
            return;
        }
        characterData = _characterData;
        PlayerInput input = GetComponent<PlayerInput>();
        
        keyController = new(input.actions, this);
        KeyController.LoadKeySet(_characterData.keySet);

        StartCoroutine(InventoryInitRoutine());
    }

    IEnumerator InventoryInitRoutine()
    {
        while (Manager.Data.GameItemData == null)
            yield return new WaitForSeconds(0.15f);
        inventory = new(characterData.inventory);
    }



    //public override void FixedUpdateNetwork()
    //{
    //    base.FixedUpdateNetwork();
    
    //}

}
public partial class KeyController { }
public partial class InventoryController { }