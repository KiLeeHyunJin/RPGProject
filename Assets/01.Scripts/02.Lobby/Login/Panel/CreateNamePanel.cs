using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class CreateNamePanel : MonoBehaviour
{
    [SerializeField] LoginPanelController panelController;
    [SerializeField] TMP_InputField nameInputField;

    [SerializeField] Button nameApplyButton;
    [SerializeField] Button backButton;

    protected void Awake()
    {
        nameApplyButton.onClick.AddListener(NameApply); //�г��� ���� ��ư�� �г��� ���� �õ� �Լ��� ����
        backButton.onClick.AddListener(Back); //������ ��ư�� ������ �Լ��� ����
    }

    private void NameApply()
    {
        SetInteractable(false);
        UserProfile profile = new UserProfile();
        string nickName = nameInputField.text;
        profile.DisplayName = nickName;
        profile.PhotoUrl = FireBaseManager.Auth.CurrentUser.PhotoUrl;


        FireBaseManager.DB
            .GetReference("NickNames")
            .Child("name")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled)
                {
                    return;
                }
                else if(task.IsFaulted)
                {
                    return;
                }
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                Debug.Log(json);
            });



        FireBaseManager.Auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => 
        {
            if(task.IsCanceled)
            {
                Utils.ShowInfo("�г��� ������ ��ҵǾ����ϴ�.");
                SetInteractable(true);
                return;
            }
            if(task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "�г��� ������ �����Ͽ����ϴ�.");
                SetInteractable(true);
                return;
            }
            FireBaseManager.DB
            .GetReference("NickNames")
            .Child("name")
            .SetRawJsonValueAsync(JsonUtility.ToJson(nickName));

            Utils.ShowInfo("�г��� ������ �����Ǿ����ϴ�.");
            SetInteractable(true);
        });
    }


    private void Back()
    {
        if(FireBaseManager.Auth.CurrentUser.DisplayName.IsNullOrEmpty())
            Utils.ShowInfo("�г����� �Է����ּ���.");
        else
        {
            gameObject.SetActive(false);
            Photon.Pun.PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Delete()
    {
        SetInteractable(false);
        FireBaseManager.Auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Utils.ShowInfo("���� ������ ��ҵǾ����ϴ�.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "���� ������ �����Ͽ����ϴ�..");
                SetInteractable(true);
                return;
            }
            Utils.ShowInfo("���� ������ �Ϸ�Ǿ����ϴ�.");
            SetInteractable(true);
            FireBaseManager.Auth.SignOut();
            //panelController.SetActivePanel(LoginPanelController.Panel.Login);
        });
    }


    private void SetInteractable(bool interactable)
    {
        nameInputField.interactable = interactable;
        nameApplyButton.interactable = interactable;
        backButton.interactable = interactable;
    }

}
