using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gopher.Core.Data;
using Gopher.Core.Logging;

namespace Gopher.Console {
  internal class GopherHostContext {
    private readonly IGopherRepositoryManager gopherRepositoryManager;
    private readonly IConfigReader configReader;

    public GopherHostContext(IGopherRepositoryManager gopherRepositoryManager, IConfigReader configReader) {
      this.gopherRepositoryManager = gopherRepositoryManager;
      this.configReader = configReader;
      this.SelectedFileRepository = configReader.GetFileRepository();
      this.SelectedFolderRepository = configReader.GetFolderRepository();
      this.SelectedFolderToScanRepository = configReader.GetFolderToScanRepository();
    }

    public FolderToScanRepositoryBase SelectedFolderToScanRepository { get; set; }
    public FolderRepositoryBase SelectedFolderRepository { get; set; }
    public FileRepositoryBase SelectedFileRepository { get; set; }    

    public bool SetFolderRepository(string folderRepositoryName) {
      bool foundMatchingRepository = false;
      folderRepositoryName = folderRepositoryName.ToLower();
      foreach (FolderRepositoryBase folderRepository in gopherRepositoryManager.FolderRepositories) {
        if (folderRepository.Name.ToLower() == folderRepositoryName) {
          SelectedFolderRepository = folderRepository;
          configReader.SetFolderRepository(folderRepository);
          foundMatchingRepository = true;
        }
      }
      return foundMatchingRepository;
    }

    public bool SetFileRepository(string fileRepositoryName) {
      bool foundMatchingRepository = false;
      fileRepositoryName = fileRepositoryName.ToLower();
      foreach (FileRepositoryBase fileRepository in gopherRepositoryManager.FileRepositories) {
        if (fileRepository.Name.ToLower() == fileRepositoryName) {
          SelectedFileRepository = fileRepository;
          configReader.SetFileRepository(fileRepository);
          foundMatchingRepository = true;
        }
      }
      return foundMatchingRepository;
    }

    public bool SetFolderToScanRepository(string folderToScanRepositoryName) {
      bool foundMatchingRepository = false;
      folderToScanRepositoryName = folderToScanRepositoryName.ToLower();
      foreach (FolderToScanRepositoryBase folderToScanRepository in gopherRepositoryManager.FolderToScanRepositories) {
        if (folderToScanRepository.Name.ToLower() == folderToScanRepositoryName) {
          SelectedFolderToScanRepository = folderToScanRepository;
          configReader.SetFolderToScanRepository(folderToScanRepository);
          foundMatchingRepository = true;
        }
      }
      return foundMatchingRepository;
    }
  }
}
