using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gopher.Core;
using Gopher.Core.Data;
using Gopher.Core.Data.Memory;
using Gopher.Core.Logging;
using Gopher.Core.Models;

namespace Gopher.Console {
  internal class GopherHost {
    private readonly TextReader input;
    private readonly TextWriter output;
    private readonly GopherHostContext context;
    private readonly ILogger logger;
    private readonly IGopherRepositoryManager gopherRepositoryManager;

    public GopherHost(TextReader input, TextWriter output, IGopherRepositoryManager gopherRepositoryManager, IConfigReader configReader, ILogger logger) {
      this.input = input;
      this.output = output;
      this.gopherRepositoryManager = gopherRepositoryManager;
      this.context = new GopherHostContext(gopherRepositoryManager, configReader);
      this.logger = logger;
    }

    public CommandReturnCodes Run() {
      try {
        return executeRun();
      } catch (Exception ex) {
        output.WriteLine("Error:");
        for (; ex != null; ex = ex.InnerException) {
          output.WriteLine("   {0}", ex.Message);
        }
        return CommandReturnCodes.Fail;
      }
    }

    private CommandReturnCodes executeRun() {
      output.WriteLine("Type \"?\" for help, \"exit\" to exit, \"cls\" to clear screen");
      while (true) {
        string command = readCommand();
        switch (command) {
          case "quit":
          case "q":
          case "exit":
          case "e":
            return 0;
          case "help":
          case "?":
            ShowHelp();
            break;
          case "cls":
            System.Console.Clear();
            break;
          default:
            RunCommand(command.Split(' '));
            break;
        }
      }
    }

    private string readCommand() {
      output.WriteLine();
      output.Write("gopher> ");
      return input.ReadLine();
    }

    public void RunCommand(string[] commands) {
      if (commands.Length == 0) {
        ShowHelp();
        return;
      }
      switch (commands[0]) {
        case "add":
          handleAdd(commands);
          break;
        case "remove":
          handleRemove(commands);
          break;
        case "append":
          handleAppend(commands);
          break;
        case "clear":
          handleClear(commands);
          break;
        case "set":
          handleSet(commands);
          break;
        case "list":
          handleList(commands);
          break;
        default:
          ShowHelp();
          break;
      }
    }

    private void handleAppend(string[] args) {
      if (args.Length < 2 || (args[0] + " " + args[1]).ToLower() != "append index") {
        ShowHelp();
        return;
      }

      if (!RepositoriesAreSet()) {
        return;
      }

      switch (args.Length) {
        case 2:
          if (context.SelectedFolderToScanRepository == null) {
            output.WriteLine("No folder to scan repository has been set yet.");
            return;
          }
          indexFolders();
          break;
        case 4:
          indexFolder(new FolderToScan {
            AbsolutePath = args[2],
            PathAlias = args[3]
          });
          break;
        default:
          ShowHelp();
          break;
      }
    }

    private void handleClear(string[] args) {
      if (args.Length < 2 || (args[0] + " " + args[1]).ToLower() != "clear index") {
        ShowHelp();
        return;
      }
      clearRepositories();
    }

    private void handleSet(string[] args) {
      if (args.Length != 3) {
        ShowHelp();
        return;
      }
      string repositoryType = args[1];
      string repositoryName = args[2];

      switch (repositoryType.ToLower()) {
        case "folderrepository":
          if (context.SetFolderRepository(repositoryName)) {
            output.WriteLine("Successfully set FolderRepository to " + repositoryName);
          } else {
            output.WriteLine("Unable to find a FolderRepository with name " + repositoryName);
            listRepositories();
          }
          break;
        case "filerepository":
          if (context.SetFileRepository(repositoryName)) {
            output.WriteLine("Successfully set FileRepository to " + repositoryName);
          } else {
            output.WriteLine("Unable to find a FileRepository with name " + repositoryName);
            listRepositories();
          }
          break;
        case "foldertoscanrepository":
          if (context.SetFolderToScanRepository(repositoryName)) {
            output.WriteLine("Successfully set FolderToScanRepository to " + repositoryName);
          } else {
            output.WriteLine("Unable to find a FolderToScanRepository with name " + repositoryName);
            listRepositories();
          }
          break;
        default:
          ShowHelp();
          return;
      }
    }

