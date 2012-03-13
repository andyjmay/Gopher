using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gopher.Core.Models;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Xunit;

namespace Gopher.Core.Data.LuceneDb.Tests {
  public class LuceneDbFileRepositoryTests {
    private string fileIndex;
    private Analyzer analyzer;

    public LuceneDbFileRepositoryTests() {
      fileIndex = Environment.CurrentDirectory + "\\FileIndex";
      analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
    }

    [Fact]
    public void GetById_ShouldGetFile() {
      IFileRepository luceneFileRepository = new LuceneDbFileRepository(fileIndex, analyzer, false);
      luceneFileRepository.Add(generateFiles(10));
      File fileInIndex = luceneFileRepository.GetById(5);
      Assert.Equal(5, fileInIndex.FileId);
    }

    private List<File> generateFiles(int numberToCreate) {
      var files = new List<File>();
      for (int i = 0; i < numberToCreate; i++) {
        files.Add(new File {
          FileId = i,
          FolderId = 1,
          CreatedTime = DateTime.Now,
          IsReadOnly = false,
          LastAccessTime = DateTime.Now,
          LastWriteTime = DateTime.Now,
          Name = "File" + i,
          Path = Environment.CurrentDirectory + "\\Files\\",
          Size = 1024
        });
      }
      return files;
    }
  }
}
