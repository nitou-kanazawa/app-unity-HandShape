using UnityEngine;
using UnityEngine.EventSystems;
using LitMotion;
using LitMotion.Extensions;


namespace Project
{
    public class ScaleHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tween settings")]
        [SerializeField] private float _normalScale = 1f;
        [SerializeField] private float _hoverScale = 1.2f;
        [SerializeField] private float _enterDuration = 0.3f;
        [SerializeField] private float _exitDuration = 0.1f;
        [SerializeField] private Ease _easeType = Ease.OutBack;

        private CompositeMotionHandle _motionHandles = new (2);
        private Vector3 _originalScale;


        #region Unity Lifecycle Events

        private void Awake()
        {
            _originalScale = transform.localScale;
        }
        
        private void OnDestroy()
        {
            _motionHandles.Cancel();
        }
        #endregion


        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _motionHandles.Cancel();

            Vector3 targetScale = _originalScale * _hoverScale;
            LMotion.Create(transform.localScale, targetScale, _enterDuration)
                .WithEase(_easeType)
                .BindToLocalScale(transform)
                .AddTo(_motionHandles);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _motionHandles.Cancel();

            Vector3 targetScale = _originalScale * _normalScale;
            LMotion.Create(transform.localScale, targetScale, _exitDuration)
                .WithEase(_easeType)
                .BindToLocalScale(transform)
                .AddTo(_motionHandles);
        }


        [ContextMenu("Reset Scale")]
        private void ResetScale()
        {
            _motionHandles.Cancel();
            transform.localScale = _originalScale * _normalScale;
        }
    }
}
