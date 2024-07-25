using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ServerData;

public class CreateCharacter : MonoBehaviour
{
    UserServerData user;
    enum Buttons
    {

    }

    [SerializeField] Stat stat;
    [SerializeField] string userId;
    [SerializeField] string nickName;
    [SerializeField] int point;

    [SerializeField] Button create;
    [SerializeField] Button check;
    [SerializeField] Button remove;
    [SerializeField] Button addStr;
    private void Start()
    {
        InitUserId();
        ButtonAddListener(create, CreateCharacterData);
        ButtonAddListener(remove, RemoveCharacterData);
        ButtonAddListener(check, CheckCharacterNickName);
        user = new();
        //point = 4;
    }

    void ButtonAddListener(Button btn, UnityAction call)
    {
        if (btn != null)
            btn.onClick.AddListener(call);
    }



    void InitUserId()
    {
        if (FireBaseManager.Auth != null)
            userId = FireBaseManager.Auth.CurrentUser.UserId;
    }



    //닉네임 사용 가능 여부 확인 호출 
    async void CheckCharacterNickName()
    {
        bool usernameExists = await CheckIfUsernameExists(nickName);
        string msg = usernameExists ? "사용할 수 없는 닉네임입니다." : "사용가능한 닉네임입니다.";
        Utils.ShowInfo(msg);
        Manager.FireBase.LoadUser(userId, null);
    }


    //캐릭터 생성
    void CreateCharacterData()
    {
        if (point > 0)
        {
            Utils.ShowInfo("능력치 포인트를 전부 배분하지 않았습니다.");
            return;
        }

        CharacterServerData uploadCharacterData = //OnCreateCharacter();
        new() { nickName = this.nickName };
        Name saveName =
            new() { nickName = this.nickName, userId = this.userId };

        string nickJson = JsonUtility.ToJson(saveName);
        string json = JsonUtility.ToJson(uploadCharacterData);

        FireBaseManager.DB.GetReference(DataDefine.SearchNickName).Child(DataDefine.UseNickName).Child(nickName).SetRawJsonValueAsync(nickJson);
        FireBaseManager.DB.GetReference(DataDefine.User).Child(userId).Child(DataDefine.Character).Child(nickName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
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



    //캐릭터 삭제
    void RemoveCharacterData()
    {
        // 삭제할 경로 지정
        DatabaseReference characterRef =
            FireBaseManager.DB.GetReference(DataDefine.User).Child(userId).Child(DataDefine.Character).Child(nickName);
        DatabaseReference nameRef =
            FireBaseManager.DB.GetReference(DataDefine.SearchNickName).Child(DataDefine.UseNickName).Child(nickName);

        characterRef?.RemoveValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "Error saving user");
            }
            else
            {
                Utils.ShowInfo("캐릭터를 삭제하였습니다.");
            }
        });
        nameRef?.RemoveValueAsync();
    }


    //닉네임 사용 가능 여부 확인하는 메소드
    async Task<bool> CheckIfUsernameExists(string username)
    {
        DatabaseReference userRef =
            FireBaseManager.DB.GetReference(DataDefine.SearchNickName).Child(DataDefine.UseNickName);
        Query query = userRef.OrderByChild(DataDefine.NickName).EqualTo(username);

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


    public CharacterServerData OnCreateCharacter()
    {
        return null;

    }
}
