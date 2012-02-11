using System;

namespace Gopher.Core.Logging {
  public interface ILogger {
    /// <summary>
    /// Most detailed log
    /// </summary>
    /// <param name="message">The message to log</param>
    void Trace(string message);

    /// <summary>
    /// Debugging information, less detailed than Trace
    /// </summary>
    /// <param name="message">The message to log</param>
    void Debug(string message);

    /// <summary>
    /// Informational message, not critical
    /// </summary>
    /// <param name="message">The message to log</param>
    void Info(string message);

    /// <summary>
    /// Warning message, can be recovered or corrected
    /// </summary>
    /// <param name="message">The message to log</param>
    void Warn(string message);

    /// <summary>
    /// Error that does not crash the application
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="ex">Exception causing the error</param>
    void ErrorException(string message, Exception ex);

    /// <summary>
    /// Epic failure
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="ex">Exception causing the failure</param>
    void FatalException(string message, Exception ex);
  }
}
