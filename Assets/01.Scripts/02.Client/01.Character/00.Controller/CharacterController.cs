using Fusion;
using System.Collections;
using UnityEditor.U2D.Animation;
using UnityEngine;
using static ServerData;

public class CharacterController : NetworkBehaviour
{
    InventoryController inventory;
    AbilityController ability;

    KeyController keyController;

    [SerializeField] string userId;
    [SerializeField] string characterId;
    
    [SerializeField] CharacterServerData characterData;


    private void Awake()
    {

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



    private void Start()
    {

    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
    
    }

}
