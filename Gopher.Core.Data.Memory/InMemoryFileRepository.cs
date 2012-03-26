using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory {
  [Export(typeof(FileRepositoryBase))]
  public class InMemoryFileRepository : FileRepositoryBase {
    private List<File> files = new List<File>();
    private int fileIndex;
    private int batchSize = 0;
    private int numberOfFilesInCurrentBatch = 0;
    private Action<IEnumerable<File>> reachedBatchSize;

    public InMemoryFileRepository(int fileIndex = 1) {
      this.fileIndex = fileIndex;
    }

    public InMemoryFileRepository(int fileIndex, int batchSize, Action<IEnumerable<File>> reachedBatchSize) {
      this.fileIndex = fileIndex;
      this.batchSize = batchSize;
      this.reachedBatchSize = reachedBatchSize;
    }

    public override File GetById(int fileId) {
      return files.Single(f => f.FileId == fileId);
    }

    public override IEnumerable<File> GetAllFiles() {
      return files;
    }

    public override IEnumerable<File> GetFilesInFolder(int folderId) {
      return files.Where(f => f.FolderId == folderId);
    }

    public override File Add(File fileToAdd) {
      fileToAdd.FileId = fileIndex++;
      files.Add(fileToAdd);
      if (reachedBatchSize != null) {
        numberOfFilesInCurrentBatch++;
        if (batchSize != 0 && numberOfFilesInCurrentBatch >= batchSize) {
          reachedBatchSize(files);
          numberOfFilesInCurrentBatch = 0;
          files.Clear();
        }
      }
      return fileToAdd;
    }

    public override IEnumerable<File> Add(IEnumerable<File> filesToAdd) {
      foreach (var file in filesToAdd) {
        Add(file);
      }
      return filesToAdd;
    }

    public override void Clear() {
      files.Clear();
      fileIndex = 1;
      numberOfFilesInCurrentBatch = 0;
    }

    public override string Name {
      get { return "InMemoryFileRepository"; }
    }

    public override string Version {
      get { return "1.0"; }
    }
  }
}
