using Photon.Pun;       // 유니티용 포톤 컴포넌트들
using Photon.Realtime;  // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    private string gameVersion = "1"; // 게임 버전

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼


    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start() {
        PhotonNetwork.GameVersion = this.gameVersion;   // 게임 버전 확인
        PhotonNetwork.ConnectUsingSettings();           // 설정한 정보를 토대로 마스터 서버 접속 시도

        this.joinButton.interactable = false;
        this.connectionInfoText.text = "Connecting Master Server...";
    }

    // 마스터 서버 접속 성공시 자동 실행; ConnectUsingSettings() -> True
    public override void OnConnectedToMaster() {
        this.joinButton.interactable = true;
        this.connectionInfoText.text = "Master Server Connected.";

    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        this.joinButton.interactable = false;
        this.connectionInfoText.text = "Connect Failed to Master Server.";

        PhotonNetwork.ConnectUsingSettings();   // Retry
    }

    // 룸 접속 시도
    public void Connect() {
        this.joinButton.interactable = false;

        if (PhotonNetwork.IsConnected) {
            this.connectionInfoText.text = "Connecting Random Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            this.connectionInfoText.text = "Connect Failed to Master Server.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message) {
        this.connectionInfoText.text = "Creating New Room...";
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});   // Callback -> OnJoinedRoom()
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        this.connectionInfoText.text = "Room Joined.";
        PhotonNetwork.LoadLevel("Main");
    }
}