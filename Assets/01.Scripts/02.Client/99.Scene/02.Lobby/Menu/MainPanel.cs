using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] GameObject editPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text emailText;
    [SerializeField] TMP_Text idText;

    [SerializeField] Button deleteButton; //�α׾ƿ� ��ư
    [SerializeField] Button editButton; //����� ���� ���� ��ư
    [SerializeField] Button cancleButton; //â �ݱ� ��ư

    protected void Awake()
    {
        deleteButton.onClick.AddListener(Delete);
        editButton.onClick.AddListener(Edit);
        cancleButton.onClick.AddListener(Cancle);
    }
    private void OnEnable()
    {
        if (FireBaseManager.Auth.CurrentUser == null)
            return;
        editPanel.SetActive(false);
        mainPanel.SetActive(true);
        nameText.text = FireBaseManager.Auth.CurrentUser.DisplayName;
        emailText.text = FireBaseManager.Auth.CurrentUser.Email;
        idText.text = FireBaseManager.Auth.CurrentUser.UserId;
    }

    void Edit()
    {
        editPanel.SetActive(true);
    }
    void Cancle()
    {
        gameObject.SetActive(false);
    }

    void Delete()
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
            //PhotonNetwork.Disconnect();
        });
    }

    void SetInteractable(bool state)
    {
        deleteButton.interactable = state;
        editButton.interactable = state;
        cancleButton.interactable = state;
    }

}
