using Project.InGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Project.InGame.Presentation
{
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class HandSignView : ViewBase
    {
        [SerializeField] Image _thumbnailImage;


        public override void Initialize()
        {
        }

        public void SetHandSign(IHandSignData handSignData)
        {
            if (_thumbnailImage == null)
            {
                Debug.LogError("Thumbnail Image component is not assigned.");
                return;
            }
            _thumbnailImage.sprite = handSignData.SignSprite;
            _thumbnailImage.enabled = true;
        }

        public void ClearHandSign()
        {
            _thumbnailImage.sprite = null;
            _thumbnailImage.enabled = false;
        }
    }
}
