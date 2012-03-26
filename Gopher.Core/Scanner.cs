using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Gopher.Core.Data;
using Gopher.Core.Logging;
using Gopher.Core.Models;

namespace Gopher.Core {
  public class Scanner {
    private readonly IFileRepository fileRepository;
    private readonly IFolderRepository folderRepository;
        
    public ILogger Logger { get; set; }

    public Scanner(IFileRepository fileRepository, IFolderRepository folderRepository, ILogger logger) {
      this.fileRepository = fileRepository;
      this.folderRepository = folderRepository;
      this.Logger = logger;
    }

    public void ScanFolder(System.IO.DirectoryInfo directoryInfo, FolderToScan folderToScan, int? parentFolderId = null) {
      try {
        var folder = new Folder {
          Name = directoryInfo.Name,
          Path = directoryInfo.FullName.Replace(folderToScan.AbsolutePath, folderToScan.PathAlias),
          ParentFolderId = parentFolderId
        };
        folderRepository.Add(folder);        
        Logger.Trace("Added folder " + folder.Path);

        var fileInfos = directoryInfo.EnumerateFiles("*", System.IO.SearchOption.TopDirectoryOnly);
        var files = new List<Models.File>();

        foreach (var fileInfo in fileInfos) {
          try {
            var file = new Models.File {
              Name = fileInfo.Name,
              Path = fileInfo.FullName.Replace(folderToScan.AbsolutePath, folderToScan.PathAlias),
              Size = fileInfo.Length,
              FolderId = folder.FolderId,
              CreatedTime = fileInfo.CreationTime,
              LastAccessTime = fileInfo.LastAccessTime,
              LastWriteTime = fileInfo.LastWriteTime
            };
            files.Add(file);
            Logger.Trace("Added file " + file.Name);
          } catch (System.IO.PathTooLongException ex) {
            // TODO: Handle these (very common) errors
          } catch (Exception ex) {
            Logger.ErrorException("Error scanning file " + fileInfo.FullName, ex);
          }
        }
        fileRepository.Add(files);

        foreach (var subDirectory in directoryInfo.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly)) {
          ScanFolder(subDirectory, folderToScan, parentFolderId = folder.FolderId);
        }
      } catch (System.IO.PathTooLongException ex) {
        // TODO: Handle these (very common) errors
      } catch (Exception ex) {
        Logger.ErrorException("Error scanning directory " + directoryInfo.FullName, ex);
      }
    }

    public void ScanFolder(System.IO.DirectoryInfo directoryInfo, int? parentFolderId=null) {
      try {
        var folder = new Folder {
          Name = directoryInfo.Name,
          Path = directoryInfo.FullName,
          ParentFolderId = parentFolderId
        };
        folderRepository.Add(folder);
        Logger.Trace("Added folder " + folder.Path);

        var fileInfos = directoryInfo.EnumerateFiles("*", System.IO.SearchOption.TopDirectoryOnly);
        var files = new List<Models.File>();

        foreach (var fileInfo in fileInfos) {
          try {
            var file = new Models.File {
              Name = fileInfo.Name,
              Path = fileInfo.FullName,
              Size = fileInfo.Length,
              FolderId = folder.FolderId,
              CreatedTime = fileInfo.CreationTime,
              LastAccessTime = fileInfo.LastAccessTime,
              LastWriteTime = fileInfo.LastWriteTime
            };
            files.Add(file);
            Logger.Trace("Added file " + file.Name);
          } catch (Exception ex) {
            Logger.ErrorException("Error scanning file " + fileInfo.FullName, ex);
          }
        }
        fileRepository.Add(files);

        foreach (var subDirectory in directoryInfo.EnumerateDirectories("*", System.IO.SearchOption.TopDirectoryOnly)) {
          ScanFolder(subDirectory, parentFolderId=folder.FolderId);
        }
      } catch (Exception ex){
        Logger.ErrorException("Error scanning directory " + directoryInfo.FullName, ex);
      }
    }
  }
}
