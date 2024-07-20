
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using static UserData;

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

    public void SaveUser(string userId, User user)
    {
        string json = JsonUtility.ToJson(user);
        DB.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
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

    public void SaveCharacter(string userId, string characterId,in Character character)
    {
        string json = JsonUtility.ToJson(character);
        DB.RootReference.Child("users").Child(userId).Child("characters").Child(characterId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
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

    public void LoadUser(string userId, System.Action<User> callback)
    {
        DB.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading user: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Firebase.Database.DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    User user = JsonUtility.FromJson<User>(json);
                    callback(user);
                }
                else
                {
                    Debug.Log("User does not exist.");
                    callback(null);
                }
            }
        });
    }

    public void LoadCharacter(string userId, string characterId, System.Action<Character> callback)
    {
        DB.RootReference.Child("users").Child(userId).Child("characters").Child(characterId).GetValueAsync().ContinueWithOnMainThread(task => {
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
                    Character character = JsonUtility.FromJson<Character>(json);
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
