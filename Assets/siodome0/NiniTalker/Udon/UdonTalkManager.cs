using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using NukoTween;
using TMPro;

public class UdonTalkManager : UdonSharpBehaviour
{
    public UdonSharpBehaviour Self;
    [Tooltip("掛け合いする場合相方を指定する")]
    public UdonTalkManager Other;
    [Space(10)]
    public Animator _animator;
    public GameObject _nukoTween;
    NukoTweenEngine tween;
    public GameObject Balloon;
    public GameObject NiniTalkText;
    [HideInInspector]
    public Text TalkText;

    [Space(10)]
    [Tooltip("ランダム及び順繰りトーク")]
    public UdonTalk[] Talks;
    // [Tooltip("予約トークなどで使う\nランダムおよび順繰りトークで選ばれない")]
    // public UdonTalk[] ReserveTalks;
    [Space(10)]
    [Tooltip("オンの時自動で話す\n掛け合いさせたい場合相方はオフに")]
    public bool AutoTalk = true;
    [Tooltip("オンの時Talksに登録したトークを順番に話す\nオフの時ランダム")]
    public bool Order;
    [Tooltip("トーク頻度（表示が消えてから）")]
    public float TalkInterval = 180f;
    [Tooltip("話す速さ")]
    public float TalkWeight = 0.025f;
    [HideInInspector]
    public int TalkNumber;
    [HideInInspector]
    public UdonTalk TempTalk;
    [HideInInspector]//次トークの予約をしてるか否か
    public bool Reserve;
    [Header("Advanced")]
    [Tooltip("有効の時、TextMeshProを利用します（ある程度分かってる人向け）")]
    public bool UseTextMeshPro;
    public GameObject TalkTextMeshPro;
    [HideInInspector]
    public TextMeshProUGUI TalkTextPro;
    public AudioSource _audio;

    void Start()
    {
        Reserve = false;
        TalkNumber = Talks.Length;
        Balloon.transform.localScale = new Vector3(0,0,0);

        if(UseTextMeshPro)
        {
            TalkTextMeshPro.gameObject.SetActive(true);
            NiniTalkText.gameObject.SetActive(false);
        }
        else
        {
            TalkTextMeshPro.gameObject.SetActive(false);
            NiniTalkText.gameObject.SetActive(true);
        }

        if(Talks.Length>0)
        {
            RandomAlreadyReset();
        }

        TalkText = NiniTalkText.GetComponent<Text>();
        TalkTextPro = TalkTextMeshPro.GetComponent<TextMeshProUGUI>();
        tween = _nukoTween.GetComponent<NukoTweenEngine>();

        NextTalk();
    }

    public void ForceTalk()
    {
        // tween.KillAll();
        Reserve = true;
        // TempTalk = ReserveTalks[TalkNumber];
        TalkStart();
    }

    public void TalkStart()
    {
        TalkText.text="";
        TalkTextPro.text = "";

        if(!Reserve)
        {
            if(Order)
            {
                TalkNumber++;
                if(TalkNumber>=Talks.Length)
                {
                    TalkNumber = 0;
                }
            }
            else
            {
                // TalkNumber = Random.Range(0, Talks.Length);
                //既読かどうか確認し、1つでも未読があれば既読リセット回避
                //全既読ならリセット
                //またランダムで引いたやつが既読なら再抽選
                bool AlreadyResetFlug = true;
                for(int i = 0;i<Talks.Length;i++)
                {
                    if(!Talks[i].RandomAlready)
                    {
                        AlreadyResetFlug = false;
                        break;
                    }
                }

                if(AlreadyResetFlug)
                {
                    RandomAlreadyReset();
                }

                for(int i = Random.Range(0, Talks.Length);true;i = Random.Range(0, Talks.Length))
                {
                    if(!Talks[i].RandomAlready)
                    {
                        TalkNumber = i;
                        Talks[i].RandomAlready = true;
                        break;
                    }
                }
            }
            TempTalk = Talks[TalkNumber];
        }

        if(TempTalk.UseUdonSendEvent)
        {
            TempTalk.SendEventTarget.SendCustomEvent(TempTalk.CustomEventName);
        }

        if(TempTalk.Sound)
        {
            _audio.PlayOneShot(TempTalk.SoundClip);
        }

        _animator.SetInteger("Facial",TempTalk.Facial);
        _animator.SetInteger("Pose",TempTalk.Pose);

        Balloon.transform.localScale = new Vector3(0,0,0);
        tween.LocalScaleTo(Balloon, new Vector3(1f,1f,1f), 0.5f, 0f, tween.EaseOutBack, false);
        if(UseTextMeshPro)
        {
            tween.TMPTextTo(TalkTextPro, TempTalk.TalkValue, TempTalk.TalkValue.Length*TalkWeight, 1f, tween.EaseLinear);
        }
        else
        {
            tween.TextTo(TalkText, TempTalk.TalkValue, TempTalk.TalkValue.Length*TalkWeight, 1f, tween.EaseLinear);
        }

        if(TempTalk.Selection)
        {
            tween.DelayedSetActive(TempTalk.SelectionButton,true, (TempTalk.TalkValue.Length*TalkWeight)+1f);
        }
        else
        {
            // tween.DelayedCallを使用するとinstanceの取得に失敗するのでSendCustomEventDelayedSecondsを使うように
            SendCustomEventDelayedSeconds(nameof(TalkEnd), (TempTalk.TalkValue.Length * TalkWeight) + TempTalk.TalkEndTime);
        }
    }

    public void TalkEnd()
    {
        if(TempTalk.TalkEnd)
        {
            _animator.SetInteger("Facial",0);
            _animator.SetInteger("Pose",0);
        }

        tween.LocalScaleTo(Balloon, new Vector3(0f,0f,0f), 0.5f, 0f, tween.EaseInBack, false);
        if(TempTalk.Reserve)
        {
            if(TempTalk.Other)
            {
                Other.TempTalk = TempTalk.ReserveTalk;
                Other.Reserve = true;
                Other.TalkStart();
                return;
            }
            else
            {
                if(TempTalk.Immediate)
                {
                    SendCustomEventDelayedSeconds(nameof(TalkStart), TempTalk.NextTalkTimer);
                }
                TempTalk = TempTalk.ReserveTalk;
                Reserve = true;
            }
        }
        else
        {
            Reserve = false;
            if (Other != null)
            {
                Other.Reserve = false;
                Other.NextTalk();
            }
        }
        NextTalk();
    }

    public void NextTalk()
    {
        if(AutoTalk)
        {
            SendCustomEventDelayedSeconds(nameof(TalkStart), TalkInterval);
        }
    }

    void RandomAlreadyReset()
    {
        if(Talks.Length>0)
        {
            for(int i = 0;i<Talks.Length;i++)
            {
                Talks[i].RandomAlready = false;
            }
        }
        
    }

    public void AutoTalkChange()
    {
        if(AutoTalk)
        {
            AutoTalk = false;
        }
        else
        {
            AutoTalk = true;
        }
    }

    public void AutoTalkON()
    {
        AutoTalk = true;
    }

    public void AutoTalkOFF()
    {
        AutoTalk = false;
    }
}
