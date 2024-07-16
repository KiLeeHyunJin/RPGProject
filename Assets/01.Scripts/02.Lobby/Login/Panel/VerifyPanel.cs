using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VerifyPanel : PopUpUI
{
    [SerializeField] Button logoutButton;
    [SerializeField] Button sendButton;
    Coroutine verifyCo;

    //[SerializeField] TextMeshProUGUI sendButtonText;

    //[SerializeField] int sendMailCooltime;
    protected override void Awake()
    {
        base.Awake();
        logoutButton.onClick.AddListener(Logout);
        sendButton.onClick.AddListener(SendVerifyMail);
    }
    protected override void Start()
    {
        base.Start();
        if (FireBaseManager.Auth == null)
            return;

        verifyCo = StartCoroutine(VerifyCheckRoutine()); //인증 확인 코루틴
        SendVerifyMail(); //이메일 전송 함수
        SetInteractable(true);
    }
    private void Logout()
    {
        FireBaseManager.Auth.SignOut();
        Manager.UI.ClosePopupUI(this);
    }

    private void SendVerifyMail()
    {
        SetInteractable(false);
        FireBaseManager.Auth.CurrentUser.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                Utils.ShowInfo("메일 전송 취소되었습니다.");
                SetInteractable(true);
                return;
            }
            else if(task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions,"메일 전송 실패하였습니다."); 
                SetInteractable(true);
                return;
            }
            Utils.ShowInfo("메일 전송 성공하였습니다.");
            SetInteractable(true);
        });
    }

    private void OnDestroy()
    {
        if(verifyCo != null)
            StopCoroutine(verifyCo);
    }

    IEnumerator VerifyCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            FireBaseManager.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Utils.ShowInfo("ReloadAsync canceled");
                }
                else if (task.IsFaulted)
                {
                    Utils.ShowError(task.Exception.InnerExceptions, "인증에 실패하였습니다.");
                }
                else if(FireBaseManager.Auth.CurrentUser.IsEmailVerified)
                {
                    Utils.ShowInfo("인증되었습니다.");
                    Manager.UI.ClosePopupUI(this);
                }
            });
        }
    }

    private void SetInteractable(bool interactable)
    {
        logoutButton.interactable = interactable;
        sendButton.interactable = interactable;
    }
}
