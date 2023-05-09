using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class OnEnableAnimationPlayer : UdonSharpBehaviour
    {

        [Header("コントロールするアニメーター")]
        public Animator       animator;
        [Header("アニメーションの名前")]
        public string         paramatorName;
        [Header("(Option)一度だけか")]
        public bool         　isOnce;

        bool isPlay = false;

        void OnEnable(){
            if(isPlay){                
                return;
            }
            animator.Play(paramatorName); 
            isPlay = 　isOnce;
        }
    }
}