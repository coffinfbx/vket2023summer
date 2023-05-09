using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SimpleUDONGimmick
{
    public class SimpleObjectSync : UdonSharpBehaviour
    {
        [UdonSynced]
        Vector3 pos;

        [UdonSynced]
        Quaternion rot;

        Vector3 pos_old;
        Quaternion rot_old;

        Transform m_transform;

        void Start()
        {
            m_transform = transform;
            pos_old = m_transform.position;
            rot_old = m_transform.rotation;
            if(Networking.IsOwner(this.gameObject))
            {
                pos = m_transform.position;
                rot = m_transform.rotation;
            }   
        }
        void  Update()
        {
            if(Networking.IsOwner(this.gameObject)){
                pos = m_transform.position;
                rot = m_transform.rotation;
            }
        }

        override public void OnDeserialization()
        {
            if(!Networking.IsOwner(this.gameObject)){
                m_transform.position = Vector3.Lerp(transform.position, pos, 0.5f);
                m_transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.5f);
            }
        }
    }
}
