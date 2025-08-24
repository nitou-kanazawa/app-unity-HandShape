using UnityEngine;

namespace Project.InGame.Presentation
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ViewBase : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup => _canvasGroup !=null ? _canvasGroup : (_canvasGroup = GetComponent<CanvasGroup>()); 


        public abstract void Initialize();

        public virtual void Show()
        {
            CanvasGroup.alpha = 1f;
        }

        public virtual void Hide()
        {
            CanvasGroup.alpha = 0f;
        }
    }
}
