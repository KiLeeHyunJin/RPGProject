using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetPanel : PopUpUI
{
    [SerializeField] TMP_InputField emailInputField; //�̸��� �Է¶�

    [SerializeField] Button sendButton; //�̸��� ���� ��ư
    [SerializeField] Button cancelButton; //������ ��ư

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
                Utils.ShowInfo("��й�ȣ �ʱ�ȭ ���� ������ ��ҵǾ����ϴ�.");
                SetInteractable(true);
            }
            else if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "���� ���� ���ۿ� �����Ͽ����ϴ�.");
                SetInteractable(true);
            }
            else
            {
                Manager.UI.ClosePopupUI(this);
                Utils.ShowInfo("���� ������ ���۵Ǿ����ϴ�.");
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
