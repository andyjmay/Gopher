using System.ComponentModel.Composition;
using System.IO;
using Gopher.Core.Data;
using ServiceStack.Text;

namespace Gopher.Console {  
  internal class JsonGopherConfigReader : IConfigReader {
    private readonly FileInfo configFile;
    private readonly IGopherRepositoryManager gopherRepositoryManager;
    
    public JsonGopherConfigReader(IGopherRepositoryManager gopherRepositoryManager)
      : this(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config", gopherRepositoryManager) {
    }

    public JsonGopherConfigReader(string pathToConfigFile, IGopherRepositoryManager gopherRepositoryManager) {
      configFile = new FileInfo(pathToConfigFile);
      this.gopherRepositoryManager = gopherRepositoryManager;
    }

    public JsonGopherConfigReader(FileInfo configFileInfo, IGopherRepositoryManager gopherRepositoryManager) {
      this.configFile = configFileInfo;
      this.gopherRepositoryManager = gopherRepositoryManager;
    }

    #region Implementation of IConfigReader
    public void SetFolderToScanRepository(FolderToScanRepositoryBase folderToScanRepository) {
      GopherConfig config = getConfig();
      config.SelectedFolderToScanRepositoryName = folderToScanRepository.Name;
      update(config);
    }

    public void SetFileRepository(FileRepositoryBase fileRepository) {
      GopherConfig config = getConfig();
      config.SelectedFileRepositoryName = fileRepository.Name;
      update(config);
    }

    public void SetFolderRepository(FolderRepositoryBase folderRepository) {
      GopherConfig config = getConfig();
      config.SelectedFolderRepositoryName = folderRepository.Name;
      update(config);
    }

    public FolderToScanRepositoryBase GetFolderToScanRepository() {
      FolderToScanRepositoryBase selectedFolderToScanRepository = null;
      GopherConfig config = getConfig();
      foreach (FolderToScanRepositoryBase folderToScanRepository in gopherRepositoryManager.FolderToScanRepositories) {
        if (folderToScanRepository.Name == config.SelectedFolderToScanRepositoryName) {
          selectedFolderToScanRepository = folderToScanRepository;
        }
      }
      return selectedFolderToScanRepository;
    }

    public FileRepositoryBase GetFileRepository() {
      FileRepositoryBase selectedFileRepository = null;
      GopherConfig config = getConfig();
      foreach (FileRepositoryBase FileRepository in gopherRepositoryManager.FileRepositories) {
        if (FileRepository.Name == config.SelectedFileRepositoryName) {
          selectedFileRepository = FileRepository;
        }
      }
      return selectedFileRepository;
    }

    public FolderRepositoryBase GetFolderRepository() {
      FolderRepositoryBase selectedFolderRepository = null;
      GopherConfig config = getConfig();
      foreach (FolderRepositoryBase FolderRepository in gopherRepositoryManager.FolderRepositories) {
        if (FolderRepository.Name == config.SelectedFolderRepositoryName) {
          selectedFolderRepository = FolderRepository;
        }
      }
      return selectedFolderRepository;
    }

    public void Clear() {
      var blankConfig = new GopherConfig();
      update(blankConfig);
    }

    #endregion

    private GopherConfig getConfig() {
      GopherConfig gopherConfig;

      if (!configFile.Exists) {
        FileStream fs = File.Create(configFile.FullName);
        fs.Close();
      }

      using (var stream = configFile.OpenText()) {
        string jsonText = stream.ReadToEnd();
        gopherConfig = JsonSerializer.DeserializeFromString<GopherConfig>(jsonText);
        stream.Close();
      }

      if (gopherConfig == null) {
        gopherConfig = new GopherConfig {
          SelectedFileRepositoryName = "",
          SelectedFolderRepositoryName = "",
          SelectedFolderToScanRepositoryName = ""
        };
        update(gopherConfig);
      }

      return gopherConfig;
    }

    private void update(GopherConfig gopherConfig) {
      using (var writer = new StreamWriter(configFile.OpenWrite())) {
        JsonSerializer.SerializeToWriter<GopherConfig>(gopherConfig, writer);
        writer.Close();
      }
    }
  }
}
