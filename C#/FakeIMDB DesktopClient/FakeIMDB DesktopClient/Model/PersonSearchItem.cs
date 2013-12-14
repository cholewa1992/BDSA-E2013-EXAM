using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FakeIMDB_DesktopClient.Model
{
    /// <summary>
    /// Model with properties describing a Person Search Item
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class PersonSearchItem : ISearchItem
    {
        public string Id { get; set; }
        public ItemType Type { get; set; }
        public ImageSource Icon { get; set; }
        public string ImageSource { get; set; }
        public string ShortDescription { get; set; }
        public string Name { get; set; }

        // Extended informations
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string CharacterName { get; set; }
        public string Role { get; set; }
        public List<MovieSearchItem> ParticipatesInList { get; set; }


        public PersonSearchItem()
        {
            Type = ItemType.Person;
        }

    }
}
