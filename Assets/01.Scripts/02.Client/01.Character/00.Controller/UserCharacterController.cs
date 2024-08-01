using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static ServerData;
public partial class KeyController { }
public partial class InventoryController { }

public class UserCharacterController :MonoBehaviour// NetworkBehaviour
{
    [SerializeField] KeyController keyController;
    internal KeyController KeyController        { get { return keyController; } }
    [SerializeField] InventoryController inventory;
    internal InventoryController Inventory      { get { return inventory; }}

    [SerializeField] AbilityController ability;


    [SerializeField] string userId;
    [SerializeField] string characterId;
    
    [SerializeField] CharacterServerData characterData;


    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        keyController = new(input.actions, this);
    }
    [ContextMenu("ResetKey")]
    public void ResetKey()
    {
        keyController.ResetKey();
    }
    [ContextMenu("Do Something")]
    public void Load()
    {
        Manager.FireBase.LoadCharacter(userId, characterId, LoadCharacterData);
    }

    void LoadCharacterData(CharacterServerData _characterData)
    {
        characterData = _characterData;
        PlayerInput input = GetComponent<PlayerInput>();
        
        keyController = new(input.actions, this);
        keyController.LoadKeySet(_characterData.keySet);

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
