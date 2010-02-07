namespace AutoBuild
{
    public interface IWriter
    {
        void WriteTitle(string message);
        void WriteError(string message);
        void WriteInfo(string message);
        void WriteDebug(string message);
        void WriteSuccess(string message);
    }
}