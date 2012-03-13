using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Gopher.Core.Models;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Search;
using SimpleLucene;
using SimpleLucene.Impl;

namespace Gopher.Core.Data.LuceneDb {
  [Export(typeof(IFileRepository))]
  public class LuceneDbFileRepository : IFileRepository {
    private readonly System.IO.DirectoryInfo indexDirectory;
    private readonly Analyzer analyzer;
    private DirectoryIndexWriter directoryIndexWriter;
    private int fileIndex = 1;

    public LuceneDbFileRepository(string indexPath, Analyzer analyzer, bool useExistingIndex = false) {
      this.indexDirectory = new System.IO.DirectoryInfo(indexPath);
      this.analyzer = analyzer;
      if (useExistingIndex) {
        IEnumerable<File> existingFiles = GetAllFiles();
        if (existingFiles.Any()) {
          fileIndex = existingFiles.OrderByDescending(f => f.FileId).First().FileId + 1;
        }
      }
      directoryIndexWriter = new DirectoryIndexWriter(indexDirectory, analyzer, recreateIndex: !useExistingIndex);
    }

    public File GetById(int projectFileId) {
      var indexSearcher = getSearcher();
      File fileInIndex = null;
      using (var searchService = new SearchService(indexSearcher)) {
        var results = searchService.SearchIndex<File>(new TermQuery(new Term("fileid", projectFileId.ToString())), new FileResultDefinition());
        fileInIndex = results.Results.SingleOrDefault();
      }
      return fileInIndex;
    }

    public IEnumerable<File> GetAllFiles() {
      var indexSearcher = getSearcher();
      var files = new List<File>();
      using (var searchService = new SearchService(indexSearcher)) {
        var results = searchService.SearchIndex(new TermQuery(new Term("fileid", "*")), new FileResultDefinition());
        files.AddRange(results.Results);
      }
      return files;
    }

    public IEnumerable<File> GetFilesInFolder(int folderId) {
      var indexSearcher = getSearcher();
      var files = new List<File>();
      using (var searchService = new SearchService(indexSearcher)) {
        var results = searchService.SearchIndex(new TermQuery(new Term("folderid", folderId.ToString())), new FileResultDefinition());
        files.AddRange(results.Results);
      }
      return files;
    }

    public File Add(File fileToAdd) {
      fileToAdd.FileId = fileIndex++;
      using (var indexService = new IndexService(directoryIndexWriter)) {
        IndexResult indexResult = indexService.IndexEntity(fileToAdd, new FileIndexDefinition());
      }
      return fileToAdd;
    }

    public IEnumerable<File> Add(IEnumerable<File> filesToAdd) {
      var filesToAddToLucene = new List<File>();
      foreach (File file in filesToAdd) {
        file.FileId = fileIndex++;
        filesToAddToLucene.Add(file);
      }
      using (var indexService = new IndexService(directoryIndexWriter)) {
        indexService.IndexEntities(filesToAddToLucene, new FileIndexDefinition());
      }
      return filesToAddToLucene;
    }

    public void Clear() {
      directoryIndexWriter = new DirectoryIndexWriter(indexDirectory, recreateIndex: true);
      directoryIndexWriter.Create();
      fileIndex = 1;
    }

    private DirectoryIndexSearcher getSearcher() {
      return new DirectoryIndexSearcher(indexDirectory, readOnly: true);
    }
  }
}
