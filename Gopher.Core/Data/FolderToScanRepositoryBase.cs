using System.Collections.Generic;
using Gopher.Core.Infrastructure;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  public abstract class FolderToScanRepositoryBase : IFolderToScanRepository, IRepositoryMetadata {
    #region Implementation of IFolderToScanRepository
    public abstract FolderToScan GetById(int folderToScanId);
    public abstract IEnumerable<FolderToScan> GetFoldersToScan();
    public abstract FolderToScan Add(FolderToScan folderToScan);
    public abstract IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan);
    public abstract void Remove(FolderToScan folderToScan);
    #endregion

    #region Implementation of IRepositoryMetadata
    public abstract string Name { get; }
    public abstract string Version { get; }
    #endregion
  }
}
