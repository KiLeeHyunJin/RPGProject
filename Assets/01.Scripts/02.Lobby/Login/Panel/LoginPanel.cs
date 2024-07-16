using Firebase.Auth;
using Firebase;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField; 

    [SerializeField] Button signUpButton; //ȸ������ ��ư
    [SerializeField] Button loginButton; //�α��� ��ư
    [SerializeField] Button resetPasswordButton; //��й�ȣ �缳�� ��ư


    protected void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        loginButton.onClick.AddListener(Login);
        resetPasswordButton.onClick.AddListener(ResetPassword);
    }

    private void OnEnable()
    {
        SetInteractable(true);
    }

    public void SignUp()
    {
        Manager.UI.ShowPopUpUI($"SignUp");
    }

    private void ResetPassword()
    {
        Manager.UI.ShowPopUpUI($"Reset");
    }

    public void Login()
    {
        string id = emailInputField.text;
        string pw = passInputField.text;
        passInputField.text = ""; //��й�ȣ �Է¶� �ʱ�ȭ

        SetInteractable(false);

        FireBaseManager.Auth.SignInWithEmailAndPasswordAsync(id, pw).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Utils.ShowInfo("���� �õ��� ��ҵǾ����ϴ�.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "���ӿ� �����Ͽ����ϴ�.");
                SetInteractable(true);
                return;
            }
            SetInteractable(true);

            if(FireBaseManager.Auth.CurrentUser.IsEmailVerified)
            {
                if(FireBaseManager.Auth.CurrentUser.DisplayName.IsNullOrEmpty() == false)
                {
                    Photon.Pun.PhotonNetwork.ConnectUsingSettings();
                }
                else
                {
                    Manager.UI.ClearPopUpUI();
                }
            }
            else
            {
                Manager.UI.ShowPopUpUI($"Verify");
            }
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat($"User signed, in successfullt ; {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }

    private void SetInteractable(bool interactable)
    {
        emailInputField.interactable = interactable;
        passInputField.interactable = interactable;
        signUpButton.interactable = interactable;
        loginButton.interactable = interactable;
        resetPasswordButton.interactable = interactable;
    }
}
