using UnityEngine;
using UnityEngine.UI;
using static UserData;

public class CharacterManager : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField characterNameInputField;
    public InputField characterClassInputField;
    public InputField characterLevelInputField;
    public Text resultText;

    const int defaultSlotCount = 10;
    public void OnCreateAccountButtonClicked()
    {
        string userId = System.Guid.NewGuid().ToString();
        User user = new User();
        Manager.FireBase.SaveUser(userId, user);
        resultText.text = "Account created.";
    }

    public void OnCreateCharacterButtonClicked()
    {
        string userId = "<user-id>"; // 로그인한 사용자 ID를 사용
        string characterId = System.Guid.NewGuid().ToString();
        string characterName = characterNameInputField.text;
        int characterLevel = int.Parse(characterLevelInputField.text);
        string characterClass = characterClassInputField.text;

        Character character = new()
        {
            nickName = characterName,
            level = characterLevel,
            job = characterClass,
            ability = new Ability
            {
                accuracy = 10.0f,
                atckPower = 20.0f,
                atckSpeed = 1.5f,
                defence = 5.0f,
                jumpPower = 2.0f,
                magicPower = 15.0f,
                moveSpeed = 3.0f,
                point = 100
            },
            stat = new Stat
            {
                str = 4,
                def = 4,
                man = 4,
                luk = 4
            },
            inventory = new Inventory
            {
                consume = new(defaultSlotCount),
                ect = new(defaultSlotCount),
                equip = new(defaultSlotCount),
                money = new(defaultSlotCount)
            },
            skill = "Basic Attack"
        };

        Manager.FireBase.SaveCharacter(userId, characterId, character);
        resultText.text = "Character created.";
    }

    public void OnLoadUserButtonClicked()
    {
        string userId = "<user-id>"; // 로그인한 사용자 ID를 사용
        Manager.FireBase.LoadUser(userId, (user) =>
        {
            if (user != null)
            {
                // user 데이터를 사용하여 UI 업데이트
                resultText.text = "User loaded successfully.";
            }
            else
            {
                resultText.text = "User does not exist.";
            }
        });
    }

    public void OnLoadCharacterButtonClicked()
    {
        string userId = "<user-id>"; // 로그인한 사용자 ID를 사용
        string characterId = "<character-id>"; // 불러올 캐릭터 ID
        Manager.FireBase.LoadCharacter(userId, characterId, (character) =>
        {
            if (character != null)
            {
                // character 데이터를 사용하여 UI 업데이트
                resultText.text = "Character loaded successfully.";
            }
            else
            {
                resultText.text = "Character does not exist.";
            }
        });
    }
}
