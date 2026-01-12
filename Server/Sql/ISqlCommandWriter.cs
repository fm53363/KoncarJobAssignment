namespace Server.Sql
{
    interface ISqlCommandWriter
    {
        Task WriteAsync(string command);
    }
}
