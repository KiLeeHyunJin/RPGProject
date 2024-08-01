using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{

    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passInputField;

    [SerializeField] Button signUpButton; //회원가입 버튼
    [SerializeField] Button loginButton; //로그인 버튼
    [SerializeField] Button resetPasswordButton; //비밀번호 재설정 버튼

    [SerializeField] Toggle saveAccountToggle;
    private void Awake()
    {
        signUpButton.onClick.AddListener(SignUp);
        loginButton.onClick.AddListener(Login);
        resetPasswordButton.onClick.AddListener(ResetPassword);
    }
    private void Start()
    {
        LoadAccount();
        SetInteractable(true);
    }

    private void LoadAccount()
    {
        if (saveAccountToggle == null)
            return;

        if (PlayerPrefs.HasKey(DataDefine.SaveAccount) && 
            PlayerPrefs.GetInt(DataDefine.SaveAccount, 0) > 0)
        {
            if (PlayerPrefs.HasKey(DataDefine.Account))
            {
                emailInputField.text = PlayerPrefs.GetString(DataDefine.Account, "");
                saveAccountToggle.isOn = true;
                return;
            }
        }
        saveAccountToggle.isOn = false;
    }

    private void SaveAccount()
    {
        if (saveAccountToggle == null)
            return;

        if(saveAccountToggle.isOn)
        {
            PlayerPrefs.SetInt(DataDefine.SaveAccount, 1);
            PlayerPrefs.SetString(DataDefine.Account, emailInputField.text);
            return;
        }
        PlayerPrefs.SetInt(DataDefine.SaveAccount, 0);
        PlayerPrefs.SetString(DataDefine.Account, "");
    }

    private void SignUp()
    {
        Manager.UI.ShowPopUpUI($"SignUp");
    }

    private void ResetPassword()
    {
        Manager.UI.ShowPopUpUI($"Reset");
    }

    private void Login()
    {
        SaveAccount();
        string id = emailInputField.text;
        string pw = passInputField.text;
        passInputField.text = ""; //비밀번호 입력란 초기화

        SetInteractable(false);

        FireBaseManager.Auth.SignInWithEmailAndPasswordAsync(id, pw).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Utils.ShowInfo("접속 시도가 취소되었습니다.");
                SetInteractable(true);
                return;
            }
            if (task.IsFaulted)
            {
                Utils.ShowError(task.Exception.InnerExceptions, "접속에 실패하였습니다.");
                SetInteractable(true);
                return;
            }
            SetInteractable(true);

            if (FireBaseManager.Auth.CurrentUser.IsEmailVerified)
            {
                if (FireBaseManager.Auth.CurrentUser.DisplayName.IsNullOrEmpty() == false)
                {
                    //Photon.Pun.PhotonNetwork.ConnectUsingSettings();
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
