
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ReserveTalkSender : UdonSharpBehaviour
{
    public UdonTalkManager _talkManager;
    // public int TalkNumber;
    public UdonTalk ReserveTalk;

    [Tooltip("選択肢に使う時にオン、ボタンを押すと選択肢を非表示にする")]
    public bool IsSelectionButton;

    public void ForceTalk()
    {
        // _talkManager.TalkNumber = TalkNumber;
        _talkManager.TempTalk = ReserveTalk;
        _talkManager.ForceTalk();

        if(IsSelectionButton)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

}
