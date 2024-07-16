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

        verifyCo = StartCoroutine(VerifyCheckRoutine()); //���� Ȯ�� �ڷ�ƾ
        SendVerifyMail(); //�̸��� ���� �Լ�
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
                Utils.ShowInfo("���� ���� ��ҵǾ����ϴ�.");
                SetInteractable(true);
                return;
            }
            else if(task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions,"���� ���� �����Ͽ����ϴ�."); 
                SetInteractable(true);
                return;
            }
            Utils.ShowInfo("���� ���� �����Ͽ����ϴ�.");
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
                    Utils.ShowError(task.Exception.InnerExceptions, "������ �����Ͽ����ϴ�.");
                }
                else if(FireBaseManager.Auth.CurrentUser.IsEmailVerified)
                {
                    Utils.ShowInfo("�����Ǿ����ϴ�.");
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
