using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory.Tests {
  public class InMemoryFolderRepositoryTests {
    [Fact]
    public void GetById_ShouldGetFolder() {
      IFolderRepository folderRepository = new InMemoryFolderRepository();
      for (int i = 0; i < 10; i++) {
        folderRepository.Add(new Folder {
          Name = "TestFolder" + i,
          ParentFolderId = 0,
          Path = @"\\TestFolder\Path" + i
        });
      }
      Folder folder = folderRepository.GetById(5);
      Assert.Equal("TestFolder4", folder.Name);
    }

    [Fact]
    public void GetFolders_ShouldGetAllFolders() {
      const int numberOfFoldersToCreate = 10;
      IFolderRepository folderRepository = new InMemoryFolderRepository();
      for (int i = 0; i < numberOfFoldersToCreate; i++) {
        folderRepository.Add(new Folder {
          Name = "TestFolder" + i,
          Path = @"\\TestFolder\Path" + i
        });
      }
      Assert.Equal(numberOfFoldersToCreate, folderRepository.GetFolders().Count());
    }
  }
}
