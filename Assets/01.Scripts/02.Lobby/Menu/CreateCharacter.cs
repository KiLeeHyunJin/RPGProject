using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateCharacter : MonoBehaviour
{
    public void CheckNicknameExists(string nickname, UnityAction<bool> callback)
    {
        FireBaseManager.DB.RootReference.Child("nicknames").OrderByValue().EqualTo(nickname).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions,"Error checking nickname");
                callback(false);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.ChildrenCount > 0)
                {
                    callback(true); //닉네임이 존재
                }
                else
                {
                    callback(false);
                }
            }
        });
    }


}
