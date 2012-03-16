using System;
using System.ComponentModel.Composition;
using Gopher.Core.Logging;

namespace Gopher.Console {
  [Export(typeof(ILogger))]
  public class ConsoleLogger : ILogger {
    #region ILogger Members

    public void Trace(string message) {
      //System.Console.ForegroundColor = ConsoleColor.DarkGray;
      //System.Console.WriteLine(message);
      //System.Console.ResetColor();
    }

    public void Debug(string message) {
      //System.Console.ForegroundColor = ConsoleColor.Gray;
      //System.Console.WriteLine(message);
      //System.Console.ResetColor();
    }

    public void Info(string message) {
      System.Console.ForegroundColor = ConsoleColor.White;
      System.Console.WriteLine(message);
      System.Console.ResetColor();
    }

    public void Warn(string message) {
      System.Console.ForegroundColor = ConsoleColor.Yellow;
      System.Console.WriteLine(message);
      System.Console.ResetColor();
    }

    public void ErrorException(string message, Exception ex) {
      System.Console.ForegroundColor = ConsoleColor.DarkRed;
      System.Console.WriteLine(message);
      System.Console.ResetColor();
    }

    public void FatalException(string message, Exception ex) {
      System.Console.ForegroundColor = ConsoleColor.Red;
      System.Console.WriteLine(message);
      System.Console.ResetColor();
    }

    #endregion
  }
}
