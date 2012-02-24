using Gopher.Core.Data;
using Gopher.Core.Logging;

namespace Gopher.Console {
  class GopherHostContext {
    public ILogger Logger { get; set; }
    public IFolderToScanRepository FolderToScanRepository { get; set; }
    public IFolderRepository FolderRepository { get; set; }
    public IFileRepository FileRepository { get; set; }
  }
}
