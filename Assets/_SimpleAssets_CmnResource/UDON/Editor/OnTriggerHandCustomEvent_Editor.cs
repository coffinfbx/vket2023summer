using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleUDONGimmick
{
    [CustomEditor(typeof(OnTriggerHandCustomEvent))]
    public class OnTriggerHandCustomEvent_Editor : Editor
    {
        int m_list_size = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();      //serializedObjectを最新に更新
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("IsOnece") , true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("reloadTime") , true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("isInteractActive") , true); 
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("pickup") , true); 
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("isNotPickupedHand") , true); 
                OnTriggerHandCustomEvent udon = target as OnTriggerHandCustomEvent; 

                udon.switchRange         = EditorGUILayout.FloatField ("(Option) スイッチの範囲"                  , udon.switchRange);
                udon.switchRangeCollider = EditorGUILayout.ObjectField ("(Option) スイッチの範囲をコライダで管理"   , udon.switchRangeCollider, typeof(SphereCollider), true) as SphereCollider;
                if(udon.switchRangeCollider != null){
                    udon.switchRange = udon.switchRangeCollider.radius;
                }

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("udonBehaviour") , true); 
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("udonEventName") , true); 
            }

            EditorGUILayout.Space();       //スペースを描画

//            base.OnInspectorGUI();      //正しく動作しているか確認にするために、元のインスペクターを表示

            serializedObject.ApplyModifiedProperties(); //serializedObjectへの変更を適用
        }
    }
}