using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Gopher.Core.Data;
using Gopher.Core.Logging;

namespace Gopher.Console {
  public class Program {
    const int ConsoleInputBufferSize = 8192;

    [Import]
    public ILogger Logger { get; set; } 

    [Import]
    public IFolderToScanRepository FolderToScanRepository { get; set; }

    [Import]
    public IFileRepository FileRepository { get; set; }

    [Import]
    public IFolderRepository FolderRepository { get; set; }

    public static int Main(string[] args) {
      var program = new Program();
      if (program.compose()) {
        return program.Run();
      } else {
        System.Console.WriteLine("Unable to compose");
        return (int)CommandReturnCodes.Fail;
      }
    }

    private bool compose() {
      try {        
        var catalog = new AggregateCatalog();                
        catalog.Catalogs.Add(new DirectoryCatalog(@".\plugins"));
        catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));        
        var container = new CompositionContainer(catalog);
        container.ComposeParts(this);

        if (this.Logger == null || this.FolderToScanRepository == null || this.FileRepository == null || this.FolderRepository == null) {
          return false;
        }
        return true;
      } catch (CompositionException ex) {
        if (this.Logger != null) {
          this.Logger.ErrorException("Unable to compose", ex);
        }
        System.Console.WriteLine(ex);
        return false;
      } catch (Exception ex) {
        if (this.Logger != null) {
          this.Logger.ErrorException("Unable to compose", ex);
        }
        return false;
      }
    }

    public int Run() {
      System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(ConsoleInputBufferSize)));
      var context = new GopherHostContext {
        FileRepository = FileRepository,
        FolderRepository = FolderRepository,
        FolderToScanRepository = FolderToScanRepository,
        Logger = Logger
      };
      return (int) new GopherHost(System.Console.In, System.Console.Out, context).Run();

      //Logger.Info("Beginning to dig...");       
      //Stopwatch stopwatch = new Stopwatch();
      //stopwatch.Start();
      //foreach (FolderToScan folder in FolderToScanRepository.GetFoldersToScan()) {
      //  Scanner scanner = new Scanner(FileRepository, FolderRepository, Logger);
      //  scanner.ScanFolder(new System.IO.DirectoryInfo(folder.AbsolutePath));
      //  System.Console.WriteLine(string.Format("Took {0} milliseconds to scan {1} (Alias: {2})", stopwatch.ElapsedMilliseconds, folder.AbsolutePath, folder.PathAlias));  
      //}
      //System.Console.WriteLine(string.Format("Total run: {0} milliseconds", stopwatch.ElapsedMilliseconds));
      //stopwatch.Stop();
    }
  }
}
