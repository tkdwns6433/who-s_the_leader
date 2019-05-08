using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.UI;

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

    public Text testText;

    void Start()
    {
        this.step = STEP.NONE;
        this.next_step = STEP.WAIT;

        // 호스트 이름을 취득합니다.

        GameObject obj = new GameObject("Network");
        obj.tag = "Network";
        if (obj != null)
        {
            network_ = obj.AddComponent<Network>();
            network_.RegisterReceiveNotification(PacketId.GameSyncInfo, OnReceiveSyncGamePacket);
            network_.RegisterReceiveNotification(PacketId.ProduceUnit, this.OnReceiveUnitProducePacket);
            network_.RegisterReceiveNotification(PacketId.UnitAttack, this.OnReceiveUnitAttackPacket);
            network_.RegisterReceiveNotification(PacketId.UnitMove, this.OnReceiveUnitMovePacket);
            network_.RegisterReceiveNotification(PacketId.RecruitNpc, this.OnReceiveRecuruitNpcPacket);
            network_.RegisterReceiveNotification(PacketId.TurnEnd, this.OnReceiveEndTurnPacket);

            // 이벤트 핸들러.
            network_.RegisterEventHandler(OnEventHandling);
        }
    }

    public void onClickStartAsServer()
    {
        isHost = true;
        network_.StartServer(usePost, Network.ConnectionType.TCP);
        UnityEngine.Random.InitState(UnityEngine.Random.Range(0, 10000));
    }

    public void onClickStartAsClient()
    {
       isHost = false;
       network_.Connect(hostAddress, usePost, Network.ConnectionType.TCP);
    }

    // ---------------------------------------------------------------- //
    // 통신 처리 함수.

    public void OnReceiveUnitProducePacket(PacketId id, byte[] data)
    {
        UnitProducePacket packet = new UnitProducePacket(data);
        UnitProduceData produceData = packet.GetPacket();
        int building_id = produceData.buildingId;
        int producedUnit = produceData.producedUnit;
        int xPos = produceData.x;
        int yPos = produceData.y;
        GameObject.FindGameObjectWithTag("UnitGenerator").GetComponent<UnitGenerator>().GenerateUnit(building_id, (UnitType)producedUnit, xPos, yPos);
    }

    public void OnReceiveUnitMovePacket(PacketId id, byte[] data)
    {
        UnitMovePacket packet = new UnitMovePacket(data);
        UnitMoveData moveData = packet.GetPacket();
        int unit_id = moveData.unitId;
        int xPos = moveData.x;
        int yPos = moveData.y;
        GameManager.GetInstance.getUnit(unit_id).ClientUnitMove(xPos, yPos);
    }

    public void OnReceiveUnitAttackPacket(PacketId id, byte[] data)
    {
        UnitAttackPacket packet = new UnitAttackPacket(data);
        UnitAttackData attack = packet.GetPacket();
        int attack_id = attack.unitId;
        int defener_id = attack.targetUnidId;
        UnitAttack unitAttack = new UnitAttack(attack_id);
        unitAttack.DoAttack(defener_id);
    }

    public void OnReceiveRecuruitNpcPacket(PacketId id, byte[] data)
    {

    }

    public void OnReceiveSelectLeaderPacket(PacketId id, byte[] data)
    {
        SelectLeaderPacket packet = new SelectLeaderPacket(data);
        SelectLeaderData selectLeader = packet.GetPacket();
        //선택 UI를 해당리더로 바꿔주는 코드
    }

    public void OnReceiveEndTurnPacket(PacketId id, byte[] data)
    {
        TurnEndPacket packet = new TurnEndPacket(data);
        TurnEndData turnend = packet.GetPacket();
        GameManager.GetInstance.myTurn = true;
        GameManager.GetInstance.startTurn(GameManager.GetInstance.myPlayer);
        Debug.Log("my turn end");
    }

    public void setSeed(int seed)
    {
        UnityEngine.Random.InitState(seed);
        GameObject.Find("TitleControl").GetComponent<TitleControl>().testText.text = UnityEngine.Random.seed.ToString();
        Debug.Log("Seed Number : " + UnityEngine.Random.seed);
    }

    //connect 되었을 떄 서버에서 이루어지는 함수
    private void SendGameSyncInfo()
    {
        //랜덤 시드 정하고 보내준다
        setSeed(40);
        SyncGameData data = new SyncGameData();
        data.randomSeed = UnityEngine.Random.seed;
        SyncGamePacket packet = new SyncGamePacket(data);
        network_.SendReliable(packet);
        //리더 설정창으로 넘어가는 코드

    }

    public void OnReceiveSyncGamePacket(PacketId id, byte[] data)
    {
        SyncGamePacket packet = new SyncGamePacket(data);
        SyncGameData syncGame = packet.GetPacket();
        setSeed(syncGame.randomSeed);
        //리더 설정창으로 넘어감
    }

    private void DisconnectClient()
    {
        Debug.Log("[SERVER]DisconnectClient");

        network_.Disconnect();
    }


    // ================================================================ //


    public void OnEventHandling(NetEventState state)
    {
        switch (state.type)
        {
            case NetEventType.Connect:
                Debug.Log("[SERVER]NetEventType.Connect");
                //유닛 선택화면으로 옮기고
                //ready 후에 sendgamesyncinfo()하든가!
                SendGameSyncInfo();
                break;

            case NetEventType.Disconnect:
                Debug.Log("[SERVER]NetEventType.Disconnect");
                DisconnectClient();
                break;
            default: 
                Debug.Log("net type not correct, but event handling is working");
                break;
        }
    }


}
