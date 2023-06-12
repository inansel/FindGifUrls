using System;
using System.Net.Sockets;

namespace FindGifUrls
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                FindGifUrls(args[0]);
            }
            else
            {
                FindGifUrls(".\\DummyLogData1.txt");
            }
        }

        static void FindGifUrls(string fileName)
        {
            int fromLastColumn = 3;
            char delimiter = '|';
            string gifFileName = String.Format(".\\gif_{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            DateTimeOffset entryDt = DateTimeOffset.Now;
            if (File.Exists(fileName))
            {
                using (FileStream logStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader logReader = new StreamReader(logStream))
                    {
                        using (FileStream gifStream = new FileStream(gifFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                        {
                            using (StreamWriter gifWriter = new StreamWriter(gifStream))
                            {
                                string line = logReader.ReadLine();
                                while (!String.IsNullOrEmpty(line))
                                {
                                    string[] fields = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                                    if (DateTimeOffset.TryParse(fields[0], out entryDt))
                                    {
                                        string url = fields[fields.Length - fromLastColumn];
                                        int queryBegin = url.IndexOf("?");
                                        int lastPathSegment = url.LastIndexOf("/");
                                        string urlFileName = null;
                                        if (queryBegin > -1)
                                        {

                                            
                                            urlFileName = url.Substring(lastPathSegment + 1, queryBegin - lastPathSegment - 1);

                                        }
                                        else
                                        {
                                           urlFileName = url.Substring(lastPathSegment + 1, url.Length - lastPathSegment - 1);
                                        }
                                        if (urlFileName.EndsWith(".gif"))
                                        {
                                            gifWriter.WriteLine(urlFileName);
                                        }
                                    }
                                    line = logReader.ReadLine();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not find log file.");
            }
        }
    }
}