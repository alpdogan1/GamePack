using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePack
{
    public class FollowObject: MonoBehaviour
    {
        private enum UpdateType { Update, LateUpdate }
        
        [SerializeField] private GameObject _ObjectToFollow;
        [SerializeField] private Vector3 _Offset;
        [Space]
        [SerializeField, Required] private bool _IsLerp = true;
        [SerializeField, Required, ShowIf("_IsLerp")] private float _LerpSpeed = 5;
        [Space]
        [SerializeField, Required] private UpdateType _UpdateType = UpdateType.LateUpdate;
        
        private Vector3 TargetPosition => _ObjectToFollow.transform.position + _Offset;

        private void Update()
        {
            if(_UpdateType != UpdateType.Update) return;

            TargetUpdate();
        }

        private void LateUpdate()
        {
            if(_UpdateType != UpdateType.LateUpdate) return;

            TargetUpdate();
        }
        
        private void TargetUpdate()
        {
            if (!_ObjectToFollow) return;
            
            if (_IsLerp)
                transform.position = Vector3.Lerp(transform.position, TargetPosition,
                    _LerpSpeed * Time.deltaTime);
            else
                transform.position = TargetPosition;
        }

        public void SetObjectToFollow(GameObject toFollow, Vector3? offset = null)
        {
            _Offset = offset ?? Vector3.zero;
            _ObjectToFollow = toFollow;
        }
        
        #region Development
#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        private void GetCurrentOffset()
        {
            _Offset =  transform.position - _ObjectToFollow.transform.position;
        } 
#endif
        #endregion
    }
}