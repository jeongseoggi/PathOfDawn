using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PVPManager : Singleton<PVPManager>
{
    public GameObject btnObj;
    public TMP_InputField nameField;
    public GameObject matchingPanel;
    public Dictionary<int, Playerable> playerDic;
    public int[] battleList;


    public void Start()
    {
        //���� �ŵ���ȭ 
        playerDic = new Dictionary<int, Playerable>();
        battleList = new int[3];
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.activeSceneChanged += (Scene1, Scene2) => { UIManager.instance.MainSceneUI(false); };
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NickName = nameField.text;
            Debug.Log("���� �� ����");
            PhotonNetwork.JoinRandomRoom();
        }
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CancleMatching()
    {
        Debug.Log("��Ī ���");
        PhotonNetwork.LeaveRoom();
        matchingPanel.SetActive(false);
        btnObj.SetActive(true);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ����");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ����");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("��Ī�ϱ� ���� ���� ����");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        btnObj.SetActive(false);
        matchingPanel.SetActive(true);
        Debug.Log(PhotonNetwork.NickName + "����");
        matchingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount + "/ 2";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "�� �濡 ���Խ��ϴ�.");

        matchingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount + "/ 2";
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("PvpMap");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "�� ���� �������ϴ�.");
    }
}