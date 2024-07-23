using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using static UserData;

public class CreateCharacter : MonoBehaviour
{
    User user;
    enum Buttons
    {

    }

    [SerializeField] UserData.Stat stat;
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
        stat.SetStat(4);
        user = new();
        //point = 4;
    }

    void ButtonAddListener(Button btn, UnityAction call)
    {
        if (btn != null)
            btn.onClick.AddListener(call);
    }

    #region Stat
    void AddStat(Define.StatType statType)
    {
        if (point < 0)
            return;

        stat.SetValue(statType, 1);
        point--;
    }

    void MinusStat(Define.StatType statType)
    {
        if (stat.GetValue(statType) <= 0)
            return;

        stat.SetValue(statType, -1);
        point++;
    }
    #endregion Stat

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

        UserData.Character uploadCharacterData = OnCreateCharacter();
        //new() { nickName = this.nickName, stat = this.stat };
        UserData.Name saveName =
            new() { nickName = this.nickName, userId = this.userId };

        for (int i = 0; i < Define.SlotDefaultSize; i++)
        {
            uploadCharacterData.inventory.ect.Add(new UserData.Item());
            uploadCharacterData.inventory.consume.Add(new UserData.Item());
            uploadCharacterData.inventory.equip.Add(new UserData.Item());
        }

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

   

    //캐릭터 삭제
    void RemoveCharacterData()
    {
        // 삭제할 경로 지정
        DatabaseReference characterRef =
            FireBaseManager.DB.GetReference(Define.User).Child(userId).Child(Define.Character).Child(nickName);
        DatabaseReference nameRef =
            FireBaseManager.DB.GetReference(Define.SearchNickName).Child(Define.UseNickName).Child(nickName);

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
            FireBaseManager.DB.GetReference(Define.SearchNickName).Child(Define.UseNickName);
        Query query = userRef.OrderByChild(Define.NickName).EqualTo(username);

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


    public Character OnCreateCharacter()
    {
        return new()
        {
            nickName = nickName,
            level = default,
            job = default,
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
            stat = this.stat,
            inventory = new Inventory
            {
                consume = new(Define.SlotDefaultSize),
                ect = new(Define.SlotDefaultSize),
                equip = new(Define.SlotDefaultSize),
                money = default
            },
            skill = "Basic Attack"
        };

    }
}
