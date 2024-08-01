using Fusion;
using System.Collections;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;
using static ServerData;

public class UserCharacterController :MonoBehaviour// NetworkBehaviour
{
    [SerializeField] KeyController keyController;
    [SerializeField] InventoryController inventory;
    [SerializeField] AbilityController ability;


    [SerializeField] string userId;
    [SerializeField] string characterId;
    
    [SerializeField] CharacterServerData characterData;


    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        keyController = new(input.actions);
    }

    [ContextMenu("Do Something")]
    public void Load()
    {
        Manager.FireBase.LoadCharacter(userId, characterId, LoadCharacterData);
    }

    void LoadCharacterData(CharacterServerData _characterData)
    {
        characterData = _characterData;
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
