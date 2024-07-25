using Firebase.Database;
using Firebase.Extensions;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MenuPanel : MonoBehaviour
{
    [SerializeField] Image PlayButton;
    [SerializeField] GameObject PlayButtons;
    [SerializeField] GameObject userInfoWindow;

    [SerializeField] Button userInfoButton;
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    [SerializeField] Button logoutButton;
    [SerializeField] Button lobbyButton;
    [SerializeField] Button randomButton;

    [SerializeField] TMP_Text noticeText;


    [SerializeField] bool startButtonBool = false;

    private void Awake()
    {

        userInfoButton.onClick.AddListener(() => { userInfoWindow.SetActive(true); });
        openButton.onClick.AddListener(() => { OpenPlayButtons(!startButtonBool); });
        closeButton.onClick.AddListener(() => { OpenPlayButtons(false); });
        logoutButton.onClick.AddListener(Logout);
        lobbyButton.onClick.AddListener(JoinLobby);
        randomButton.onClick.AddListener(RandomMatching);
    }

    [Serializable]
    public class NickNames
    {
        public string[] nickNames;
    }
    private void OnEnable()
    {

        FireBaseManager.DB
             .GetReference("Notice")
             .GetValueAsync().ContinueWithOnMainThread(task =>
             {
                 if (task.IsCanceled)
                 {
                     Debug.Log("cancle");
                     return;
                 }
                 else if (task.IsFaulted)
                 {
                     Debug.Log("fault");
                     return;
                 }
                 DataSnapshot snapshot = task.Result;
                 if (snapshot.Exists)
                 {
                     string value = (string)snapshot.Value;
                     noticeText.SetText(value);

                 }
                 else
                 {
                     noticeText.text = "NONE";
                 }

             });
        OpenPlayButtons(false);
        startButtonBool = false;
    }

    public void RandomMatching()
    {
        JoinLobby();
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    void Logout()
    {
        FireBaseManager.Auth.SignOut();
        PhotonNetwork.Disconnect();
    }

    public void OpenPlayButtons(bool state)
    {

        PlayButton.color = state ? Color.gray : Color.white;
        PlayButtons.SetActive(state);
        startButtonBool = !startButtonBool;
    }

}
