
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class OnEnableParticlePlayer : UdonSharpBehaviour
    {
        [Header("コントロールするパーティクル")]
        public ParticleSystem[] particles;

        [Header("(Option)プレイするオーディオ")]
        public AudioSource    audioSource;
        public AudioClip      audioClip;

        bool isPlay = false;

        void OnEnable(){
            if(isPlay){                
                return;
            }
            isPlay = true;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }            
            if(audioSource != null)
                audioSource.PlayOneShot(audioClip);
        }

        void OnDisable(){
            if(!isPlay){                
                return;
            }
            isPlay = false;
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }            
        }
    }
}