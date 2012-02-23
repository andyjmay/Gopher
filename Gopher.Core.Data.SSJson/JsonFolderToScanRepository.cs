using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Gopher.Core.Data.SSJson.Properties;
using Gopher.Core.Models;
using ServiceStack.Text;

namespace Gopher.Core.Data.SSJson {
  [Export(typeof(IFolderToScanRepository))]
  public class JsonFolderToScanRepository : IFolderToScanRepository {
    private string pathToJsonFile;
    private int folderToScanIndex;

    public JsonFolderToScanRepository() : this(Settings.Default.FolderToScanRepositoryPath) { }

    public JsonFolderToScanRepository(string pathToJsonFile) {
      this.pathToJsonFile = pathToJsonFile;
      if (!System.IO.File.Exists(pathToJsonFile)) {
        System.IO.File.CreateText(pathToJsonFile).Close();
        folderToScanIndex = 1;
      } else {
        var currentFolders = GetFoldersToScan();
        if (currentFolders.Count() == 0) {
          folderToScanIndex = 1;
        } else {
          folderToScanIndex = currentFolders.OrderByDescending(f => f.FolderToScanId).First().FolderToScanId + 1;
        }
      }
    }

    public FolderToScan GetById(int folderToScanId) {
      return GetFoldersToScan().Single(f => f.FolderToScanId == folderToScanId);
    }

    public IEnumerable<FolderToScan> GetFoldersToScan() {
      var foldersToScan = new List<FolderToScan>();
      using (var stream = new StreamReader(pathToJsonFile)) {
        while (!stream.EndOfStream) {
          var folderToScan = JsonSerializer.DeserializeFromString<FolderToScan>(stream.ReadLine());
          foldersToScan.Add(folderToScan);
        }
      }
      return foldersToScan;
    }

    public FolderToScan Add(FolderToScan folderToScan) {
      folderToScan.FolderToScanId = folderToScanIndex++;
      string folderToScanJson = JsonSerializer.SerializeToString(folderToScan);
      using (var stream = new StreamWriter(pathToJsonFile, append: true)) {
        stream.WriteLine(folderToScanJson);
      }      
      return folderToScan;
    }

    public IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan) {
      var jsonString = new StringBuilder();
      foreach (FolderToScan folderToScan in foldersToScan) {
        folderToScan.FolderToScanId = folderToScanIndex++;
        jsonString.AppendLine(JsonSerializer.SerializeToString(foldersToScan));
      }
      using (var stream = new StreamWriter(pathToJsonFile, append: true)) {
        stream.Write(jsonString);
      }
      return foldersToScan;
    }
  }
}
