using Gopher.Core.Models;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SimpleLucene;


namespace Gopher.Core.Data.LuceneDb {
  internal class FileIndexDefinition : IIndexDefinition<File> {
    public Document Convert(File entity) {
      var doc = new Document();
      doc.Add(new Field("fileid", entity.FileId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("folderid", entity.FolderId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("name", entity.Name, Field.Store.YES, Field.Index.ANALYZED));
      doc.Add(new Field("size", entity.Size.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("path", entity.Name, Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("isreadonly", entity.IsReadOnly.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("createdtime", entity.CreatedTime.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("lastaccesstime", entity.LastAccessTime.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      doc.Add(new Field("lastwritetime", entity.LastWriteTime.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
      return doc;
    }

    public Term GetIndex(File entity) {
      return new Term("fileid", entity.FileId.ToString());
    }
  }
}
