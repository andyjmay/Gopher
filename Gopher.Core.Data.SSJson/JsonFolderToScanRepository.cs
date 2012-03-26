using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Gopher.Core.Data.SSJson.Properties;
using Gopher.Core.Logging;
using Gopher.Core.Models;
using ServiceStack.Text;

namespace Gopher.Core.Data.SSJson {
  [Export(typeof(FolderToScanRepositoryBase))]
  public class JsonFolderToScanRepository : FolderToScanRepositoryBase {
    private string pathToJsonFile;
    private int folderToScanIndex;

    [Import]
    private ILogger Logger { get; set; }

    public JsonFolderToScanRepository() : this(Settings.Default.FolderToScanRepositoryPath) { }

    public JsonFolderToScanRepository(string pathToJsonFile) {
      this.pathToJsonFile = pathToJsonFile;
      if (!System.IO.File.Exists(pathToJsonFile)) {
        System.IO.File.CreateText(pathToJsonFile).Close();
        folderToScanIndex = 1;
      } else {
        var currentFolders = GetFoldersToScan();
        if (!currentFolders.Any()) {
          folderToScanIndex = 1;
        } else {
          folderToScanIndex = currentFolders.OrderByDescending(f => f.FolderToScanId).First().FolderToScanId + 1;
        }
      }
    }

    public override FolderToScan GetById(int folderToScanId) {
      return GetFoldersToScan().Single(f => f.FolderToScanId == folderToScanId);
    }

    public override IEnumerable<FolderToScan> GetFoldersToScan() {
      var foldersToScan = new List<FolderToScan>();
      if (System.IO.File.Exists(pathToJsonFile)) {
        using (var stream = new StreamReader(pathToJsonFile)) {
          while (!stream.EndOfStream) {
            string json = stream.ReadLine();
            if (!string.IsNullOrWhiteSpace(json)) {
              try {
                foldersToScan.Add(JsonSerializer.DeserializeFromString<FolderToScan>(json));
              } catch (System.Exception ex) {
                if (Logger != null) Logger.ErrorException("Error while getting folders to scan", ex);
              }
            }
          }
        }
      }
      return foldersToScan;
    }

    public override FolderToScan Add(FolderToScan folderToScan) {
      folderToScan.FolderToScanId = folderToScanIndex++;
      StringBuilder jsonString = new StringBuilder();
      jsonString.AppendLine(JsonSerializer.SerializeToString<FolderToScan>(folderToScan));
      System.IO.File.AppendAllText(pathToJsonFile, jsonString.ToString());
      return folderToScan;
    }

    public override IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan) {
      StringBuilder jsonString = new StringBuilder();
      foreach (FolderToScan folderToScan in foldersToScan) {
        folderToScan.FolderToScanId = folderToScanIndex++;
        jsonString.AppendLine(JsonSerializer.SerializeToString<FolderToScan>(folderToScan));
      }
      System.IO.File.AppendAllText(pathToJsonFile, jsonString.ToString());
      return foldersToScan;
    }

    public override void Remove(FolderToScan folderToScan) {
      List<FolderToScan> existingFoldersToScan = GetFoldersToScan().ToList();
      existingFoldersToScan.RemoveAll(f => f.AbsolutePath.ToLower() == folderToScan.AbsolutePath.ToLower());
      if (existingFoldersToScan.Count > 0) {
        StringBuilder jsonString = new StringBuilder();
        foreach (FolderToScan folder in existingFoldersToScan) {
          jsonString.AppendLine(JsonSerializer.SerializeToString<FolderToScan>(folder));
        }
        System.IO.File.WriteAllText(pathToJsonFile, jsonString.ToString());
      } else {
        System.IO.File.Delete(pathToJsonFile);
        folderToScanIndex = 0;
      }
    }
    
    public override string Name {
      get { return "ServiceStackJsonFolderToScanRepository"; }
    }

    public override string Version {
      get { return "1.0"; }
    }
  }
}
