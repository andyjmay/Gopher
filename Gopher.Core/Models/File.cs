using System;

namespace Gopher.Core.Models {
  public partial class File {
    /// <summary>
    /// Gets or sets the entity ID of the project file
    /// </summary>
    public int FileId { get; set; }

    /// <summary>
    /// Gets or sets the entity ID of the project folder
    /// </summary>
    public int FolderId { get; set; }

    /// <summary>
    /// Gets or sets the name of the project file
    /// </summary>    
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the project file size, in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Gets or sets the full path to the project file 
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the read-only status of this project file
    /// </summary>
    public bool IsReadOnly { get; set; }

    /// <summary>
    /// Gets or sets the creation time of the current file
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Gets or sets the time the current file was last accessed
    /// </summary>
    public DateTime LastAccessTime { get; set; }

    /// <summary>
    /// Gets or sets the time when the current file was last written to
    /// </summary>
    public DateTime LastWriteTime { get; set; }
  }
}
