using Firebase;
using Firebase.Auth;
using System;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : PopUpUI
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] Button closeButton;

    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(Close);
    }

    public void ShowInfo(string message)
    {
        //gameObject.SetActive(true);
        infoText.text = message;
    }

    public void ShowError(ReadOnlyCollection<Exception> exceptions, string str)
    {
        foreach (System.Exception innerException in exceptions)
        {
            if (innerException is FirebaseException authException)
            {
                //에러코드와 같이 출력 
                AuthError errorCode = (AuthError)authException.ErrorCode;
                ShowInfo($"{str}\n ErrorCode : {errorCode}");
            }
        }
    }
}
