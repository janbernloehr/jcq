using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Jcq.Core.Collections;
using Jcq.Core.Contracts.Collections;
using Jcq.IcqProtocol.Contracts;
using Jcq.Ux.ViewModel;

namespace Jcq.Ux.Main.Views
{
    /// <summary>
    ///     Interaction logic for RateLimitsWindow.xaml
    /// </summary>
    public partial class RateLimitsWindow : Window
    {
        public RateLimitsWindow()
        {
            DataContext = new RateLimitsViewModel();

            InitializeComponent();
        }
    }

    public class RateLimitsViewModel
    {
        public RateLimitsViewModel()
        {
            RateLimits = ApplicationService.CurrentContext.GetService<IRateLimitsService>().RateLimits.WrapObservable();

            Debug.WriteLine(RateLimits.Count);
        }

        public ReadOnlyObservableCollection<IRateLimitsClass> RateLimits { get; private set; }
    }

    public static class CollectionExtensions
    {
        public static ReadOnlyObservableCollection<T> WrapObservable<T>(this IReadOnlyNotifyingCollection<T> source)
        {
            var collection = new ObservableCollection<T>(source.ToList());
            var binding = new NotifyingCollectionBinding<T>(source, collection);

            return new ReadOnlyObservableCollection<T>(collection);
        }
    }
}