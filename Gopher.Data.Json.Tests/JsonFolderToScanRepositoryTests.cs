using System.Collections.Generic;
using System.Linq;
using Gopher.Core.Data.Json;
using Gopher.Core.Models;
using Xunit;

namespace Gopher.Data.Json.Tests {
  public class JsonFolderToScanRepositoryTests {
    private string sampleJsonFile = System.Environment.CurrentDirectory + "\\SampleJsonFolderToScanRepository.json";

    public JsonFolderToScanRepositoryTests() {
      if (System.IO.File.Exists(sampleJsonFile)) {
        System.IO.File.Delete(sampleJsonFile);
      }
    }

    [Fact]
    public void GetFolderById_GetsFolderById() {
      System.IO.File.Delete(sampleJsonFile);
      JsonFolderToScanRepository jsonFolderToScanRepository = new JsonFolderToScanRepository(sampleJsonFile);
      for (int i = 0; i < 10; i++) {
        FolderToScan folderToScan = new FolderToScan {
          AbsolutePath = System.Environment.CurrentDirectory,
          PathAlias = "TestFolder" + i
        };
        jsonFolderToScanRepository.Add(folderToScan);
      }
      IEnumerable<FolderToScan> foldersToScan = jsonFolderToScanRepository.GetFoldersToScan();
      FolderToScan folder = foldersToScan.ToArray()[4];
      Assert.True(folder.PathAlias == "TestFolder4");
    }

    [Fact]
    public void GetFoldersToScan_GetsSerializesFoldersToScan() {
      System.IO.File.Delete(sampleJsonFile);
      JsonFolderToScanRepository jsonFolderToScanRepository = new JsonFolderToScanRepository(sampleJsonFile);
      for (int i = 0; i < 10; i++) {
        FolderToScan folderToScan = new FolderToScan {
          AbsolutePath = System.Environment.CurrentDirectory,
          PathAlias = "TestFolder" + i
        };
        jsonFolderToScanRepository.Add(folderToScan);
      }
      IEnumerable<FolderToScan> foldersToScan = jsonFolderToScanRepository.GetFoldersToScan();
      Assert.True(foldersToScan.Count() == 10);
    }

    [Fact]
    public void AddFolderToScan_ShouldAddFolder() {
      System.IO.File.Delete(sampleJsonFile);
      JsonFolderToScanRepository jsonFolderToScanRepository = new JsonFolderToScanRepository(sampleJsonFile);
      FolderToScan folderToScan = new FolderToScan {
        AbsolutePath = System.Environment.CurrentDirectory,
        PathAlias = "TestFolder"
      };
      jsonFolderToScanRepository.Add(folderToScan);
      Assert.True(jsonFolderToScanRepository.GetFoldersToScan().Count() == 1);
    }

    [Fact]
    public void AddFolderToScan_ShouldAddFolderAndSetID() {
      System.IO.File.Delete(sampleJsonFile);
      JsonFolderToScanRepository jsonFolderToScanRepository = new JsonFolderToScanRepository(sampleJsonFile);
      FolderToScan folderToScan = new FolderToScan {
        AbsolutePath = System.Environment.CurrentDirectory,
        PathAlias = "TestFolder"
      };
      FolderToScan addedFolder = jsonFolderToScanRepository.Add(folderToScan);
      Assert.True(jsonFolderToScanRepository.GetFoldersToScan().Single().FolderToScanId == 1);
    }

    [Fact]
    public void AddFoldersToScan_ShouldAddFolders() {
      int numberOfFoldersToAddToTheRepository = 10;
      List<FolderToScan> foldersToAdd = new List<FolderToScan>();
      for (int i = 0; i < numberOfFoldersToAddToTheRepository; i++) {
        FolderToScan folderToScan = new FolderToScan {
          AbsolutePath = System.Environment.CurrentDirectory,
          PathAlias = "TestFolder" + i
        };
        foldersToAdd.Add(folderToScan);
      }
      System.IO.File.Delete(sampleJsonFile);
      JsonFolderToScanRepository jsonFolderToScanRepository = new JsonFolderToScanRepository(sampleJsonFile);
      jsonFolderToScanRepository.Add(foldersToAdd);
      Assert.True(foldersToAdd.Count == numberOfFoldersToAddToTheRepository);
    }
  }
}
