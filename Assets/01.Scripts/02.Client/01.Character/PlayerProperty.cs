using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperty : MonoBehaviour
{
    //Chat chat; //대화창
    [SerializeField] Player player; //우클릭한 대상

    [SerializeField] Button whisper; //귓속말 버튼
    [SerializeField] Button playerInfo;
    [SerializeField] Button windowClick;
    [SerializeField] Button partyInvite;

    [SerializeField] PopUpUI infoUI;
    [SerializeField] TMP_Text whisperText;
    private void Awake()
    {
        //playerInfo.onClick.AddListener(PlayerInfo);
        windowClick.onClick.AddListener(Close);
        whisper.onClick.AddListener(Whisper); //귓속말 버튼에 귓속말 함수 연결
    }

    //private void Start()
    //{
    //    chat ??= FindObjectOfType<Chat>();
    //}
    //public void SetChat(Chat _chat)
    //{
    //    chat = _chat; //대화창 연결
    //}
    public void SetPlayer(Player _player)
    {
        if (_player != null) //우클릭 객체가 비어있는지 확인
            player = _player; //있다면 대입
        else
            gameObject.SetActive(false); //없다면 오류기때문에 비활성화
    }
    public void isWhispering(bool b)
    {
        if (b)
            whisperText.text = "해제";
        else
            whisperText.text = "귓속말";
    }
    private void OnEnable()
    {

    }

    void Close()
    {
        if (infoUI != null)
        {
            infoUI.Close();
        }
        gameObject.SetActive(false);
    }
    void Whisper()
    {
        //채팅을 보낼 상대를 설정한다.
        //chat.SendTarget(player);
        //임무를 마쳤기에 비활성화

        gameObject.SetActive(false);
    }

    void GetOut()
    {
        //추방시킨다.
        PhotonNetwork.CloseConnection(player);
        //임무를 마쳤기에 비활성화
        gameObject.SetActive(false);
    }

}
