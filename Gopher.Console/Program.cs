using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Autofac;
using Gopher.Core.Logging;

namespace Gopher.Console {
  public class Program {
    const int ConsoleInputBufferSize = 8192;
    
    public IContainer Container { get; set; }

    [Import]
    public ILogger Logger { get; set; }

    [Import]
    private IGopherRepositoryManager gopherRepositoryManager { get; set; }

    public static void Main(string[] args) {
      var program = new Program();
      if (program.compose()) {
        if (args.Length == 0) {
          program.RunInHost();
        } else {
          program.Run(args);
        }
      } else {
        System.Console.WriteLine("Unable to compose");
      }
    }

    private bool compose() {
      try {        
        var catalog = new AggregateCatalog();                
        catalog.Catalogs.Add(new DirectoryCatalog(@".\plugins"));
        catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
        var container = new CompositionContainer(catalog);
        container.ComposeParts(this);

        var builder = new ContainerBuilder();        
        builder.Register((c, p) => new JsonGopherConfigReader(gopherRepositoryManager)).As<IConfigReader>();
        Container = builder.Build(Autofac.Builder.ContainerBuildOptions.Default);        

        return true;
      } catch (CompositionException ex) {
        if (Logger != null) {
          Logger.ErrorException("Unable to compose", ex);
        }
        System.Console.WriteLine(ex);
        return false;
      } catch (Exception ex) {
        if (Logger != null) {
          Logger.ErrorException("Unable to compose", ex);
        }
        System.Console.WriteLine(ex);
        return false;
      }
    }

    public void RunInHost() {            
      System.Console.SetIn(new StreamReader(System.Console.OpenStandardInput(ConsoleInputBufferSize)));
      var gopherHost = new GopherHost(System.Console.In, System.Console.Out, gopherRepositoryManager, Container.Resolve<IConfigReader>(), Logger);      
      gopherHost.Run();
    }

    public void Run(string[] args) {
      var gopherHost = new GopherHost(System.Console.In, System.Console.Out, gopherRepositoryManager, Container.Resolve<IConfigReader>(), Logger);      
      gopherHost.RunCommand(args);
    }
  }
}
