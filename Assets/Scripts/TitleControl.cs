using UnityEngine;
using System.Collections;
using System.Net;

// 타이틀 화면 제어.
public class TitleControl : MonoBehaviour
{              // 타이틀 화면.

    // ================================================================ //

    public enum STEP
    {

        NONE = -1,

        WAIT = 0,           // 입력 대기.
        SERVER_START,       // 대기 시작.
        SERVER_CONNECT,     // 게임 서버에 접속.
        CLIENT_CONNECT,     // 클라이언트 간 접속.
        PREPARE,            // 각종 준비.
        GAME_START,         // 게임 시작.

        ERROR,              // 오류 발생.
        WAIT_RESTART,       // 오류에서 복귀 대기.

        NUM,
    };

    public STEP step = STEP.NONE;
    public STEP next_step = STEP.NONE;

    private float step_timer = 0.0f;

    private const int usePost = 50765;

    // 통신 모듈의 컴포넌트.
    private Network network_ = null;

    // 호스트 플래그.
    private bool isHost = false;

    // 호스트 IP 주소.
    private string hostAddress = "127.0.0.1";

    // 게임 시작 동기화 정보 취득 상태 .
    private bool isReceiveSyncGameData = false;

    // ================================================================ //
    // MonoBehaviour에서 상속.

    void Start()
    {
        this.step = STEP.NONE;
        this.next_step = STEP.WAIT;

        // 호스트 이름을 취득합니다.

        GameObject obj = new GameObject("Network");
        if (obj != null)
        {
            network_ = obj.AddComponent<Network>();
            network_.RegisterReceiveNotification(PacketId.GameSyncInfo, OnReceiveSyncGamePacket);
        }
    }

    public void onClickStartAsServer()
    {
        isHost = true;
        network_.StartServer(usePost, Network.ConnectionType.TCP);
    }

    public void onClickStartAsClient()
    {
       isHost = false;
       network_.Connect(hostAddress, usePost, Network.ConnectionType.TCP);
    }

    // ---------------------------------------------------------------- //
    // 통신 처리 함수.

    // 
    public void OnReceiveSyncGamePacket(PacketId id, byte[] data)
    {
        isReceiveSyncGameData = true;
    }

    // 오류 통지.
 
}
