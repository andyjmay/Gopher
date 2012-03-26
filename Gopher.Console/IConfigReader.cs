using Gopher.Core.Data;
namespace Gopher.Console {
  internal interface IConfigReader {
    void SetFolderToScanRepository(FolderToScanRepositoryBase folderToScanRepository);
    void SetFileRepository(FileRepositoryBase fileRepository);
    void SetFolderRepository(FolderRepositoryBase folderRepository);
    FolderToScanRepositoryBase GetFolderToScanRepository();
    FileRepositoryBase GetFileRepository();
    FolderRepositoryBase GetFolderRepository();
    void Clear();
  }
}
