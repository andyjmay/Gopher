using System;
using System.Collections.Generic;
using Gopher.Core.Data;

namespace Gopher.Console {
  internal interface IGopherRepositoryManager {
    IEnumerable<FileRepositoryBase> FileRepositories { get; set; }
    IEnumerable<FolderRepositoryBase> FolderRepositories { get; set; }
    IEnumerable<FolderToScanRepositoryBase> FolderToScanRepositories { get; set; }
  }
}
