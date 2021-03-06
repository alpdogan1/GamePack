using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePack
{
    public abstract class SlingCarControllerBase : MonoBehaviour
    {
        [SerializeField, Required] protected SlingCar _SlingCar;
        [SerializeField, Required] protected ForwardRoad _Road;
        [SerializeField, Required] private float _DetectionSideSpeedLimit = 0.3f;
        [SerializeField, Required] private float _FindFrontCarTime = 2f;
        [SerializeField, Required] private LayerMask _LayerMask;
        [SerializeField, ReadOnly] private float _CarLength;
        [SerializeField] private Collider _Collider;

        [ShowInInspector] protected virtual float TargetSpeed
        {
            get => _SlingCar.TargetSpeed;
            set => _SlingCar.TargetSpeed = value;
        }

        private float FindFrontCarDistance => SlingCar.Speed * _FindFrontCarTime;
        public SlingCar SlingCar => _SlingCar;
        private Collider Collider => _Collider;
        public Vector3 Center => Collider.bounds.center;

        private void OnValidate()
        {
            SlingCar.Controller = this;
            _Collider = GetComponentInChildren<Collider>();
            if(Collider)
                _CarLength = Collider.bounds.size.z;
        }

        private void FixedUpdate()
        {
            Debug.DrawRay(Collider.bounds.center, Vector3.forward * (FindFrontCarDistance + (_CarLength / 2)), Color.magenta);
            // if (Mathf.Abs(SlingCar.SideSpeed) > _DetectionSideSpeedLimit)
            // {
                // SlingCar.TargetSpeed = TargetSpeed;
            // }
            if (Physics.Raycast(Collider.bounds.center, Vector3.forward,
                out var hitInfo,
                FindFrontCarDistance + (_CarLength / 2),
                _LayerMask))
            {
                var frontCar = hitInfo.transform.GetComponent<SlingCar>();
                
                if(frontCar && frontCar != _SlingCar)
                    FoundCar(frontCar);
            }
            // else
            // {
            
                SlingCar.TargetSpeed = TargetSpeed;
                
            // }
        }

        protected virtual void FoundCar(SlingCar slingCar)
        {
            
        }

        public virtual void SetRoad(ForwardRoad road)
        {
            _Road = road;
        }
    }
}