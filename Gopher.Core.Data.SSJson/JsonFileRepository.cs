using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Gopher.Core.Data.SSJson.Properties;
using Gopher.Core.Logging;
using Gopher.Core.Models;
using ServiceStack.Text;

namespace Gopher.Core.Data.SSJson {
  [Export(typeof(IFileRepository))]
  public class JsonFileRepository : IFileRepository {
    private readonly ILogger logger;
    private readonly string pathToJsonFile;
    int fileIndex = 1;

    [ImportingConstructor]
    public JsonFileRepository(ILogger logger) : this(Settings.Default.FileRepositoryPath, logger) { }

    /// <summary>
    /// Instantiates a new instance of the JSON File Repository
    /// </summary>
    /// <param name="pathToJsonFile">Full path to the file to use for the JSON File Repository</param>
    /// <param name="logger">Instance of logger</param>
    /// <param name="useExistingFile">Reuse existing file? Default: False</param>
    public JsonFileRepository(string pathToJsonFile, ILogger logger, bool useExistingFile = false) {
      this.pathToJsonFile = pathToJsonFile;
      this.logger = logger;

      if (!System.IO.File.Exists(pathToJsonFile)) {
        System.IO.File.CreateText(pathToJsonFile).Close();
      } else {
        if (useExistingFile) {
          IEnumerable<File> files = GetAllFiles();
          if (files.Count() != 0) {
            fileIndex = files.OrderByDescending(p => p.FileId).First().FileId + 1;
          }
        } else {
          System.IO.File.Delete(pathToJsonFile);
        }
      }
    }

    #region IFileRepository Members

    public File GetById(int fileId) {
      throw new NotImplementedException("This method is not implemented in the JsonFileRepository for performance reasons.");
    }

    public IEnumerable<File> GetAllFiles() {
      var files = new List<File>();
      using (var stream = new System.IO.StreamReader(pathToJsonFile)) {
        while (!stream.EndOfStream) {
          string fileJson = string.Empty;
          try {
            fileJson = stream.ReadLine();
            File file = JsonSerializer.DeserializeFromString<File>(fileJson);
            files.Add(file);
          } catch (Exception ex) {
            logger.ErrorException("Error deserializing File from json: " + fileJson, ex);
          }
        }
        stream.Close();
      }
      return files;
    }

    public IEnumerable<File> GetFilesInFolder(int folderId) {
      throw new NotImplementedException("This method is not implemented in the JsonFileRepository for performance reasons.");
    }

    public File Add(File fileToAdd) {
      fileToAdd.FileId = fileIndex++;
      string fileJson = JsonSerializer.SerializeToString(fileToAdd);
      using (var stream = new System.IO.StreamWriter(pathToJsonFile, append: true)) {
        stream.WriteLine(fileJson);
        stream.Close();
      }
      return fileToAdd;
    }

    public IEnumerable<File> Add(IEnumerable<File> filesToAdd) {
      var fileJsonArray = new StringBuilder();
      foreach (File file in filesToAdd) {
        file.FileId = fileIndex++;
        fileJsonArray.AppendLine(JsonSerializer.SerializeToString(file));
      }
      using (var stream = new System.IO.StreamWriter(pathToJsonFile, append: true)) {
        stream.Write(fileJsonArray);
        stream.Close();
      }
      return filesToAdd;
    }

    #endregion
  }
}
