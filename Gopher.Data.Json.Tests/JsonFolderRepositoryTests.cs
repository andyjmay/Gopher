using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Gopher.Core.Logging;
using Gopher.Core.Data.Json.Repositories;
using Gopher.Core.Data;
using Gopher.Core.Models;

namespace Gopher.Data.Json.Tests {
  public class JsonFolderRepositoryTests {
    private string sampleJsonFile = System.Environment.CurrentDirectory + "\\SampleJsonFolderRepository.json";

    public JsonFolderRepositoryTests() {
      if (System.IO.File.Exists(sampleJsonFile)) {
        System.IO.File.Delete(sampleJsonFile);
      }
    }

    [Fact]
    public void GetById_ShouldThrowException() {
      ILogger logger = new InMemoryLogger();
      var jsonFolderRepository = new JsonFolderRepository(sampleJsonFile, logger);
      Assert.Throws(typeof(NotImplementedException), () => jsonFolderRepository.GetById(1));
    }

    [Fact]
    public void GetFolders_ShouldGetFolders() {
      ILogger logger = new InMemoryLogger();
      var jsonFolderRepository = new JsonFolderRepository(sampleJsonFile, logger);
      int numberOfFolders = 10;
      addFoldersToRepository(jsonFolderRepository, numberOfFolders);
      Assert.Equal(numberOfFolders, jsonFolderRepository.GetFolders().Count());
    }

    [Fact]
    public void Add_AddsFolders() {
      ILogger logger = new InMemoryLogger();
      var jsonFolderRepository = new JsonFolderRepository(sampleJsonFile, logger);
      int numberOfFolders = 10;
      Assert.Equal(0, jsonFolderRepository.GetFolders().Count());
      addFoldersToRepository(jsonFolderRepository, numberOfFolders);
      Assert.Equal(numberOfFolders, jsonFolderRepository.GetFolders().Count());
    }

    private void addFoldersToRepository(IFolderRepository folderRepository, int numberOfFoldersToAdd) {
      for (int i = 0; i < numberOfFoldersToAdd; i++) {
        folderRepository.Add(new Folder() {
          Name = "Test Folder " + i,
          ParentFolderId = 0,
          Path = System.Environment.CurrentDirectory
        });
      }
    }
  }
}
