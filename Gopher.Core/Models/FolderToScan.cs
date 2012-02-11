using System;

namespace Gopher.Core.Models {
  [Serializable]
  public class FolderToScan {
    /// <summary>
    /// The ID of the folder to scan
    /// </summary>
    public int FolderToScanId { get; set; }

    /// <summary>
    /// Full path to the folder to scan
    /// </summary>
    public string AbsolutePath { get; set; }

    /// <summary>
    /// Optional - Swaps out the Absolute path for an alias. Useful when using UNC paths.
    /// </summary>
    public string PathAlias { get; set; }
  }
}
