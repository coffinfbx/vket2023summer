
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    // 手が触れると押下できるスイッチ
    public class OnTriggerHandCustomEvent : UdonSharpBehaviour
    {
        [Header("押せるのは一度だけ")]
        public bool           IsOnece = false;

        [Header("再度スイッチを押せる時間")]
        public float          reloadTime = 0.5f;

        [Header("(Option) スイッチの範囲")]
        public float          switchRange = 0.1f;

        [Header("(Option) スイッチの範囲をコライダで管理")]
        public SphereCollider switchRangeCollider;

        [Header("(Option) VRでもInteractを有効にする")]
        public bool           isInteractActive = false;

        [Header("(Option) このアイテムを持っている手は対象にしない")]
        public VRC_Pickup     pickup;

        [Header("(Option) 他のピックアップを持っている手は対象にしない")]
        public bool           isNotPickupedHand = false;

        [Header("実行するイベント")]
        public UdonBehaviour  udonBehaviour;
        public string         udonEventName = "";

        System.DateTime nextTime ;
        
        bool isPlayed = false;
        float _switchRange = 0;

        void Start(){
            this.DisableInteractive = !isInteractActive || !Networking.LocalPlayer.IsUserInVR();
            _switchRange = switchRange*switchRange;
        }


        //　持ち手判定無しか、反対の手が 右手か左手が範囲内にあれば起動
        void Update(){
            if(!isPlayed 
               && Networking.LocalPlayer.IsUserInVR()               
               && System.DateTime.Now >= nextTime               
               && (    ( (!isNotPickupedHand     || Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Left)   == null)   
                            &&  (pickup == null  || pickup.currentHand != VRC_Pickup.PickupHand.Left ) 
                            && _switchRange >= (transform.position - Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand ).position).sqrMagnitude)
                    || ( (!isNotPickupedHand     || Networking.LocalPlayer.GetPickupInHand(VRC_Pickup.PickupHand.Right)  == null) 
                            &&  (pickup == null  || pickup.currentHand != VRC_Pickup.PickupHand.Right) 
                            && _switchRange >= (transform.position - Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position).sqrMagnitude))
            ){
                isPlayed = IsOnece;
                nextTime = System.DateTime.Now.AddSeconds(reloadTime);
                udonBehaviour.SendCustomEvent(udonEventName);
            }
        }

        public override void Interact()
        {
            if(!isPlayed 
               && System.DateTime.Now >= nextTime
            ){
                isPlayed = IsOnece;
                nextTime = System.DateTime.Now.AddSeconds(reloadTime);
                udonBehaviour.SendCustomEvent(udonEventName);
            }
        }
    }
}