    private void handleAdd(string[] args) {
      if (args.Length < 2) {
        ShowHelp();
        return;
      }

      if (context.SelectedFolderToScanRepository == null) {
        output.WriteLine("You must first set a FolderToScan Repository");
        return;
      }
      
      var folderToScan = new FolderToScan();
      folderToScan.AbsolutePath = args[1];
      
      if (args.Length == 3) {
        folderToScan.PathAlias = args[2];
      } else {
        folderToScan.PathAlias = folderToScan.AbsolutePath;
      }

      IEnumerable<FolderToScan> existingFoldersToScan = context.SelectedFolderToScanRepository.GetFoldersToScan();
      
      if(existingFoldersToScan.Any(f => f.AbsolutePath == folderToScan.AbsolutePath)) {
        output.WriteLine("This folder has already been added to the repository");
        return;
      }

      context.SelectedFolderToScanRepository.Add(folderToScan);
      output.WriteLine("Added {0} to the repository", folderToScan);
    }

    private void handleRemove(string[] args) {
      if (args.Length != 2) {
        ShowHelp();
        return;
      }
      if (context.SelectedFolderToScanRepository == null) {
        output.WriteLine("You must first set a FolderToScan Repository");
        return;
      }
      int folderToScanId;
      if (int.TryParse(args[1], out folderToScanId)) {
        FolderToScan folderToScanToDelete = context.SelectedFolderToScanRepository.GetById(folderToScanId);
        if (folderToScanToDelete == null) {
          output.WriteLine("Could not find any matching folder to scan");
          return;
        }
        context.SelectedFolderToScanRepository.Remove(folderToScanToDelete);
        return;
      }
      IEnumerable<FolderToScan> matches = context.SelectedFolderToScanRepository.GetFoldersToScan().Where(f => f.AbsolutePath.ToLower() == args[1].ToLower());
      if (!matches.Any()) {
        output.WriteLine("Could not find any matching folder to scan");
        return;
      }
      foreach (FolderToScan folderToScan in matches) {
        context.SelectedFolderToScanRepository.Remove(folderToScan);
      }
    }

    private void handleList(string[] args) {
      listFoldersToScan();
      listRepositories();
    }

    private void listFoldersToScan() {
      if (context.SelectedFolderToScanRepository != null) {
        output.WriteLine("Folders to scan:");
        foreach (FolderToScan folderToScan in context.SelectedFolderToScanRepository.GetFoldersToScan()) {
          output.WriteLine(string.Format("   ID:{0} Path:{1} Alias:{2}", folderToScan.FolderToScanId, folderToScan.AbsolutePath, folderToScan.PathAlias));
        }
      }
    }

    private void listRepositories() {
      output.WriteLine("");
      output.Write("Selected File Repository:");
      if (context.SelectedFileRepository != null) {
        output.Write(" " + context.SelectedFileRepository);
      } else {
        output.Write(" Not set");
      }
      output.WriteLine("");
      output.WriteLine("Available File Repositories:");
      foreach (FileRepositoryBase fileRepositoryBase in gopherRepositoryManager.FileRepositories) {
        output.WriteLine(string.Format("   {0} - {1}", fileRepositoryBase.Name, fileRepositoryBase.Version));
      }
      output.WriteLine("");

      output.Write("Selected Folder Repository:");
      if (context.SelectedFolderRepository != null) {
        output.Write(" " + context.SelectedFolderRepository);
      } else {
        output.Write(" Not set");
      }
      output.WriteLine("");
      output.WriteLine("Available Folder Repositories:");
      foreach (FolderRepositoryBase folderRepositoryBase in gopherRepositoryManager.FolderRepositories) {
        output.WriteLine(string.Format("   {0} - {1}", folderRepositoryBase.Name, folderRepositoryBase.Version));
      }
      output.WriteLine("");

      output.Write("Selected Folder to Scan Repository:");
      if (context.SelectedFileRepository != null) {
        output.Write(" " + context.SelectedFolderToScanRepository);
      } else {
        output.Write(" Not set");
      }
      output.WriteLine("");
      output.WriteLine("Available Folders to Scan Repositories:");
      foreach (FolderToScanRepositoryBase folderToScanRepositoryBase in gopherRepositoryManager.FolderToScanRepositories) {
        output.WriteLine(string.Format("   {0} - {1}", folderToScanRepositoryBase.Name, folderToScanRepositoryBase.Version));
      }
      output.WriteLine("");
    }

