using Gopher.Core.Models;
using System.Collections.Generic;

namespace Gopher.Core.Data {
  public interface IFolderRepository {
    Folder GetById(int folderId);

    Folder Add(Folder folderToAdd);

    IEnumerable<Folder> GetFolders();

    void Clear();
  }
}
