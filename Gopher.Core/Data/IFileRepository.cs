using System.Collections.Generic;
using Gopher.Core.Models;

namespace Gopher.Core.Data {
  public interface IFileRepository {
    File GetById(int projectFileId);

    IEnumerable<File> GetAllFiles();

    IEnumerable<File> GetFilesInFolder(int folderId);

    File Add(File fileToAdd);

    IEnumerable<File> Add(IEnumerable<File> filesToAdd);

    void Clear();
  }
}
