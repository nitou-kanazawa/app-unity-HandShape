using UnityEngine;
using UnityEngine.UI;
using R3;

namespace Project.InGame.Presentation
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class HandSignChoiceButton : MonoBehaviour
    {
        [SerializeField] Image _thumbnailImage;
        [SerializeField] Button _button;


        public int ButtonIndex { get; private set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public Observable<Unit> OnClickAsObservable => _button.OnClickAsObservable();


        public void SetThumbnail(Sprite thumbnail)
        {
            if (_thumbnailImage == null)
            {
                Debug.LogError("Thumbnail Image component is not assigned.");
                return;
            }
            _thumbnailImage.sprite = thumbnail;
        }

        public void ClearThumbnail()
        {
            _thumbnailImage.sprite = null;
        }


        public void SetButtonIndex(int index)
        {
            ButtonIndex = index;
        }
    }
}
