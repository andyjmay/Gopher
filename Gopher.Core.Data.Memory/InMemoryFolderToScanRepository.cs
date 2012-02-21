using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory {
  [Export(typeof(IFolderToScanRepository))]
  public class InMemoryFolderToScanRepository : IFolderToScanRepository {
    private List<FolderToScan> foldersToScan;
    private int folderToScanIndex;

    public InMemoryFolderToScanRepository(int folderToScanIndex = 1) {
      foldersToScan = new List<FolderToScan>();
    }

    #region IFolderToScanRepository Members

    public FolderToScan GetById(int folderToScanId) {
      return foldersToScan.SingleOrDefault(f => f.FolderToScanId == folderToScanIndex);
    }

    public IEnumerable<FolderToScan> GetFoldersToScan() {
      return foldersToScan;
    }

    public FolderToScan Add(FolderToScan folderToScan) {
      folderToScan.FolderToScanId = folderToScanIndex++;
      foldersToScan.Add(folderToScan);
      return folderToScan;
    }

    public IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan) {
      foreach (FolderToScan folder in foldersToScan) {
        Add(folder);
      }
      return foldersToScan;
    }

    #endregion
  }
}
