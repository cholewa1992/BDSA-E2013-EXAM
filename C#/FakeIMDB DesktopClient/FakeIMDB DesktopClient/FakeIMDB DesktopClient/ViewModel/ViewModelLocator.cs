
using FakeIMDB_DesktopClient.Services;
using FakeIMDB_DesktopClient.Services.Stubs;
using FakeIMDB_DesktopClient.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class ViewModelLocator
    {

        /// <summary>
        /// Constructor of the ViewModelLocator which registers classes with SimpleIoc
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {
                SimpleIoc.Default.Register<ISearchService, SearchService>();
                SimpleIoc.Default.Register<IMovieExtendedInformationService, MovieExtendedInformationService>();
                SimpleIoc.Default.Register<IPersonExtendedInformationService, PersonExtendedInformationService>();
                SimpleIoc.Default.Register<IPutMovieDataService, PutMovieDataService>();
                SimpleIoc.Default.Register<IPutPersonDataService, PutPersonDataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<MovieItemViewModel>();
            SimpleIoc.Default.Register<PersonItemViewModel>();
            SimpleIoc.Default.Register<ConnectionViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the Search property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SearchViewModel Search
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchViewModel>();
            }
        }

        /// <summary>
        /// Gets the Movie property
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MovieItemViewModel Movie
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MovieItemViewModel>();
            }
        }

        /// <summary>
        /// Gets the Person property
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PersonItemViewModel Person
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PersonItemViewModel>();
            }
        }


        /// <summary>
        /// Gets the Connection property
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ConnectionViewModel Connection
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConnectionViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}