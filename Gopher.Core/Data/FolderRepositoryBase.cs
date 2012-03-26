using System.Collections.Generic;
using Gopher.Core.Infrastructure;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  public abstract class FolderRepositoryBase : IFolderRepository, IRepositoryMetadata {
    #region IFolderRepository Members
    public abstract Folder GetById(int folderId);
    public abstract Folder Add(Folder folderToAdd);
    public abstract IEnumerable<Folder> GetFolders();
    public abstract void Clear();
    #endregion

    #region IRepositoryMetadata Members
    public abstract string Name { get ; }
    public abstract string Version { get; }
    #endregion
    }
}