    private void clearRepositories() {
      if (context.SelectedFileRepository != null) {
        context.SelectedFileRepository.Clear();
        output.WriteLine("File index cleared.");
      }

      if (context.SelectedFolderRepository != null) {
        context.SelectedFolderRepository.Clear();
        output.WriteLine("Folder index cleared.");
      }     
    }

    private void indexFolders() {
      var indexStopwatch = Stopwatch.StartNew();
      indexStopwatch.Start();
      int projectFileIndex = getFileRepositoryIndex();
      int projectFolderIndex = getFolderRepositoryIndex();
      int batchSize = 10000;
      
      foreach(FolderToScan folderToScan in context.SelectedFolderToScanRepository.GetFoldersToScan()) {
        try {
          var inMemoryFileRepository = new InMemoryFileRepository();
          output.WriteLine(string.Format("Now indexing folder {0} (Alias: {1})", folderToScan.AbsolutePath, folderToScan.PathAlias));
          indexFolder(folderToScan);
        } catch (Exception ex) {
          output.WriteLine(ex.Message);
        }
      }
      indexStopwatch.Stop();
      output.WriteLine(string.Format("Took {0} milliseconds total", indexStopwatch.ElapsedMilliseconds));
    }

    private void indexFolder(FolderToScan folderToScan) {
      try {
        var folderStopwatch = Stopwatch.StartNew();
        var scanner = new Scanner(context.SelectedFileRepository, context.SelectedFolderRepository, logger);
        scanner.ScanFolder(new DirectoryInfo(folderToScan.AbsolutePath), folderToScan);
        output.WriteLine(string.Format("Took {0} milliseconds to scan {1} (Alias: {2})", folderStopwatch.ElapsedMilliseconds, folderToScan.AbsolutePath, folderToScan.PathAlias));
        folderStopwatch.Stop();
      } catch (Exception ex) {
         logger.ErrorException("Exception encountered while scanning " + folderToScan.AbsolutePath, ex);
      }
    }

    private int getFileRepositoryIndex() {
      var currentFilesInRepository = context.SelectedFileRepository.GetAllFiles().ToList();
      if (currentFilesInRepository.Any()) {
        return currentFilesInRepository.Count();
      } else {
        return 1;
      }
    }

    private int getFolderRepositoryIndex() {
      var currentFoldersInRepository = context.SelectedFolderRepository.GetFolders().ToList();
      if (currentFoldersInRepository.Any()) {
        return currentFoldersInRepository.Count();
      } else {
        return 1;
      }
    }

    public void ShowHelp() {
      output.WriteLine("The following are the available actions:");
      output.WriteLine("");
      output.WriteLine("list");
      output.WriteLine("   Lists selected and available repositories");
      output.WriteLine("");
      output.WriteLine("set [FolderRepository|FileRepository|FolderToScanRepository] [RepositoryName]");
      output.WriteLine("   Sets the selected repository");
      output.WriteLine("");
      output.WriteLine("add [actual path] [path alias]");
      output.WriteLine("   Adds a new folder to scan, with an optional alias");
      output.WriteLine("");
      output.WriteLine("remove [actual path|folder ID]");
      output.WriteLine("   Removes the specified folder to scan");
      output.WriteLine("");
      output.WriteLine("clear index");
      output.WriteLine("   Clears the file and folder index.");
      output.WriteLine("");
      output.WriteLine("append index [actual path] [path alias]");
      output.WriteLine("   Creates a new index");
      output.WriteLine("   If no actual path and path alias are supplied, the"); 
      output.WriteLine("   folders to scan will be loaded from the repository.");
      output.WriteLine("");
      output.WriteLine("");
      listFoldersToScan();
      output.WriteLine("");
      output.WriteLine("Current indexes to use:");
      if (context.SelectedFileRepository != null) {
        output.WriteLine("   " + context.SelectedFileRepository.GetType());
      }
      if (context.SelectedFolderToScanRepository != null) {
        output.WriteLine("   " + context.SelectedFolderRepository.GetType());
      }
    }

    public bool RepositoriesAreSet() {
      bool repositoriesAreSet = true;

      if (context.SelectedFileRepository == null) {
        output.WriteLine("No file repository has been set yet.");
        repositoriesAreSet = false;
      }

      if (context.SelectedFolderRepository == null) {
        output.WriteLine("No folder repository has been set yet.");
        repositoriesAreSet = false;
      }

      return repositoriesAreSet;
    }
  }
}
