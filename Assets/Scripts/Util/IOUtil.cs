using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public static class IOUtil
{
    public static void CreateDirectoryIfNotExist(string path)
    {
        try
        {
            var directoryExist = Directory.Exists(path);
            if (directoryExist == false)
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (Exception e)
        {
            Logger.Error($"CreateDirectoryIfNotExist failed. path={path}. e={e}", typeof(IOUtil));
        }
    }

    public static string CombinePath(params object[] tokens)
    {
        if (tokens.IsEmptyExt())
        {
            return null;
        }

        var builder = new StringBuilder();
        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if (token == null)
            {
                continue;
            }

            if (i == 0)
            {
                builder.Append(token.ToString());
            }
            else
            {
                builder.AppendSlashExt(token.ToString());
            }
        }

        return builder.ToString();
    }

    public static string GetFullPath(string s)
    {
        return System.IO.Path.GetFullPath(s);
    }

    public static string GetExtension(string path)
    {
        if (path.IsEmptyExt())
        {
            return null;
        }

        return System.IO.Path.GetExtension(path);
    }

    public static string GetFileName(string path)
    {
        if (path.IsEmptyExt())
        {
            return null;
        }

        return System.IO.Path.GetFileName(path);
    }

    public static string GetFileNameWithoutExtension(string path)
    {
        if (path.IsEmptyExt())
        {
            return null;
        }

        return System.IO.Path.GetFileNameWithoutExtension(path);
    }

    public static void CreateDirectoryForFile(string filePath)
    {
        try
        {
            var directoryName = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }
        }
        catch (Exception e)
        {
            Logger.Error($"CreateDirectoryForFile failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }
    }

    public static void CopyFile(string sourceFilePath, string destFilePath)
    {
        try
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        catch (Exception e)
        {
            Logger.Error($"CopyFile failed. sourceFilePath={sourceFilePath}. destFilePath={destFilePath}. e={e}", typeof(IOUtil));
        }
    }

    public static string GetFileContent(string filePath, bool suppressError = false)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            if (suppressError == false)
            {
                Logger.Error($"GetFileContent failed. filePath={filePath}. e={e}", typeof(IOUtil));
            }
        }

        return string.Empty;
    }

    public static string[] GetFileLines(string filePath, bool suppressError = false)
    {
        try
        {
            return File.ReadAllLines(filePath);
        }
        catch (Exception e)
        {
            if (suppressError == false)
            {
                Logger.Error($"GetFileLines failed. filePath={filePath}. e={e}", typeof(IOUtil));
            }
        }

        return null;
    }

    public static void ForEachFileLines(string filePath, Action<int, string> action, Func<bool> stopper = null)
    {
        if (action == null)
        {
            return;
        }

        using (var reader = File.OpenText(filePath))
        {
            var lineNumber = 0;
            while (true)
            {
                var isStopped = stopper != null && stopper();
                if (isStopped)
                {
                    break;
                }

                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                action.RunExt(++lineNumber, line);
            }
        }
    }

    public static byte[] GetFileContentBytes(string filePath)
    {
        try
        {
            return File.ReadAllBytes(filePath);
        }
        catch (Exception e)
        {
            Logger.Error($"GetFileContentBytes failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }

        return null;
    }

    public static int GetFileLength(string fileName)
    {
        if (File.Exists(fileName) == false)
        {
            return 0;
        }

        try
        {
            var fileInfo = new System.IO.FileInfo(fileName);
            return (int)fileInfo.Length;
        }
        catch (Exception e)
        {
            Logger.Error($"GetFileLength failed. filePath={fileName}. e={e}", typeof(IOUtil));
            return 0;
        }
    }

    public static void WriteToFile(string filePath, string content)
    {
        try
        {
            CreateDirectoryForFile(filePath);
            File.WriteAllText(filePath, content);
        }
        catch (Exception e)
        {
            Logger.Error($"WriteToFile failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }
    }

    public static void WriteToFile(string filePath, IList<string> lines)
    {
        try
        {
            CreateDirectoryForFile(filePath);
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception e)
        {
            Logger.Error($"WriteToFile failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }
    }

    public static void AppendToFile(string filePath, string content)
    {
        try
        {
            CreateDirectoryForFile(filePath);
            File.AppendAllText(filePath, content);
        }
        catch (Exception e)
        {
            Logger.Error($"AppendToFile failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }
    }

    public static bool FileExist(string filePath)
    {
        try
        {
            return File.Exists(filePath);
        }
        catch (Exception e)
        {
            Logger.Error($"FileExist failed. filePath={filePath}. e={e}", typeof(IOUtil));
        }

        return false;
    }

    public static bool DirectoryExist(string directoryPath)
    {
        try
        {
            return Directory.Exists(directoryPath);
        }
        catch (Exception e)
        {
            Logger.Error($"DirectoryExist failed. filePath={directoryPath}. e={e}", typeof(IOUtil));
        }

        return false;
    }

    public static string GetDirectoryName(string path)
    {
        try
        {
            return System.IO.Path.GetDirectoryName(path);
        }
        catch (Exception e)
        {
            Logger.Error($"GetDirectoryName failed. filePath={path}. e={e}", typeof(IOUtil));
        }

        return null;
    }

    public static bool DeleteFile(string filePath, bool throwOnError = false)
    {
        try
        {
            File.Delete(filePath);
            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"DeleteFile failed. filePath={filePath}. e={e}", typeof(IOUtil));

            if (throwOnError)
            {
                throw;
            }
        }

        return false;
    }

    public static bool DeleteFileIfExist(string filePath, bool throwOnError = false)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"DeleteFileIfExist failed. filePath={filePath}. e={e}", typeof(IOUtil));

            if (throwOnError)
            {
                throw;
            }
        }

        return false;
    }

    public static bool DeleteDirectory(string directoryPath, bool throwOnError = false)
    {
        try
        {
            Directory.Delete(directoryPath, true);
            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"DeleteDirectory failed. filePath={directoryPath}. e={e}", typeof(IOUtil));

            if (throwOnError)
            {
                throw;
            }
        }

        return false;
    }

    public static bool DeleteDirectoryIfExist(string directoryPath, bool throwOnError = false)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            return true;
        }
        catch (Exception e)
        {
            Logger.Error($"DeleteDirectoryIfExist failed. filePath={directoryPath}. e={e}", typeof(IOUtil));

            if (throwOnError)
            {
                throw;
            }
        }

        return false;
    }

    public static IEnumerable<string> GetFilesOfExtensionInDirectory(string rootPath, bool recursive, IReadOnlyList<string> extensions)
    {
        foreach (var filePath in GetFilesInDirectory(rootPath, recursive))
        {
            if (extensions.IsEmptyExt() || filePath.HasExtensionExt(extensions))
            {
                yield return filePath;
            }
        }
    }

    public static IEnumerable<string> GetFilesInDirectory(string rootPath, bool recursive, string searchPattern = null)
    {
        IEnumerator<string> filePaths = null;

        try
        {
            var option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (searchPattern.IsEmptyExt())
            {
                filePaths = Directory.EnumerateFiles(rootPath, "*", option).GetEnumerator();
            }
            else
            {
                filePaths = Directory.EnumerateFiles(rootPath, searchPattern, option).GetEnumerator();
            }
        }
        catch (Exception e)
        {
            Logger.Error($"GetFilesInDirectory failed. filePath={rootPath}. e={e}", typeof(IOUtil));
            yield break;
        }

        if (filePaths == null)
        {
            yield break;
        }

        while (true)
        {
            string filePath;
            try
            {
                var moved = filePaths.MoveNext();
                if (moved == false)
                {
                    break;
                }

                filePath = filePaths.Current;
            }
            catch (Exception e)
            {
                Logger.Error($"GetFilesInDirectory failed. filePath={rootPath}. e={e}", typeof(IOUtil));
                yield break;
            }

            if (filePath.IsEmptyExt())
            {
                continue;
            }

            yield return filePath;
        }
    }

    public static IEnumerable<string> GetSubDirectories(string rootPath)
    {
        string[] subDirectories = null;

        try
        {
            subDirectories = Directory.GetDirectories(rootPath);
        }
        catch (Exception e)
        {
            Logger.Error($"GetSubDirectories failed. filePath={rootPath}. e={e}", typeof(IOUtil));
        }

        if (subDirectories.IsEmptyExt())
        {
            yield break;
        }

        foreach (var subDirectory in subDirectories)
        {
            yield return subDirectory;
        }
    }

    public static FileStream OpenFileStreamForRead(string path)
    {
        try
        {
            return File.OpenRead(path);
        }
        catch (Exception e)
        {
            Logger.Error($"OpenFileStreamForRead failed. filePath={path}. e={e}", typeof(IOUtil));
            return null;
        }
    }

    public static StreamReader OpenFileForTextRead(string path)
    {
        try
        {
            return File.OpenText(path);
        }
        catch (Exception e)
        {
            Logger.Error($"OpenFileForTextRead failed. filePath={path}. e={e}", typeof(IOUtil));
            return null;
        }
    }

    public static FileStream OpenFileStreamForWrite(string path)
    {
        try
        {
            CreateDirectoryForFile(path);
            return File.Open(path, FileMode.Create, FileAccess.Write);
        }
        catch (Exception e)
        {
            Logger.Error($"OpenFileStreamForWrite failed. filePath={path}. e={e}", typeof(IOUtil));
            return null;
        }
    }
}