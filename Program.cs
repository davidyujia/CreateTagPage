using System;
using System.Collections.Generic;
using System.Linq;

namespace CreateTagPage
{
    class Program
    {
        static void Main(string[] args)
        {
            var tags = new List<string>();
            var files = System.IO.Directory.GetFiles("_posts");
            foreach (var file in files)
            {
                var tagArray = GetTags(file);

                tags.AddRange(tagArray);
            }

            var folderName = "tag";
            if (!System.IO.Directory.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
            foreach (var tag in tags.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)).ToList())
            {
                var path = $"{folderName}\\{tag}.md";
                if (System.IO.File.Exists(path))
                {
                    continue;
                }

                System.IO.File.AppendAllLines(path, new[]{
                    "---",
                    "layout: tagPage",
                    $"title: \"Tag: {tag}\"",
                    $"tag: {tag}",
                    "---"
                });
            }
        }

        static string[] GetTags(string file)
        {
            var firstLine = true;
            foreach (var line in System.IO.File.ReadAllLines(file))
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }
                if (line.StartsWith("---"))
                {
                    break;
                }

                var lineArray = line.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                if (lineArray[0] != "tags")
                {
                    continue;
                }

                var tagArray = lineArray[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                return tagArray;
            }

            return Array.Empty<string>();
        }
    }
}
