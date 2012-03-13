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
  [Export(typeof(IFolderRepository))]
  public class LuceneDbFolderRepository : IFolderRepository {
    private readonly System.IO.DirectoryInfo indexDirectory;
    private readonly Analyzer analyzer;
    private DirectoryIndexWriter directoryIndexWriter;
    private int folderIndex = 1;

    public LuceneDbFolderRepository(string indexPath, Analyzer analyzer, bool useExistingIndex = false) {
      this.indexDirectory = new System.IO.DirectoryInfo(indexPath);
      this.analyzer = analyzer;
      if (useExistingIndex) {
        IEnumerable<Folder> existingFolders = GetFolders();
        if (existingFolders.Any()) {
          folderIndex = existingFolders.OrderByDescending(f => f.FolderId).First().FolderId + 1;
        }
      }
      directoryIndexWriter = new DirectoryIndexWriter(indexDirectory, analyzer, recreateIndex: !useExistingIndex);
    }

    public Folder GetById(int folderId) {
      var indexSearcher = getSearcher();
      Folder folderInIndex = null;
      using (var searchService = new SearchService(indexSearcher)) {
        var results = searchService.SearchIndex(new TermQuery(new Term("folderid", folderId.ToString())), new FolderResultDefinition());
        folderInIndex = results.Results.SingleOrDefault();
      }
      return folderInIndex;
    }

    public Folder Add(Folder folderToAdd) {
      folderToAdd.FolderId = folderIndex++;
      using (var indexService = new IndexService(directoryIndexWriter)) {
        IndexResult indexResult = indexService.IndexEntity(folderToAdd, new FolderIndexDefinition());
      }
      return folderToAdd;
    }

    public IEnumerable<Folder> GetFolders() {
      var indexSearcher = getSearcher();
      var folders = new List<Folder>();
      using (var searchService = new SearchService(indexSearcher)) {
        var results = searchService.SearchIndex(new TermQuery(new Term("folderid", "*")), new FolderResultDefinition());
        folders.AddRange(results.Results);
      }
      return folders;
    }

    public void Clear() {
      directoryIndexWriter = new DirectoryIndexWriter(indexDirectory, recreateIndex: true);
      directoryIndexWriter.Create();
      folderIndex = 1;
    }

    private DirectoryIndexSearcher getSearcher() {
      return new DirectoryIndexSearcher(indexDirectory, readOnly: true);
    }
  }
}
