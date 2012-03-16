using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gopher.Core.Infrastructure;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  [Export(typeof(IFileRepository))]
  public abstract class FileRepositoryBase : IFileRepository, IRepositoryMetadata {
    #region Implementation of IFileRepository
    public abstract File GetById(int projectFileId);
    public abstract IEnumerable<File> GetAllFiles();
    public abstract IEnumerable<File> GetFilesInFolder(int folderId);
    public abstract File Add(File fileToAdd);
    public abstract IEnumerable<File> Add(IEnumerable<File> filesToAdd);
    public abstract void Clear();
    #endregion

    #region Implementation of IRepositoryMetadata
    public abstract string Name { get; }
    public abstract string Version { get; }
    #endregion
  }
}
