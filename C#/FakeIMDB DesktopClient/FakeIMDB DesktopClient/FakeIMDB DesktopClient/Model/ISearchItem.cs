using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FakeIMDB_DesktopClient.Model
{

    public enum ItemType
    {
        Movie,
        Person,
        Series,
        Episode
    }

    public interface ISearchItem
    {
        string Id { get; set; }
        ItemType Type { get; set; }
        ImageSource Icon { get; set; }
        string ImageSource { get; set; }
        string ShortDescription { get; set; }
    }
}