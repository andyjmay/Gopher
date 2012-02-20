using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Gopher.Core.Logging;
using Gopher.Core.Models;
using Newtonsoft.Json;
using Gopher.Data.Json.Properties;

namespace Gopher.Core.Data.Json.Repositories {
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
    /// <param name="append">Append existing file? Default: False</param>
    public JsonFileRepository(string pathToJsonFile, ILogger logger, bool append = false) {
      this.pathToJsonFile = pathToJsonFile;
      this.logger = logger;

      if (!System.IO.File.Exists(pathToJsonFile)) {
        System.IO.File.CreateText(pathToJsonFile).Close();
      } else {
        if (append) {
          IEnumerable<File> files = GetAllFiles();
          if (files.Count() != 0) {
            fileIndex = files.OrderByDescending(p => p.FileId).First().FileId + 1;
          }
        }
        //throw new Exception("The file database already exists");
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
            File file = JsonConvert.DeserializeObject<File>(fileJson);
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
      string fileJson = JsonConvert.SerializeObject(fileToAdd);
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
        fileJsonArray.AppendLine(JsonConvert.SerializeObject(file));
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
