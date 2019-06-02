using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace ExtendWebBrowser
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object o, RoutedEventArgs routedEventArgs)
        {
            Browser.Navigating += WebBrowOnNavigating;
            Browser.Navigated += WebBrowOnNavigated;
            Browser.NewWindow3 += WebBrowNewWindow;
        }
        private void OnUnloaded(object o, RoutedEventArgs routedEventArgs)
        {
            Browser.Navigating -= WebBrowOnNavigating;
            Browser.Navigated -= WebBrowOnNavigated;
            Browser.NewWindow3 -= WebBrowNewWindow;
            Browser.NavigateError += WebBrowOnNavigateError;
        }
        private void WebBrowOnNavigating(object o, WebBrowserNavigatingEventArgs args)
        {
            if (args.Url != null)
            {
                Console.WriteLine("WebBrowOnNavigating...");
                var arg = Browser.GetUriCookieContainer(Browser.Url);

                var cookies = arg.GetCookies(Browser.Url);
                foreach (var item in cookies)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        private void WebBrowOnNavigated(object o, WebBrowserNavigatedEventArgs args)
        {
            if (args.Url != null)
            {
                Console.WriteLine("WebBrowOnNavigated...");
                var arg = Browser.GetUriCookieContainer(Browser.Url);

                var cookies = arg.GetCookies(Browser.Url);
                foreach (var item in cookies)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }

        private void WebBrowOnNavigateError(object o, WebBrowserNavigateErrorEventArgs arg)
        {
            Console.WriteLine("WebBrowOnNavigateError...");
        }

        private void WebBrowNewWindow(object sender, NewWindow3EventArgs e)
        {
            MessageBox.Show("new window");
        }
    }
}
