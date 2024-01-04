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
    public List<Playerable> battleList;


    public void Start()
    {
        //포톤 신동기화 
        battleList = new List<Playerable>();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.activeSceneChanged += (Scene1, Scene2) => { Setting(); UIManager.instance.MainSceneUI(false); };
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.NickName = nameField.text;
            Debug.Log("조인 룸 누름");
            PhotonNetwork.JoinRandomRoom();
        }
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CancleMatching()
    {
        Debug.Log("매칭 취소");
        PhotonNetwork.LeaveRoom();
        matchingPanel.SetActive(false);
        btnObj.SetActive(true);
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 닫힘");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("매칭하기 위해 방을 만듬");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        btnObj.SetActive(false);
        matchingPanel.SetActive(true);
        Debug.Log(PhotonNetwork.NickName + "입장");
        matchingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount + "/ 2";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "이 방에 들어왔습니다.");

        matchingPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.PlayerCount + "/ 2";
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("PvpMap");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "이 방을 나갔습니다.");
    }

    public void Setting()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("PlayerPoints", new Vector3(-6.63f, 1.17f, 0.1f), Quaternion.identity);
    }
    
    //public void PlayerInstantiate()
    //{
    //    foreach (Playerable player in User.instance.Deck)
    //    {
    //        string[] cloneStr = player.gameObject.name.Split('(');
    //        GameObject clonePlayer = PhotonNetwork.Instantiate(cloneStr[0], transform.position, Quaternion.identity);
    //        clonePlayer.GetComponent<Playerable>().DeepCopy(player);

    //        if (PhotonNetwork.IsMasterClient)
    //            masterPlayerList.Add(clonePlayer.GetComponent<Playerable>());
    //        else
    //            otherPlayerList.Add(clonePlayer.GetComponent<Playerable>());
    //        battleList.Add(clonePlayer.GetComponent<Playerable>());
    //    }
    //}
}