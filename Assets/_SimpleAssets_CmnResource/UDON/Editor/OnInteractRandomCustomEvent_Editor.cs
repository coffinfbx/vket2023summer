using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleUDONGimmick
{
    [CustomEditor(typeof(OnInteractRandomCustomEvent))]
    public class OnInteractRandomCustomEvent_Editor : Editor
    {
        int m_list_size = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();      //serializedObjectを最新に更新
            {// 標準インスペクタの表示
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("isSync") , true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("targetUdon") , true);                
            }

            EditorGUILayout.LabelField("実行イベント");
            var randomEventNames    = serializedObject.FindProperty("randomEventNames");
            var randomRates         = serializedObject.FindProperty("randomRates");                 
       
            m_list_size = randomEventNames.arraySize;       //長さを一時的に変数に保存しておく
            m_list_size = EditorGUILayout.DelayedIntField("イベント数", m_list_size);//一時的に保存した配列の長さをカスタムインスペクタに描画（ここで書き換えも可能）

            EditorGUILayout.Space();       //スペースを描画

            //一時的に保存した配列の長さと、本来の配列の長さが同じかチェックする
            if (m_list_size != randomEventNames.arraySize)
            {
                //長さが違う場合は
                randomEventNames.arraySize          = m_list_size;     //長さの変更を適用
                randomRates.arraySize               = m_list_size;

                //ここでserializedObjectへの変更を適用し、再び更新する
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            else
            {
                OnInteractRandomCustomEvent udon = target as OnInteractRandomCustomEvent; 
                EditorGUILayout.LabelField ("イベントデータ");
                //一時的に保存した配列の長さと、本来の配列の長さが同じ場合は　配列の要素を描画する                 
                for (int i = 0; i < randomEventNames.arraySize; ++i)
                {
                    string st_i = (i+1).ToString();
                    EditorGUILayout.LabelField ("No.", st_i);
                    udon.randomEventNames[i] = EditorGUILayout.TextField ("イベント名"    , udon.randomEventNames[i]);
                    EditorGUILayout.LabelField ("イベントの重み");
                    udon.randomRates[i]      = EditorGUILayout.Slider  (udon.randomRates[i], 0, 100);
                    EditorGUILayout.Space();  
                }            
            }
//            base.OnInspectorGUI();      //正しく動作しているか確認にするために、元のインスペクターを表示

            serializedObject.ApplyModifiedProperties(); //serializedObjectへの変更を適用
        }
    }
}