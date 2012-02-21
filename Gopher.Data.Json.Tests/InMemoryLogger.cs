using System;
using System.Collections.Generic;
using Gopher.Core.Logging;

namespace Gopher.Core.Data.Json.Tests {
  public class InMemoryLogger : ILogger {
    private List<string> logMessages;

    public InMemoryLogger() {
      logMessages = new List<string>();
    }

    #region ILogger Members

    public void Trace(string message) {
      logMessages.Add(message);
    }

    public void Debug(string message) {
      logMessages.Add(message);
    }

    public void Info(string message) {
      logMessages.Add(message);
    }

    public void Warn(string message) {
      logMessages.Add(message);
    }

    public void ErrorException(string message, Exception ex) {
      logMessages.Add(message);
    }

    public void FatalException(string message, Exception ex) {
      logMessages.Add(message);
    }

    #endregion
  }
}
