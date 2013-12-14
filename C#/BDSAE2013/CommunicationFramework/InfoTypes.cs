using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Utils
{
    /// <summary>
    /// A utiliy class that is used as a lookup for a string format of each movie type
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    public class InfoTypes
    {
        public static Dictionary<int, string> infoTypeTable;

        /// <summary>
        /// Gives a dictionary to look up a string representation of each type id
        /// </summary>
        /// <returns> A Dictionary to look up a string representation of each type id </returns>
        public static Dictionary<int, string> GetTable()
        {
            if(infoTypeTable == null)
                InitializeTable();
            
            return infoTypeTable;
        }

        /// <summary>
        /// Gets the string representation of a type id
        /// </summary>
        /// <returns> The string representation of a specific type id </returns>
        public static string GetTypeString(int id)
        {
            if (infoTypeTable == null)
                InitializeTable();

            return infoTypeTable[id];
        }

        /// <summary>
        /// Initialize the Dictionary
        /// </summary>
        private static void InitializeTable()
        {
            infoTypeTable = new Dictionary<int, string>();

            infoTypeTable.Add(1, "Runtime");
            infoTypeTable.Add(2, "ColorInfo");
            infoTypeTable.Add(3, "Genre");
            infoTypeTable.Add(4, "Language");
            infoTypeTable.Add(5, "Certificates");
            infoTypeTable.Add(6, "SoundMix");
            infoTypeTable.Add(7, "TechInfo");
            infoTypeTable.Add(8, "Countries");
            infoTypeTable.Add(9, "Taglines");
            infoTypeTable.Add(10, "Keywords");
            infoTypeTable.Add(11, "AlternateVersion");
            infoTypeTable.Add(12, "CrazyCredits");
            infoTypeTable.Add(13, "Goofs");
            infoTypeTable.Add(14, "Soundtrack");
            infoTypeTable.Add(15, "Quotes");
            infoTypeTable.Add(16, "ReleaseDates");
            infoTypeTable.Add(17, "Trivia");
            infoTypeTable.Add(18, "Locations");
            infoTypeTable.Add(19, "MiniBiography");
            infoTypeTable.Add(20, "BirthNotes");
            infoTypeTable.Add(21, "BirthDate");
            infoTypeTable.Add(22, "Height");
            infoTypeTable.Add(23, "DeathDate");
            infoTypeTable.Add(24, "Spouse");
            infoTypeTable.Add(25, "OtherWorks");
            infoTypeTable.Add(26, "BirthName");
            infoTypeTable.Add(27, "SalaryHistory");
            infoTypeTable.Add(28, "NickNames");
            infoTypeTable.Add(29, "Books");
            infoTypeTable.Add(30, "AgentAddress");
            infoTypeTable.Add(31, "BiographicalMovies");
            infoTypeTable.Add(32, "PortrayedIn");
            infoTypeTable.Add(33, "WhereNow");
            infoTypeTable.Add(34, "Trademark");
            infoTypeTable.Add(35, "Interviews");
            infoTypeTable.Add(36, "Article");
            infoTypeTable.Add(37, "MagazineCoverPhoto");
            infoTypeTable.Add(38, "Pictorial");
            infoTypeTable.Add(39, "DeathNotes");
            infoTypeTable.Add(40, "DiscFormat");
            infoTypeTable.Add(41, "Year");
            infoTypeTable.Add(42, "DigitalSound");
            infoTypeTable.Add(43, "OfficialRetailPrice");
            infoTypeTable.Add(44, "FrequencyResponse");
            infoTypeTable.Add(45, "PressingPlant");
            infoTypeTable.Add(46, "Length");
            infoTypeTable.Add(47, "LDLanguage");
            infoTypeTable.Add(48, "Review");
            infoTypeTable.Add(49, "Speciality");
            infoTypeTable.Add(50, "ReleaseDate");
            infoTypeTable.Add(51, "ProductionCountry");
            infoTypeTable.Add(52, "Contrast");
            infoTypeTable.Add(53, "ColorRendition");
            infoTypeTable.Add(54, "PictureFormat");
            infoTypeTable.Add(55, "VideoNoise");
            infoTypeTable.Add(56, "VideoArtifacts");
            infoTypeTable.Add(57, "ReleaseCountry");
            infoTypeTable.Add(58, "Sharpness");
            infoTypeTable.Add(59, "DynamicRange");
            infoTypeTable.Add(60, "AudioNoise");
            infoTypeTable.Add(61, "ColorInformation");
            infoTypeTable.Add(62, "GroupGenre");
            infoTypeTable.Add(63, "QualityProgram");
            infoTypeTable.Add(64, "CloseCaptionsTeletext");
            infoTypeTable.Add(65, "category");
            infoTypeTable.Add(66, "AnalogLeft");
            infoTypeTable.Add(67, "AudioQuality");
            infoTypeTable.Add(68, "VideoQuality");
            infoTypeTable.Add(69, "AspectRatio");
            infoTypeTable.Add(71, "AnalogRight");
            infoTypeTable.Add(72, "AdditionalInformation");
            infoTypeTable.Add(73, "NumberOfChapterStops");
            infoTypeTable.Add(74, "DialogueIntelligibility");
            infoTypeTable.Add(75, "DiscSize");
            infoTypeTable.Add(76, "MasterFormat");
            infoTypeTable.Add(77, "Subtitles");
            infoTypeTable.Add(78, "StatusOfAvailability");
            infoTypeTable.Add(79, "QualityOfSource");
            infoTypeTable.Add(80, "NumberOfSides");
            infoTypeTable.Add(81, "VideoStandard");
            infoTypeTable.Add(82, "Supplement");
            infoTypeTable.Add(83, "OriginalTitle");
            infoTypeTable.Add(84, "SoundEncoding");
            infoTypeTable.Add(85, "Number");
            infoTypeTable.Add(86, "Label");
            infoTypeTable.Add(87, "CatalogNumber");
            infoTypeTable.Add(88, "LaserDiscTitle");
            infoTypeTable.Add(89, "ScreenplayTeleplay");
            infoTypeTable.Add(90, "Novel");
            infoTypeTable.Add(91, "Adaption");
            infoTypeTable.Add(92, "Book");
            infoTypeTable.Add(93, "ProductionProcessProtocol");
            infoTypeTable.Add(94, "PrintedMediaReviews");
            infoTypeTable.Add(95, "Essays");
            infoTypeTable.Add(96, "OtherLiterature");
            infoTypeTable.Add(97, "Mpaa");
            infoTypeTable.Add(98, "Plot");
            infoTypeTable.Add(99, "VotesDistribution");
            infoTypeTable.Add(100, "Votes");
            infoTypeTable.Add(101, "Rating");
            infoTypeTable.Add(102, "ProductionDates");
            infoTypeTable.Add(103, "CopyrightHolder");
            infoTypeTable.Add(104, "FilmingDates");
            infoTypeTable.Add(105, "Budget");
            infoTypeTable.Add(106, "WeekendGross");
            infoTypeTable.Add(107, "Gross");
            infoTypeTable.Add(108, "OpeningWeekend");
            infoTypeTable.Add(109, "Rentals");
            infoTypeTable.Add(110, "Admissions");
            infoTypeTable.Add(111, "Studios");
            infoTypeTable.Add(112, "Top250Rank");
            infoTypeTable.Add(113, "Bottom10Rank");
        }
    }
}
