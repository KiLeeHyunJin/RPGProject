
using Firebase.Extensions;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using static ServerData;

public class FireBaseManager : Singleton<FireBaseManager>
{
    private static Firebase.FirebaseApp app;
    private static Firebase.Auth.FirebaseAuth auth;
    private static Firebase.Database.FirebaseDatabase db;
    public static Firebase.FirebaseApp App { get { return app; } }
    public static Firebase.Auth.FirebaseAuth Auth { get { return auth; } }
    public static Firebase.Database.FirebaseDatabase DB { get { return db; } }



    protected override void Awake()
    {
        base.Awake();
        CheckFirebaseAvailable();
    }
    void CheckFirebaseAvailable()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().
            ContinueWithOnMainThread(FirebaseAvailableCheck);
    }

    void FirebaseAvailableCheck(Task<Firebase.DependencyStatus> task)
    {
        var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            Debug.Log("Firebase Check Success");
            app = Firebase.FirebaseApp.DefaultInstance;
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            db = Firebase.Database.FirebaseDatabase.DefaultInstance;
        }
        else
        {
            UnityEngine.Debug.LogError(System.String.Format(
              "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            app = null;
            auth = null;
            db = null;
        }
    }

    public void SaveUser(string userId, UserServerData user)
    {
        string json = JsonUtility.ToJson(user);
        DB.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error saving user: " + task.Exception);
            }
            else
            {
                Debug.Log("User saved successfully.");
            }
        });
    }

    public void SaveCharacter(string userId, string characterId, in CharacterServerData character)
    {
        string json = JsonUtility.ToJson(character);
        DB.RootReference.Child("users").Child(userId).Child("characters").Child(characterId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error saving character: " + task.Exception);
            }
            else
            {
                Debug.Log("Character saved successfully.");
            }
        });
    }

    public void LoadUser(string userId, System.Action<UserServerData> callback)
    {
        DB.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading user: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Firebase.Database.DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string jsonData = snapshot.GetRawJsonValue();
                    UserServerData user = JsonConvert.DeserializeObject<UserServerData>(jsonData);
                    callback?.Invoke(user);
#if UNITY_EDITOR
                    foreach (var character in user.characters)
                        Debug.Log("Character NickName: " + character.Value.nickName);

                    System.IO.File.WriteAllText($"save.txt", jsonData);
#endif

                }
                else
                {
                    Debug.Log("User does not exist.");
                    callback?.Invoke(null);
                }
            }
        });
    }

    public void LoadCharacter(string userId, string characterId, System.Action<CharacterServerData> callback)
    {
        DB.RootReference.Child("users").Child(userId).Child("characters").Child(characterId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading character: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Firebase.Database.DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    CharacterServerData character = JsonUtility.FromJson<CharacterServerData>(json);
                    callback(character);
                }
                else
                {
                    Debug.Log("Character does not exist.");
                    callback(null);
                }
            }
        });
    }

}
