using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FakeIMDB_DesktopClient.Model
{

    /// <summary>
    /// Enum describing the different types of Search Items
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public enum ItemType
    {
        Movie,
        Person,
        Series,
        Episode
    }

    /// <summary>
    /// Interface for SearchItems
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface ISearchItem
    {
        string Id { get; set; }
        ItemType Type { get; set; }
        ImageSource Icon { get; set; }
        string ImageSource { get; set; }
        string ShortDescription { get; set; }
    }
}