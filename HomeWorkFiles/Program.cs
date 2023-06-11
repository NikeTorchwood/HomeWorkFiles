using System.Text;

namespace HomeWorkFiles
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dirMask = "c:\\Otus\\TestDir";
            var fileNameMask = "File";
            var directoryInfos = GetDirs(dirMask, 2);
            await CreateFiles(fileNameMask, directoryInfos, 10);
            await PrintFilesInfo(directoryInfos);
            Console.ReadKey();
        }

        private static async Task PrintFilesInfo(List<DirectoryInfo> directoryInfos)
        {
            foreach (var directoryInfo in directoryInfos)
            {
                Console.WriteLine($"Печатаю файлы из папки{directoryInfo.FullName}");
                try
                {
                    var files = directoryInfo.GetFiles();
                    foreach (var fileInfo in files)
                    {
                        var content = GetString(await File.ReadAllLinesAsync(fileInfo.FullName));
                        Console.WriteLine($"""
                    Имя_файла:{fileInfo.Name}
                    (на всякий случай) Полное имя файла: {fileInfo.FullName}
                    {content}
                    """);
                    }
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine($"{directoryInfo.FullName} - папка не существует");
                }

                Console.WriteLine(new string('-', 30));
            }
        }
        private static async Task CreateFiles(string fileNameMask, List<DirectoryInfo> directoryInfos, int amountFiles)
        {
            foreach (var directoryInfo in directoryInfos)
            {
                if (!directoryInfo.Exists)
                {
                    Console.WriteLine($"{directoryInfo.FullName} папки не существует");
                    continue;
                }
                Console.WriteLine($"Создаем файлы в папке {directoryInfo.FullName}");
                for (var i = 1; i <= amountFiles; i++)
                {
                    var fileName = $"{fileNameMask}{i}.txt";
                    var path = $"{directoryInfo.FullName}\\{fileNameMask}{i}.txt";
                    try
                    {
                        await File.AppendAllLinesAsync(path, new[]
                        {
                            $"{fileName}",
                            $"{DateTime.Now}"}, Encoding.UTF8);

                    }
                    catch (IOException e)
                    {
                        Console.WriteLine($"файл занят {fileName}");
                    }

                    if (File.Exists(path))
                    {
                        Console.WriteLine($"{fileName} был создан");
                    }
                }
                Console.WriteLine(new string('-', 30));
            }
        }
        private static List<DirectoryInfo> GetDirs(string dirNameMask, int amount)
        {
            var result = new List<DirectoryInfo>();
            for (var i = 1; i <= amount; i++)
            {
                try
                {
                    var di = new DirectoryInfo($"{dirNameMask}{i}");
                    if (!di.Exists)
                    {
                        di.Create();
                        Console.WriteLine($"Папка {di.FullName} была создана");
                    }
                    result.Add(di);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine($"Нет прав на создания папки {dirNameMask}{i}");
                }
            }
            Console.WriteLine(new string('-', 30));
            return result;
        }
        private static string GetString(string[] strings)
        {
            var sb = new StringBuilder();
            sb.AppendJoin("\n", strings);
            sb.AppendLine("\n");
            sb.AppendLine(new string('-', 25));
            return sb.ToString();
        }
    }
}