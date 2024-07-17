using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    [SerializeField] UserData.Stat stat;
    [SerializeField] string userId;
    [SerializeField] string nickName;

    [SerializeField] Button create;
    [SerializeField] Button remove;
    [SerializeField] Button check;

    private void Start()
    {
        create.onClick.AddListener(CreateCharacterData);
        remove.onClick.AddListener(RemoveCharacterData);
        check.onClick.AddListener(CheckCharacterNickName);
    }


    void InitUserId()
    {
        userId = FireBaseManager.Auth.CurrentUser.UserId;
    }

    async void CheckCharacterNickName()
    {
        bool usernameExists = await CheckIfUsernameExists(nickName);
        string msg = usernameExists ? "사용할 수 없는 닉네임입니다." : "사용가능한 닉네임입니다.";
        Utils.ShowInfo(msg);
    }

    void CreateCharacterData()
    {
        UserData.Character uploadCharacterData = 
            new() { nickName = this.nickName, stat = this.stat };
        UserData.Name saveName = 
            new() { nickName = this.nickName };

        string nickJson = JsonUtility.ToJson(saveName);
        string json = JsonUtility.ToJson(uploadCharacterData);

        FireBaseManager.DB.GetReference(Define.SearchNickName).Child(Define.UseNickName).Child(nickName).SetRawJsonValueAsync(nickJson);
        FireBaseManager.DB.GetReference(Define.User).Child(userId).Child(Define.Character).Child(nickName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "Error saving user");
            }
            else
            {
                Utils.ShowInfo("캐릭터를 성공적으로 생성하였습니다.");
            }
        });
    }

    void RemoveCharacterData()
    {
        // 삭제할 경로 지정
        DatabaseReference characterRef = 
            FireBaseManager.DB.GetReference(Define.User).Child(userId).Child(Define.Character).Child(nickName);
        DatabaseReference nameRef = 
            FireBaseManager.DB.GetReference(Define.SearchNickName).Child(Define.UseNickName).Child(nickName);

        characterRef?.RemoveValueAsync();
        nameRef?.RemoveValueAsync();
    }

    async Task<bool> CheckIfUsernameExists(string username)
    {
        DatabaseReference userRef =
            FireBaseManager.DB.GetReference(Define.SearchNickName).Child(Define.UseNickName);
        Query query = userRef.OrderByChild("nickName").EqualTo(username);

        try
        {
            DataSnapshot snapshot = await query.GetValueAsync();
            return snapshot.Exists && snapshot.ChildrenCount > 0;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error checking username: " + ex.Message);
            return false;
        }
    }

}
