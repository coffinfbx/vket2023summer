using UnityEditor;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    [CustomEditor(typeof(OnInteractEvent))]
    public class OnInteractEvent_Editor : Editor
    {
        int m_list_size = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();      //serializedObjectを最新に更新
            {// 標準インスペクタの表示
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("isOnce") , true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("isSync") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_animator") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_animation_name") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_audioSource") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_sound") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_activeObjects") , true);       
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_desableObjects") , true);          
            }

            var m_playUdons      = serializedObject.FindProperty("m_playUdons");
            var m_playUdonEvents = serializedObject.FindProperty("m_playUdonEvents");                 
       
            m_list_size = m_playUdons.arraySize;       //長さを一時的に変数に保存しておく
            m_list_size = EditorGUILayout.DelayedIntField("イベント数", m_list_size);//一時的に保存した配列の長さをカスタムインスペクタに描画（ここで書き換えも可能）

            EditorGUILayout.Space();       //スペースを描画

            //一時的に保存した配列の長さと、本来の配列の長さが同じかチェックする
            if (m_list_size != m_playUdons.arraySize)
            {
                //長さが違う場合は
                m_playUdons     .arraySize          = m_list_size;     //長さの変更を適用
                m_playUdonEvents.arraySize               = m_list_size;

                //ここでserializedObjectへの変更を適用し、再び更新する
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            else
            {
                OnInteractEvent udon = target as OnInteractEvent; 
                EditorGUILayout.LabelField ("実行するUdon");
                //一時的に保存した配列の長さと、本来の配列の長さが同じ場合は　配列の要素を描画する                 
                for (int i = 0; i < m_playUdons.arraySize; ++i)
                {
                    string st_i = (i+1).ToString();
                    EditorGUILayout.LabelField ("No.", st_i);
                    udon.m_playUdons[i]      = EditorGUILayout.ObjectField ("Udon"     , udon.m_playUdons[i],typeof(UdonBehaviour),true) as UdonBehaviour ;
                    udon.m_playUdonEvents[i] = EditorGUILayout.TextField   ("イベント名",udon.m_playUdonEvents[i]);
                    EditorGUILayout.Space();  
                }            
            }
//            base.OnInspectorGUI();      //正しく動作しているか確認にするために、元のインスペクターを表示

            serializedObject.ApplyModifiedProperties(); //serializedObjectへの変更を適用
        }
    }
}