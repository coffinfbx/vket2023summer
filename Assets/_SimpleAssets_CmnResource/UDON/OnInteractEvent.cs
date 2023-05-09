
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class OnInteractEvent : UdonSharpBehaviour
    {
        [Header("(Option) 実行は一度だけ")]
        public bool isOnce = true;

        [Header("(Option) 同期する")]
        public bool isSync = true;

        [Header("実行時のアニメーション")]
        public Animator    m_animator;
        public string      m_animation_name;

        [Header("実行時のSE")]
        public AudioSource m_audioSource;
        public AudioClip   m_sound;

        [Header("実行時に表示/非表示するオブジェクト")]
        public GameObject[] m_activeObjects;
        public GameObject[] m_desableObjects;

        [Header("実行時に同時に実行するUdonEvent")]
        public UdonBehaviour[] m_playUdons;
        public string[]        m_playUdonEvents;

        public override void Interact()
        {            
            if(isSync)  SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,nameof(Play));
            else        Play();
        }

        public void Play(){
            DisableInteractive = isOnce;
            if (m_audioSource != null && m_sound != null)
                m_audioSource.PlayOneShot(m_sound);
            if (m_animator != null && m_animation_name != null)
                m_animator.Play(m_animation_name);
            for (int i = 0; i < m_activeObjects.Length; i++)
            {
                m_activeObjects[i].SetActive(true);
            }
            for (int i = 0; i < m_desableObjects.Length; i++)
            {
                m_desableObjects[i].SetActive(false);
            }
            for (int i = 0; i < m_playUdons.Length; i++)
            {
                m_playUdons[i].SendCustomEvent(m_playUdonEvents[i]);
            }
        }
    }
}