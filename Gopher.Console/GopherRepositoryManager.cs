using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gopher.Core.Data;
using Gopher.Core.Logging;

namespace Gopher.Console {
  [Export(typeof(IGopherRepositoryManager))]
  internal class GopherRepositoryManager : IGopherRepositoryManager {
    [ImportMany(AllowRecomposition = true)]
    public IEnumerable<FolderToScanRepositoryBase> FolderToScanRepositories { get; set; }

    [ImportMany(AllowRecomposition = true)]
    public IEnumerable<FileRepositoryBase> FileRepositories { get; set; }

    [ImportMany(AllowRecomposition = true)]
    public IEnumerable<FolderRepositoryBase> FolderRepositories { get; set; }

    [Import(AllowDefault = true)]
    public ILogger Logger { get; set; }
  }
}
