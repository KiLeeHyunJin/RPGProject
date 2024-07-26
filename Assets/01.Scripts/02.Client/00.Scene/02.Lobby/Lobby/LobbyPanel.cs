
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] CreateRoomPanel createRoomPanel; //
    [SerializeField] Button createButton;
    [SerializeField] Button leaveButton;

    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;
    Dictionary<string, RoomEntry> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomEntry>();
        createButton.onClick.AddListener(CreateRoomMenu); //�� ���� ��ư�� �ش� �̺�Ʈ ����
        leaveButton.onClick.AddListener((LeaveLobby)); //�κ� ������ ��ư�� �κ� ������ �̺�Ʈ ����
    }
    private void OnDisable()
    {
        for (int i = 0; i < roomContent.childCount; i++)
            Destroy(roomContent.GetChild(i).gameObject);
        roomDictionary.Clear();
    }
    void LeaveLobby()
    {
        //PhotonNetwork.LeaveLobby();
    }
    void CreateRoomMenu()
    {
        createRoomPanel.gameObject.SetActive(true);
    }
    //public void UpdateRoomList(List<RoomInfo> roomList)
    //{
    //    foreach (RoomInfo room in roomList)
    //    {
    //        if (roomDictionary.ContainsKey(room.Name))
    //        {
    //           if (room.RemovedFromList || room.IsOpen == false || room.IsVisible == false)
    //           {
    //               RoomEntry roomEntry = roomDictionary[room.Name];
    //               if (roomEntry != null)
    //                   Destroy(roomEntry.gameObject);
    //               roomDictionary.Remove(room.Name);
    //           }
    //           else
    //               roomDictionary[room.Name].SetRoomInfo(room);
    //        }
    //        else
    //        {
    //            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
    //            entry.SetRoomInfo(room);
    //            roomDictionary.Add(room.Name, entry);
    //        }
    //    }
    //}


}
