using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gopher.Core.Infrastructure;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  [Export(typeof(IFolderToScanRepository))]
  public abstract class FolderToScanRepositoryBase : IFolderToScanRepository, IRepositoryMetadata {
    #region Implementation of IFolderToScanRepository
    public abstract FolderToScan GetById(int folderToScanId);
    public abstract IEnumerable<FolderToScan> GetFoldersToScan();
    public abstract FolderToScan Add(FolderToScan folderToScan);
    public abstract IEnumerable<FolderToScan> Add(IEnumerable<FolderToScan> foldersToScan);
    #endregion

    #region Implementation of IRepositoryMetadata
    public abstract string Name { get; }
    public abstract string Version { get; }
    #endregion
  }
}
