
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
public class UdonTalk : UdonSharpBehaviour
{
    public int Facial;
    public int Pose;
    [Tooltip("トークが表示され切ってから消えるまでの時間")]
    public float TalkEndTime = 5f;
    [TextArea(5, 12)]
    public string TalkValue;

    [Header("Selection")]
    [Tooltip("有効の時、EndTime及びNextTalk関連無効\n選択肢を発生させたい時に有効\n選択肢のトークでオンではない")]
    public bool Selection;
    [Tooltip("Selectionが有効の時、これで指定した選択肢を表示する")]
    public GameObject SelectionButton;
    [Header("Option")]
    [Tooltip("有効の時、話しはじめにSoundClipを再生する")]
    public bool Sound;
    public AudioClip SoundClip;
    [Tooltip("有効の時、話し終わった時に表情とポーズをリセットする")]
    public bool TalkEnd;

    [Header("NextTalks")]
    [Tooltip("有効の時、次のトークの予約をする")]
    public bool Reserve;
    // public GameObject ReserveTalkObj; 
    // [HideInInspector]
    public UdonTalk ReserveTalk;
    [Tooltip("有効の時、次のタイミングを待たずにトークを行う")]
    public bool Immediate;
    [Tooltip("Immediateが有効の時、この秒数後にトークを行う\nまたこれより先に次のタイミングが来るとキャンセルされる")]
    public float NextTalkTimer;
    [Tooltip("これとReserveが有効の時、\n指定した相方にReserveTalkを実行させる")]
    public bool Other;
    [Header("UdonSendEvent")]
    [Tooltip("有効の時、トーク開始時に指定したUdonのEventを呼ぶ")]
    public bool UseUdonSendEvent;
    [Tooltip("UseUdonSendEventが有効の時、\n対象のUdonのCustomEventを呼び出す")]
    public UdonBehaviour SendEventTarget;
    [Tooltip("呼び出すCustomEvent名")]
    public string CustomEventName;
    [HideInInspector]
    public bool RandomAlready;
    
    void Start()
    {
        // if(ReserveTalk)
        // ReserveTalk = ReserveTalkObj.GetComponent<UdonTalk>();
    }
}
