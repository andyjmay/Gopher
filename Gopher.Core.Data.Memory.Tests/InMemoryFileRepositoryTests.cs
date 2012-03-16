using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Gopher.Core.Logging;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory.Tests {
  public class InMemoryFileRepositoryTests {
    [Fact]
    public void GetById_ShouldGetFile() {
      IFileRepository fileRepository = new InMemoryFileRepository();
      for (int i = 0; i < 10; i++) {
        fileRepository.Add(new File {
          CreatedTime = DateTime.Now,
          FolderId = 0,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "TestFile" + i,
          Path = @"\\Test\Path" + i,
          Size = 1024
        });
      }
      File file = fileRepository.GetById(5);
      Assert.Equal("TestFile4", file.Name);
    }

    [Fact]
    public void GetAllFiles_ShouldGetAllFiles() {
      const int numberOfFilesToCreate = 10;

      IFileRepository fileRepository = new InMemoryFileRepository();
      for (int i = 0; i < numberOfFilesToCreate; i++) {
        fileRepository.Add(new File {
          CreatedTime = DateTime.Now,
          FolderId = 0,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "TestFile" + i,
          Path = @"\\Test\Path" + i,
          Size = 1024
        });
      }

      Assert.Equal(numberOfFilesToCreate, fileRepository.GetAllFiles().Count());
    }

    [Fact]
    public void GetFilesInFolder_ShouldGetFilesInFolder() {
      const int numberOfFilesToCreate = 30;

      IFileRepository fileRepository = new InMemoryFileRepository();
      for (int i = 0; i < numberOfFilesToCreate; i++) {
        fileRepository.Add(new File {
          CreatedTime = DateTime.Now,
          FolderId = 5,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "TestFile" + i,
          Path = @"\\Test\Path" + i,
          Size = 1024
        });
      }
      Assert.Equal(numberOfFilesToCreate, fileRepository.GetFilesInFolder(5).Count());
    }

    [Fact]
    public void AddFile_AddsFile() {
      IFileRepository fileRepository = new InMemoryFileRepository();
      fileRepository.Add(new File() {
        CreatedTime = DateTime.Now.AddDays(-1),
        FolderId = 0,
        IsReadOnly = true,
        LastAccessTime = DateTime.Now,
        LastWriteTime = DateTime.Now,
        Name = "Test.txt",
        Path = System.Environment.CurrentDirectory,
        Size = 1024
      });
      Assert.Equal(1, fileRepository.GetAllFiles().Count());
      Assert.Equal(1, fileRepository.GetAllFiles().First().FileId);
    }

    [Fact]
    public void AddFile_AddsFiles() {
      const int numberOfFilesToCreate = 30;
      IFileRepository fileRepository = new InMemoryFileRepository();
      for (int i = 0; i < numberOfFilesToCreate; i++) {
        fileRepository.Add(new File() {
          CreatedTime = DateTime.Now.AddDays(-1),
          FolderId = 0,
          IsReadOnly = true,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "Test.txt",
          Path = System.Environment.CurrentDirectory,
          Size = 1024
        });
      }
      Assert.Equal(numberOfFilesToCreate, fileRepository.GetAllFiles().Count());
    }
  }
}