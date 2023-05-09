
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class TargetActiveEvent : UdonSharpBehaviour
    {
        [Header("活性化するオブジェクト")]
        public GameObject[] actives;
        [Header("非活性にするオブジェクト")]
        public GameObject[] disables;

        [Header("一度だけか")]
        public bool isOnece = true;

        [Header("(Option) オブジェクトがアクティブ化したら起動する")]
        public bool  IsActive  = false;

        bool Played = false;

        void OnEnable()
        {
            if(IsActive)
                Play();
        }

        public void Play()
        {
            if(Played)
                return;
            Played = isOnece;
            for (int i = 0; i < actives.Length; i++)
            {
                actives[i].SetActive(true);
            }
            for (int i = 0; i < disables.Length; i++)
            {
                disables[i].SetActive(false);
            }
        }
    }
}