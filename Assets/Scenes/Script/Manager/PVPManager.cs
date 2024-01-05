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
        SceneManager.activeSceneChanged += (Scene1, Scene2) => { if (photonView.IsMine) photonView.RPC("Setting", RpcTarget.All);  UIManager.instance.MainSceneUI(false); };
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

    [PunRPC]
    public void Setting()
    {
        for (int i = 0; i < User.instance.Deck.Count; i++)
        {
            string[] cloneStr = User.instance.Deck[i].gameObject.name.Split("(");
            GameObject clonePlayer = PhotonNetwork.Instantiate(cloneStr[0], transform.position, Quaternion.identity);
            clonePlayer.GetComponent<Character>().DeepCopy(User.instance.Deck[i]);


            playerDic.Add(clonePlayer.GetComponent<PhotonView>().ViewID, clonePlayer.GetComponent<Playerable>());
            battleList[i] = clonePlayer.GetComponent<PhotonView>().ViewID;
            //Transform[] playerPos = PhotonNetwork.IsMasterClient ? masterPlayerPos : otherPlayerPos;
            //clonePlayer.transform.position = playerPos[i].position;
            //if (PhotonNetwork.IsMasterClient)
            //    clonePlayer.transform.LookAt(otherPlayerPos[i]);
            //else
            //    clonePlayer.transform.LookAt(masterPlayerPos[i]);
            //dic.Add(clonePlayer.GetComponent<PhotonView>().ViewID, clonePlayer.GetComponent<Playerable>());
            //ownerList[i] = clonePlayer.GetComponent<PhotonView>().ViewID;
        }
    }
}