using System.Collections.Generic;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  public interface IFolderToScanRepository {
    FolderToScan GetById(int folderToScanId);

    IEnumerable<FolderToScan> GetFoldersToScan();

    FolderToScan Add(FolderToScan folderToScan);

    IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan);
  }
}
