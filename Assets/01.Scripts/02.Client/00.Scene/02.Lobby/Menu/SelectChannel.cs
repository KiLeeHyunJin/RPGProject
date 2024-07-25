using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class SelectChannel : MonoBehaviour
{
    [SerializeField] int serverNum;
    List<UserData.Character> characterDic;


    private void Start()
    {
        CallCharacterData();
    }

    public void EnterServer()
    {
        ServerDataTableManager.Instance.EnterServer(serverNum);
    }


    void CallCharacterData()
    {
        FireBaseManager.DB.RootReference.Child("users").Child(FireBaseManager.Auth.CurrentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "Error loading user");
            }
            else if (task.IsCompleted)
            {
                Firebase.Database.DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    UserData.User user = JsonUtility.FromJson<UserData.User>(json);
                    //characterDic = user.characters;
                    DebugCharacterName();
                }
                else
                {
                    Utils.ShowInfo("User does not exist.");
                }
            }
        });
    }

    void DebugCharacterName()
    {
        foreach (var item in characterDic)
        {
            Message.Log(item.nickName);
        }
    }
}
