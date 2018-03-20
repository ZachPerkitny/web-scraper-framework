namespace ScraperFramework.Sync
{
    public interface ISyncTask
    {
        /// <summary>
        /// 
        /// </summary>
        int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        void Execute();
    }
}
