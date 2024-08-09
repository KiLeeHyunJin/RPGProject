using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static ServerData;
/// <summary>
/// 유저 캐릭터 컨트롤러
/// </summary>
public class UserCharacterController : MonoBehaviour// NetworkBehaviour
{
    public KeyController KeyController              { get { return keyController; } }
    public InventoryController InventoryController  { get { return inventoryController; } }
    public SkillController SkillController          { get { return skillController; } }
    public AbilityController AbilityController      { get { return abilityController; } }

    [SerializeField] KeyController          keyController;           //키
    [SerializeField] InventoryController    inventoryController;         //아이템
    [SerializeField] SkillController        skillController;
    [SerializeField] AbilityController      abilityController;             //능력치


    [SerializeField] ScriptableSkillBundle  skillBundle;
    [SerializeField] CharacterServerData    characterData;

    [SerializeField] string userId;
    [SerializeField] string characterId;


    private void Awake()
    {

    }

    private void Start()
    {
        keyController = new(this, gameObject.GetOrAddComponent<PlayerInput>());
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
        if (_characterData == null)
        {
            Message.LogWarning($"characterData Load Failed");
            return;
        }
        characterData = _characterData;

        StartCoroutine(InventoryInitRoutine());

        abilityController = new(this, _characterData.ability);
        skillController = new(this, skillBundle, _characterData.skill);

        keyController = new(this, gameObject.GetOrAddComponent<PlayerInput>());
        KeyController.LoadKeySet(_characterData.keySet);

    }

    IEnumerator InventoryInitRoutine()
    {
        while (Manager.Data.GameItemData == null)
            yield return new WaitForSeconds(0.15f);
        inventoryController = new(this, characterData.inventory);
    }



    //public override void FixedUpdateNetwork()
    //{
    //    base.FixedUpdateNetwork();

    //}




}