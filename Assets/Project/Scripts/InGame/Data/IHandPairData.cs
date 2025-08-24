
namespace Project.InGame.Data
{
    public interface IHandPairData
    {
        /// <summary>
        /// 
        /// </summary>
        IHandSignData MasterHandSign { get; }

        /// <summary>
        /// 
        /// </summary>
        IHandSignData PlayerHandSign { get; }

        /// <summary>
        /// 
        /// </summary>
        int BaseScore { get; }
    }
}
