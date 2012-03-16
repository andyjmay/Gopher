using Gopher.Core.Models;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SimpleLucene;

namespace Gopher.Core.Data.LuceneDb {
  internal class FolderIndexDefinition : IIndexDefinition<Folder> {
    public Document Convert(Folder folder) {
      var doc = new Document();
      doc.Add(new Field("folderid", folder.FolderId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      if (folder.ParentFolderId != null) {
        doc.Add(new Field("parentfolderid", folder.ParentFolderId.Value.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      } else {
        doc.Add(new Field("parentfolderid", "", Field.Store.YES, Field.Index.NOT_ANALYZED));
      }
      doc.Add(new Field("name", folder.Name, Field.Store.YES, Field.Index.ANALYZED));
      doc.Add(new Field("path", folder.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
      return doc;
    }

    public Term GetIndex(Folder folder) {
      return new Term("folderid", folder.FolderId.ToString());
    }
  }
}
