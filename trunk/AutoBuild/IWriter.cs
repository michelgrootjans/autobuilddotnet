namespace AutoBuild
{
    internal interface IWriter
    {
        void WriteTitle(string message);
        void WriteError(string message);
        void WriteMessage(string message);
        void WriteDebug(string message);
    }
}