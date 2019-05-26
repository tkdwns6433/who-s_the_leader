using UnityEngine;
using System;
using System.Collections;
using System.Net;


public enum PacketId
{
	// 게임용 패킷.
	GameSyncInfo, //케릭터 선택과 같은 이벤트
	GameSyncInfoHouse,
	CharacterData,
	ItemData,
	Moving,
	GoingOut,
	ChatMessage,
	
	Max,
    //직접 구현하는 패킷
    ProduceUnit,
    UnitMove,
    UnitAttack,
    RecruitNpc,
    TurnEnd,
    SyncGame,
    SelectLeader
}


public struct PacketHeader
{
	// 패킷 종류.
	public PacketId 	packetId;
}


//
//
// 게임용 패킷 데이터 정의.
//
//


//
// 게임 전 동기화 정보.
//
public struct SyncGameData
{
	public int			randomSeed; 
}

public struct SelectLeaderData
{
    public int unitType;
}

//
// 아이템 획득 정보.
//
public struct ItemData
{
	public string 		itemId;		// 아이템 식별자.
	public int			state;		// 아이템 취득 상태.
	public string 		ownerId;	// 소유자 ID.

	public const int 	itemNameLength = 32;		// 아이템 이름 길이.
	public const int 	characterNameLength = 64;	// 캐릭터 ID 길이.
}

//
// 캐릭터 좌표 정보.
//
public struct CharacterCoord
{
	public float	x;		// 캐릭터의 x좌표.
	public float	z;		// 캐릭터의 z좌표.
	
	public CharacterCoord(float x, float z)
	{
		this.x = x;
		this.z = z;
	}
	public Vector3	ToVector3()
	{
		return(new Vector3(this.x, 0.0f, this.z));
	}
	public static CharacterCoord	FromVector3(Vector3 v)
	{
		return(new CharacterCoord(v.x, v.z));
	}
	
	public static CharacterCoord	Lerp(CharacterCoord c0, CharacterCoord c1, float rate)
	{
		CharacterCoord	c;
		
		c.x = Mathf.Lerp(c0.x, c1.x, rate);
		c.z = Mathf.Lerp(c0.z, c1.z, rate);
		
		return(c);
	}
}

//
// 캐릭터의 이동 정보.
//
public struct CharacterData
{
	public string			characterId;	// 캐릭터 ID.
	public int 				index;			// 위치 좌표 인덱스.
	public int				dataNum;		// 좌표 데이터 수.
	public CharacterCoord[]	coordinates;	// 좌표 데이터.
	
	public const int 	characterNameLength = 64;	// 캐릭터 ID 길이.
}

//
// 이사 정보.
//
public struct MovingData
{
	public string		characterId;	// 캐릭터 ID.
	public string		houseId;		// 집 ID.
	public bool 		moving;			// 이사 정보.

	public const int 	characterNameLength = 64;	// 캐릭터 ID의 길이.
	public const int 	houseIdLength = 32;			// 집 ID 길이.
}

//
// 정원 이동 정보.
//
public struct GoingOutData
{
	public string		characterId;	// 캐릭터ID.
	public bool 		goingOut;		// 놀러갈 정보.

	public const int 	characterNameLength = 64;	// 캐릭터 ID 길이.
	public const int 	houseIdLength = 32;			// 집ID 길이.
}

//
// 채팅 메시지.
//
public struct ChatMessage
{
	public string		characterId; // 캐릭터 ID.
	public string		message;	 // 채팅 메시지.

	public const int 	characterNameLength = 64;	// 캐릭터 ID 길이.
	public const int	messageLength = 64;
}

public struct TurnEndData
{
    public int state;
}

//
// 유닛 이동
//
public struct UnitMoveData
{
    public int unitId;
    public float x;
    public float y;
}
//
// 유닛 공격
//
public struct UnitAttackData
{
    public int unitId;
    public int targetUnidId;
}
//
//유닛 생산
//
public struct UnitProduceData
{
    public int buildingId;
    public int producedUnit; //enum형태의 타입!
    public int x;
    public int y;
}
//
//NPC 상호작용 : 대학생, 마피아, 시민, 갱스터 >> NpcId를 통해 Npc의 종류를 파악하고 그에 따른 영입효과를 적용한다.(자원의 차감, 유닛의 생성)
//
public struct recruitNpcData
{
    public int NpcId;
}
