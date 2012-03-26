using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory {
  [Export(typeof(FolderRepositoryBase))]
  public class InMemoryFolderRepository : FolderRepositoryBase {
    private readonly List<Folder> folders = new List<Folder>();
    private int folderIndex;
    private int batchSize = 0;
    private int numberOfFoldersInCurrentBatch = 0;
    private readonly Action<IEnumerable<Folder>> reachedBatchSize;

    public InMemoryFolderRepository(int folderIndex = 1) {
      this.folderIndex = folderIndex;
    }

    public InMemoryFolderRepository(int folderIndex, int batchSize, Action<IEnumerable<Folder>> reachedBatchSize) {
      this.folderIndex = folderIndex;
      this.batchSize = batchSize;
      this.reachedBatchSize = reachedBatchSize;
    }

    #region IFolderRepository Members

    public override Folder GetById(int folderId) {
      return folders.Single(p => p.FolderId == folderId);
    }

    public override Folder Add(Folder folderToAdd) {
      folderToAdd.FolderId = folderIndex++;
      folders.Add(folderToAdd);      
      if (reachedBatchSize != null) {
        numberOfFoldersInCurrentBatch++;
        if (batchSize != 0 && numberOfFoldersInCurrentBatch >= batchSize) {
          reachedBatchSize(folders);
          numberOfFoldersInCurrentBatch = 0;
          folders.Clear();
        }
      }
      return folderToAdd;
    }

    public override IEnumerable<Folder> GetFolders() {
      return folders;
    }

    public override void Clear() {
      folders.Clear();
      folderIndex = 1;
      batchSize = 0;
      numberOfFoldersInCurrentBatch = 0;
    }

    public override string Name {
      get { return "InMemoryFolderRepository"; }
    }

    public override string Version {
      get { return "1.0"; }
    }

    #endregion
  }
}
