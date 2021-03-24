namespace SubTime.Contracts
{
    public interface ISubtitle
    {
        string Pattern { get; }
        string ParseExactPattern { get; }

        void SyncTime(string fileName, int timeBySeconds);
        bool IsCompatible(string content);
    }
}