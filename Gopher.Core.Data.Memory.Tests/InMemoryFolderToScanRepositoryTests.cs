using Gopher.Core.Models;
using Xunit;

namespace Gopher.Core.Data.Memory.Tests {
  public class InMemoryFolderToScanRepositoryTests {
    [Fact]
    public void GetById_ShouldGetFolderToScan() {
      IFolderToScanRepository folderToScanRepository = new InMemoryFolderToScanRepository();
      for (int i = 0; i < 10; i++) {
        FolderToScan addedFolderToScan = folderToScanRepository.Add(new FolderToScan {
          AbsolutePath = @"C:\Test\Path\" + i,
          PathAlias = @"\\My\Fake\Path\" + i
        });
        Assert.NotNull(addedFolderToScan);
        Assert.Equal(i, addedFolderToScan.FolderToScanId - 1);
      }
      FolderToScan folderToTest = folderToScanRepository.GetById(5);
      Assert.NotNull(folderToTest);
      Assert.Equal(@"\\My\Fake\Path\4", folderToTest.PathAlias);
      Assert.Equal(@"C:\Test\Path\4", folderToTest.AbsolutePath);
    }
  }
}
