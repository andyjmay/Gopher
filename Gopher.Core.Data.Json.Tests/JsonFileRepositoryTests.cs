using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Gopher.Core.Logging;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Json.Tests {
  public class JsonFileRepositoryTests {
    private string sampleJsonFile = System.Environment.CurrentDirectory + "\\SampleJsonFileRepository.json";

    public JsonFileRepositoryTests() {
      if (System.IO.File.Exists(sampleJsonFile)) {
        System.IO.File.Delete(sampleJsonFile);
      }
    }

    [Fact]
    public void GetById_ShouldThrowException() {
      ILogger logger = new InMemoryLogger();
      var jsonFileRepository = new JsonFileRepository(sampleJsonFile, logger);
      Assert.Throws(typeof(NotImplementedException), () => jsonFileRepository.GetById(1));
    }

    [Fact]
    public void GetAllFiles_ShouldGetAllFiles() {
      ILogger logger = new InMemoryLogger();
      var jsonFileRepository = new JsonFileRepository(sampleJsonFile, logger);
      int numberOfFilesToAdd = 10;

      for (int i = 0; i < numberOfFilesToAdd; i++) {
        jsonFileRepository.Add(new File() {
          CreatedTime = DateTime.Now.AddDays(-1),
          FolderId = 0,
          IsReadOnly = true,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "Test" + i + ".txt",
          Path = System.Environment.CurrentDirectory,
          Size = 1024
        });
      }

      var allFiles = jsonFileRepository.GetAllFiles();
      Assert.Equal(numberOfFilesToAdd, allFiles.Count());
    }

    [Fact]
    public void GetFilesInFolder_ShouldThrowException() {
      ILogger logger = new InMemoryLogger();
      var jsonFileRepository = new JsonFileRepository(sampleJsonFile, logger);
      Assert.Throws(typeof(NotImplementedException), () => jsonFileRepository.GetFilesInFolder(1));
    }

    [Fact]
    public void AddFile_AddsFile() {
      ILogger logger = new InMemoryLogger();
      var jsonFileRepository = new JsonFileRepository(sampleJsonFile, logger);
      jsonFileRepository.Add(new File() {
        CreatedTime = DateTime.Now.AddDays(-1),
        FolderId = 0,
        IsReadOnly = true,
        LastAccessTime = DateTime.Now,
        LastWriteTime = DateTime.Now,
        Name = "Test.txt",
        Path = System.Environment.CurrentDirectory,
        Size = 1024
      });
      File file = jsonFileRepository.GetAllFiles().Single();
      Assert.Equal("Test.txt", file.Name);
    }

    [Fact]
    public void AddFiles_AddsFiles() {
      ILogger logger = new InMemoryLogger();
      var jsonFileRespository = new JsonFileRepository(sampleJsonFile, logger);
      List<File> files = new List<File>();
      for (int i = 0; i < 10; i++) {
        files.Add(new File() {
          CreatedTime = DateTime.Now.AddDays(-1),
          FolderId = 0,
          IsReadOnly = true,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "Test" + i + ".txt",
          Path = System.Environment.CurrentDirectory,
          Size = 1024
        });
      }
      jsonFileRespository.Add(files);
      Assert.Equal(files.Count, jsonFileRespository.GetAllFiles().Count());
    }
  }
}
