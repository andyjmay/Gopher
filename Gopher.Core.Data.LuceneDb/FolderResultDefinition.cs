using Gopher.Core.Models;
using Lucene.Net.Documents;
using SimpleLucene;

namespace Gopher.Core.Data.LuceneDb {
  internal class FolderResultDefinition : IResultDefinition<Folder> {
    public Folder Convert(Document document) {
      var folder = new Folder {
        FolderId = document.GetValue<int>("folderid"),
        ParentFolderId = document.GetValue<int>("parentfolderid"),
        Name = document.GetValue<string>("name"),
        Path = document.GetValue<string>("path")
      };
      return folder;
    }
  }
}
