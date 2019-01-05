using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public GameObject PlayerGO;
        public float InitialZ;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        private void Awake()
        {
            PlayerGO = GameObject.FindGameObjectWithTag("Player");
            target = PlayerGO.transform;
            InitialZ = PlayerGO.transform.position.z;

        }
        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
        }

        public void RoomLock(Transform Room)
        {
            target = Room;
        }
        public void Unlock()
        {
            PlayerGO = GameObject.FindGameObjectWithTag("Player");
            target = PlayerGO.transform;

        }


        // Update is called once per frame
        private void Update()
        {
            if (!target)
            {
                GameObject PlayerGO = GameObject.FindGameObjectWithTag("Player");
                target = PlayerGO.transform;
            }
            if (!target)
            {
                GameObject EnemyNext = GameObject.FindGameObjectWithTag("Enemy");
                target = EnemyNext.transform;
            }
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            newPos.z = InitialZ;
            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }

    }
}
