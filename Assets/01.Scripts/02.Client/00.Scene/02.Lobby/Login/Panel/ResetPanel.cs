using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPanel : PopUpUI
{
    [SerializeField] TMP_InputField emailInputField; //이메일 입력란

    [SerializeField] Button sendButton; //이메일 전송 버튼
    [SerializeField] Button cancelButton; //나가기 버튼

    protected override void Awake()
    {
        base.Awake();
        sendButton.onClick.AddListener(SendResetMail);
        cancelButton.onClick.AddListener(Cancel);
    }
    protected override void Start()
    {
        base.Start();
        emailInputField.contentType = TMP_InputField.ContentType.EmailAddress;
        SetInteractable(true);
    }
    private void SendResetMail()
    {
        SetInteractable(false);
        string email = emailInputField.text;
        FireBaseManager.Auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Utils.ShowInfo("비밀번호 초기화 메일 전송이 취소되었습니다.");
                SetInteractable(true);
            }
            else if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "인증 메일 전송에 실패하였습니다.");
                SetInteractable(true);
            }
            else
            {
                Manager.UI.ClosePopupUI(this);
                Utils.ShowInfo("인증 메일이 전송되었습니다.");
                SetInteractable(true);
            }
        });
    }

    private void Cancel()
    {
        Manager.UI.ClosePopupUI(this);
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        sendButton.interactable = interactable;
        cancelButton.interactable = interactable;
    }

}
