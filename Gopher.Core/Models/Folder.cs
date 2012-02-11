namespace Gopher.Core.Models {
  public partial class Folder {
    /// <summary>
    /// Gets or sets the ID of the folder
    /// </summary>
    public int FolderId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent folder
    /// </summary>
    public int? ParentFolderId { get; set; }

    /// <summary>
    /// Gets or sets the name of the folder
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the full path to the folder
    /// </summary>
    public string Path { get; set; }    
  }
}
