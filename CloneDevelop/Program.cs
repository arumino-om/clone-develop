namespace CloneDevelop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- CloneDevelop ---");

            if (args.Length < 2)
            {
                Console.WriteLine("usage: this.exe <from> <to>");
                return;
            }

            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("directory is not found");
                return;
            }

            CloneDirectory(args[1], args[2]);
        }

        static bool CloneDirectory(string from, string to)
        {
            var ignoreDirectories = new List<string>()
            {
                "node_modules",
                ".vs"
            };

            var files = Directory.GetFiles(from);
            foreach (var file in files)
            {
                // VS関係プロジェクトの場合は bin, obj ディレクトリを除外する
                if (file.EndsWith("proj")) ignoreDirectories.AddRange(new string[] { "bin", "obj"});

                Console.WriteLine($"  COPY  {from} => {to}");
                var filename = Path.GetFileName(file);
                File.Copy(file, Path.Combine(to, filename));
            }

            var dirs = Directory.GetDirectories(from);
            foreach (var dir in dirs)
            {
                var dirname = Path.GetDirectoryName(dir);

                if (ignoreDirectories.Contains(dirname))
                {
                    Console.WriteLine($"  SKIP  {dir}");
                    continue;
                }

                CloneDirectory(Path.Combine(from, dirname), Path.Combine(to, dirname));
            }

            return true;
        }
    }
}