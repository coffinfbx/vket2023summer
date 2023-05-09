using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class OnInteractRandomCustomEvent : UdonSharpBehaviour
    {
        [Header("同期するか")]
        public bool isSync   = false;
        [Header("ランダム実行するイベントを抱えているUDON")]
        public UdonBehaviour targetUdon;

        public string[]      randomEventNames   = new string[0];
        public float[]       randomRates        = new float[0];

        float max = 0;
        void Start(){            
            for (int i = 0; i < randomRates.Length; i++)
            {
                max += randomRates[i];
            }
        }

        public override void Interact()
        {
            float rate  = Random.Range(0,max);
            float check = 0;
            for (int i = 0; i < randomRates.Length; i++)
            {                   
                check += randomRates[i];
                if(check >= rate){
                    if(isSync) targetUdon.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,randomEventNames[i]);
                    else       targetUdon.SendCustomEvent(randomEventNames[i]);
                    return;
                }
            }
            if(isSync) targetUdon.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,randomEventNames[randomRates.Length-1]);
            else       targetUdon.SendCustomEvent(randomEventNames[randomRates.Length-1]);
        }
    }
}