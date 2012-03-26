using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory {
  [Export(typeof(FolderToScanRepositoryBase))]
  public class InMemoryFolderToScanRepository : FolderToScanRepositoryBase {
    private readonly List<FolderToScan> foldersToScan;
    private int folderToScanIndex;

    public InMemoryFolderToScanRepository(int folderToScanIndex = 1) {
      this.folderToScanIndex = folderToScanIndex;
      foldersToScan = new List<FolderToScan>();
    }

    #region IFolderToScanRepository Members

    public override FolderToScan GetById(int folderToScanId) {
      return foldersToScan.SingleOrDefault(f => f.FolderToScanId == folderToScanId);
    }

    public override IEnumerable<FolderToScan> GetFoldersToScan() {
      return foldersToScan;
    }

    public override FolderToScan Add(FolderToScan folderToScan) {
      folderToScan.FolderToScanId = folderToScanIndex++;
      foldersToScan.Add(folderToScan);
      return folderToScan;
    }

    public override IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan) {
      foreach (FolderToScan folder in foldersToScan) {
        Add(folder);
      }
      return foldersToScan;
    }

    public override void Remove(FolderToScan folderToScan) {
      foldersToScan.RemoveAll(f => f.AbsolutePath.ToLower() == folderToScan.AbsolutePath.ToLower());
    }

    public override string Name {
      get { return "InMemoryFolderToScanRepository"; }
    }

    public override string Version {
      get { return "1.0"; }
    }

    #endregion
  }
}
