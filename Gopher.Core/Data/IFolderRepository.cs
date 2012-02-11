using Gopher.Core.Models;
using System.Collections.Generic;

namespace Gopher.Core.Data {
  public interface IFolderRepository {
    Folder GetById(int projectFolderId);

    Folder Add(Folder projectFolderToAdd);

    IEnumerable<Folder> GetProjectFolders();
  }
}
