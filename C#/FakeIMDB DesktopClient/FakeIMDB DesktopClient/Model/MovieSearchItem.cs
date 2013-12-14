using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FakeIMDB_DesktopClient.Model
{
    /// <summary>
    /// Model with properties describing a Movie Search Item
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class MovieSearchItem : ISearchItem
    {
        public string Id { get; set; }
        public ItemType Type { get; set; }
        public ImageSource Icon { get; set; }
        public string ImageSource { get; set; }
        public string ShortDescription { get; set; }
        public string Title { get; set; }


        // Extended informations
        public string Year { get; set; }
        public List<PersonSearchItem> ParticipantsList { get; set; }
        

        public MovieSearchItem()
        {
            Type = ItemType.Movie;

            //Icon = new Bitmap(System.Reflection.Assembly.
            //GetEntryAssembly().GetManifestResourceStream("FakeIMDB_DesktopClient.Icons.movie-icon.png"));
        }
    }
}
