using System;
using Gopher.Core.Models;
using SimpleLucene;

namespace Gopher.Core.Data.LuceneDb {
  internal class FileResultDefinition : IResultDefinition<File>{
    public File Convert(Lucene.Net.Documents.Document document) {
      var file = new File {
        FileId = document.GetValue<int>("fileid"),
        FolderId = document.GetValue<int>("folderid"),
        Name = document.GetValue<string>("name"),
        Size = document.GetValue<long>("size"),
        Path = document.GetValue<string>("path"),
        IsReadOnly = document.GetValue<bool>("isreadonly"),
        CreatedTime = document.GetValue<DateTime>("createdtime"),
        LastAccessTime = document.GetValue<DateTime>("lastaccesstime"),
        LastWriteTime = document.GetValue<DateTime>("lastwritetime")
      };
      return file;
    }
  }
}
