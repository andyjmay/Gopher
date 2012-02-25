using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gopher.Core.Models;

namespace Gopher.Core.Data.Memory {
  [Export(typeof(IFileRepository))]
  public class InMemoryFileRepository : IFileRepository {
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

    public File GetById(int fileId) {
      return files.Single(f => f.FileId == fileId);
    }

    public IEnumerable<File> GetAllFiles() {
      return files;
    }

    public IEnumerable<File> GetFilesInFolder(int folderId) {
      return files.Where(f => f.FolderId == folderId);
    }

    public File Add(File fileToAdd) {
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

    public IEnumerable<File> Add(IEnumerable<File> filesToAdd) {
      foreach (var file in filesToAdd) {
        Add(file);
      }
      return filesToAdd;
    }

    public void Clear() {
      files.Clear();
      fileIndex = 1;
      numberOfFilesInCurrentBatch = 0;
    }
  }
}
