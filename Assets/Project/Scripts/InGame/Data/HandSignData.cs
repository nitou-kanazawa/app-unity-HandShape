using UnityEngine;

namespace Project.InGame.Data
{
    [CreateAssetMenu(fileName = "HandSignData", menuName = "Project/InGame/HandSignData")]
    public class HandSignData : ScriptableObject
    {
        [SerializeField] private string signName;
        [SerializeField] private Sprite signSprite;
        [SerializeField] private int signId;

        public string SignName => signName;
        public Sprite SignSprite => signSprite;
        public int SignId => signId;
    }
}