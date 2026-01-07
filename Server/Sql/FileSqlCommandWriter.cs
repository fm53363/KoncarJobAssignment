namespace Server.Sql
{
    internal class FileSqlCommandWriter : ISqlCommandWriter
    {

        private readonly string _filePath = "myDb.txt";

        public async Task WriteAsync(string command)
        {
            await File.AppendAllTextAsync(_filePath, command + Environment.NewLine);
        }
    }
}
