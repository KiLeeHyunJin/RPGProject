using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;

public class CreateCharacter : MonoBehaviour
{


    public async void CheckCharacterId(string characterId)
    {
        bool exists = await CharacterIdExists(characterId);

        string showTxt;
        if (exists)
            showTxt = "이미 사용중인 닉네임입니다.";
        else
            showTxt = "사용가능한 닉네임입니다.";

        Utils.ShowInfo(showTxt);
    }




    async Task<bool> CharacterIdExists(string characterId)
    {
        DataSnapshot usersSnapshot = await FireBaseManager.DB.RootReference.Child("users").GetValueAsync();

        // 모든 userId를 순회하면서 characterId 확인
        foreach (var user in usersSnapshot.Children)
        {
            if (user.Child("characters").HasChildren)
            {
                foreach (var character in user.Child("characters").Children)
                {
                    if (character.Key == characterId)
                    {
                        return true; // 존재하면 true 반환
                    }
                }
            }
        }
        return false; // 존재하지 않으면 false 반환
    }
}
