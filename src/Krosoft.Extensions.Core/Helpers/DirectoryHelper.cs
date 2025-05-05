using System.Globalization;

namespace Krosoft.Extensions.Core.Helpers;

public static class DirectoryHelper
{
    public static void ClearDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            var di = new DirectoryInfo(directoryPath);
            di.Delete(true);
        }
    }

    /// <summary>
    /// Créer un dossier s'il n'existe pas
    /// </summary>
    /// <param name="pathFolder">le chemin du dossier</param>
    /// <returns>True si on créé le dossier, false sinon</returns>
    public static bool CreateDirectoryIfNotExist(string pathFolder)
    {
        var isExist = Directory.Exists(pathFolder);
        if (!isExist)
        {
            Directory.CreateDirectory(pathFolder);
        }

        return !isExist;
    }

    /// <summary>
    /// </summary>
    /// <param name="directoryPath">Chemin du dossier.</param>
    public static void DeleteAllInDirectory(string directoryPath)
    {
        var exists = Directory.Exists(directoryPath);
        if (exists)
        {
            var di = new DirectoryInfo(directoryPath);

            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }

    public static IEnumerable<string> GetFiles(string path,
                                               bool recursif = false,
                                               IList<string>? extensionsFilter = null)
    {
        IEnumerable<string> filePaths;
        if (recursif)
        {
            filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }
        else
        {
            filePaths = Directory.GetFiles(path);
        }

        if (extensionsFilter != null && extensionsFilter.Any())
        {
            filePaths = filePaths.Where(s =>
            {
                var extension = Path.GetExtension(s);
                return !string.IsNullOrEmpty(extension) && extensionsFilter.Contains(extension.ToLower());
            });
        }

        return filePaths;
    }

    /// <summary>
    /// Renvoie la liste des fichiers catégoriser par date
    /// </summary>
    /// <param name="directory">le répertoire contenant les fichiers</param>
    /// <param name="ignoreToday">true si on doit ignorer les fichiers du jour</param>
    /// <returns>la liste des fichiers catégoriser par date</returns>
    public static Dictionary<string, IList<FileInfo>> GetFilesOrderByDate(DirectoryInfo directory, bool ignoreToday = true)
    {
        var allFiles = directory.GetFiles();
        var filesByExtension = new Dictionary<string, IList<FileInfo>>();
        foreach (var fileInfo in allFiles)
        {
            if (ignoreToday)
            {
                if (fileInfo.LastWriteTime.Date == DateTime.Now.Date)
                {
                    continue;
                }
            }

            var critere = fileInfo.LastWriteTime.ToString("yyyy_MM_dd", CultureInfo.InvariantCulture);
            if (!filesByExtension.ContainsKey(critere))
            {
                filesByExtension.Add(critere, new List<FileInfo>());
            }

            filesByExtension[critere].Add(fileInfo);
        }

        return filesByExtension;
    }

    /// <summary>
    /// Renvoie la liste des fichiers catégoriser par date
    /// </summary>
    /// <param name="path">le chemin du répertoire contenant les fichiers</param>
    /// <param name="ignoreToday">true si on doit ignorer les fichiers du jour</param>
    /// <returns>la liste des fichiers catégoriser par extension</returns>
    public static Dictionary<string, IList<FileInfo>> GetFilesOrderByDate(string path, bool ignoreToday = true)
    {
        var aDirectorySource = new DirectoryInfo(path);
        return GetFilesOrderByDate(aDirectorySource, ignoreToday);
    }

    /// <summary>
    /// Permet d'avoir un nom unique, si le fichier existe déjà.
    /// </summary>
    /// <param name="fullPath">le nom d'origine</param>
    /// <returns>le nom modifié</returns>
    public static string GetUniqueName(string fullPath)
    {
        var count = 1;

        var fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
        var extension = Path.GetExtension(fullPath);
        var path = Path.GetDirectoryName(fullPath);
        var newFullPath = fullPath;

        if (!string.IsNullOrEmpty(path))
        {
            while (File.Exists(newFullPath))
            {
                var tempFileName = $"{fileNameOnly}_{count++}";
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }

        return fullPath;
    }
}