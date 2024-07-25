using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : PopUpUI
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField;
    [SerializeField] TMP_InputField confirmInputField;

    [SerializeField] Button cancelButton; //창 닫기 버튼
    [SerializeField] Button signUpButton; //아이디 생성 버튼

    protected override void  Awake()
    {
        base.Awake();
        cancelButton.onClick.AddListener(Cancel); 
        signUpButton.onClick.AddListener(SignUp);
    }
    protected override void Start()
    {
        base.Start();
        emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
        passInputField.contentType = TMP_InputField.ContentType.Password;
        passInputField.contentType = TMP_InputField.ContentType.Password;
        SetInteractable(true);
    }
    public void SignUp()
    {
        string email = emailInputField.text;
        string password = passInputField.text;
        string confirm = confirmInputField.text;

        if (confirm != password)
        {
            Utils.ShowInfo($"비밀번호가 불일치합니다.");

            return;
        }
        SetInteractable(false);
        FireBaseManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread((task) => {
            if (task.IsCanceled)
            {
                Utils.ShowInfo($"계정 생성이 취소되었습니다.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "사용할 수 없습니다.");
                SetInteractable(true);
                return;
            }
            Firebase.Auth.AuthResult result = task.Result;
            Manager.UI.ClosePopupUI(this);
            Utils.ShowInfo("계정이 생성되었습니다.");
            SetInteractable(true);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            //Manager.Game.CreateUserData();
        });
    }



    public void Cancel()
    {
        Manager.UI.ClosePopupUI(this);
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        passInputField.interactable = interactable;
        confirmInputField.interactable = interactable;
        cancelButton.interactable = interactable;
        signUpButton.interactable = interactable;
    }
}
