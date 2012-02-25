using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Gopher.Core;
using Gopher.Core.Models;

namespace Gopher.Console {
  class GopherHost {
    private readonly TextReader input;
    private readonly TextWriter output;
    private readonly GopherHostContext context;

    public GopherHost(TextReader input, TextWriter output, GopherHostContext context) {
      this.input = input;
      this.output = output;
      this.context = context;
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
            showHelp();
            break;
          case "cls":
            System.Console.Clear();
            break;
          default:
            runCommand(command);
            break;
        }
      }
    }

    private string readCommand() {
      output.WriteLine();
      output.Write("gopher> ");
      return input.ReadLine();
    }

    private void runCommand(string command) {
      if (string.IsNullOrWhiteSpace(command)) {
        return;
      }
      command = command.ToLowerInvariant();
      string[] commands = command.Split(' ');

      switch (commands.Length) {
        case 2:
          switch (command) {
            case "append index":
              indexFolders();
              break;
            case "clear index":
              clearRepositories();
              indexFolders();
              break;
            default:
              showHelp();
              break;
          }
          break;
        case 4:
          if (commands[0] == "clear") {
            clearRepositories();
          }
          indexFolder(new FolderToScan {
            AbsolutePath = commands[2],
            PathAlias = commands[3]
          });
          break;
        default:
          showHelp();
          break;
      }
    }

    private void clearRepositories() {
      context.FileRepository.Clear();
      context.FolderRepository.Clear();
      System.Console.WriteLine("Index cleared.");
    }

    private void indexFolders() {
      var indexStopwatch = Stopwatch.StartNew();
      indexStopwatch.Start();
      foreach(FolderToScan folderToScan in context.FolderToScanRepository.GetFoldersToScan()) {
        indexFolder(folderToScan);
      }
      indexStopwatch.Stop();
      System.Console.WriteLine(string.Format("Took {0} milliseconds total", indexStopwatch.ElapsedMilliseconds));
    }

    private void indexFolder(FolderToScan folderToScan) {
      try {
        var folderStopwatch = Stopwatch.StartNew();
        var scanner = new Scanner(context.FileRepository, context.FolderRepository, context.Logger);
        scanner.ScanFolder(new DirectoryInfo(folderToScan.AbsolutePath), folderToScan);
        System.Console.WriteLine(string.Format("Took {0} milliseconds to scan {1} (Alias: {2})", folderStopwatch.ElapsedMilliseconds, folderToScan.AbsolutePath, folderToScan.PathAlias));
        folderStopwatch.Stop();
      } catch (Exception ex) {
        context.Logger.ErrorException("Exception encountered while scanning " + folderToScan.AbsolutePath, ex);
      }
    }

    private void showHelp() {
      output.WriteLine("The following are the available actions:");
      output.WriteLine("");
      output.WriteLine("[append|clear] index [actual path] [path alias]");
      output.WriteLine("   Creates a new index");
      output.WriteLine("   If no actual path and path alias are supplied, the"); 
      output.WriteLine("   folders to scan will be loaded from the repository.");
      output.WriteLine("");
    }

  }
}
