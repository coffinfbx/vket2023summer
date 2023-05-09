using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

namespace SimpleUDONGimmick
{
    public class AutoCustomEvent : UdonSharpBehaviour
    {
        [Header("イベントを実行するUDON")]
        public UdonBehaviour udon;
        [Header("カスタムイベント名")]
        public string        eventName = "Event";
        [Header("イベントを実行する時間")]
        public float eventTime = 1f;


        DateTime timer;

        void Start()
        {
            timer = DateTime.Now.AddSeconds(eventTime);
        }
        void Update(){
            if(timer < DateTime.Now){
                udon.SendCustomEvent(eventName);
                timer = DateTime.MaxValue;
                this.enabled = false;
            }
        }
    }
}