using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    UserData userData;
    public bool dbLoad;


    public UserData UserData { get { return userData; } }




    public void CreateUserData()
    {
        userData = new UserData();
        string json = JsonUtility.ToJson(userData);

        FireBaseManager.DB
            .GetReference("UserData").Child(FireBaseManager.Auth.CurrentUser.UserId)
            .SetRawJsonValueAsync(json);
    }
    public void GetUserData()
    {
        dbLoad = true;
        FireBaseManager.DB
               .GetReference("UserData")
               .Child(FireBaseManager.Auth.CurrentUser.UserId)
               .GetValueAsync().ContinueWithOnMainThread(task =>
               {
                   if (task.IsCanceled)
                   {
                       Debug.Log("cancle");
                       return;
                   }
                   else if (task.IsFaulted)
                   {
                       Debug.Log("fault");
                       return;
                   }
                   DataSnapshot snapshot = task.Result;
                   if (snapshot.Exists)
                   {
                       string json = snapshot.GetRawJsonValue();
                       userData = JsonUtility.FromJson<UserData>(json);
                       dbLoad = false;
                   }
               });
    }
}
