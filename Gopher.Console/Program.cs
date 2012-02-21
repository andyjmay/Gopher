using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Gopher.Core.Data;
using Gopher.Core.Logging;
using Gopher.Core.Models;

namespace Gopher.Console {
  public class Program {
    [Import]
    public ILogger Logger { get; set; } 

    [Import]
    public IFolderToScanRepository FolderToScanRepository { get; set; }

    [Import]
    public IFileRepository FileRepository { get; set; }

    [Import]
    public IFolderRepository FolderRepository { get; set; }

    public static void Main(string[] args) {
      var program = new Program();
      if (program.compose()) {
        program.Run();
      } else {
        System.Console.WriteLine("Unable to compose");
      }
    }

    private bool compose() {
      try {        
        AggregateCatalog catalog = new AggregateCatalog();                
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

    public void Run() {
      Logger.Info("Beginning to dig...");

      foreach (FolderToScan folder in FolderToScanRepository.GetFoldersToScan()) {
        
      }
    }
  }
}
