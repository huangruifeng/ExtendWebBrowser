using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExtendWebBrowser
{
    public class ExtendedWebBrowser : WebBrowser, IWebBrowserEvent
    {
        AxHost.ConnectionPointCookie cookie;
        WebBrowserExtendedEvents events;
        protected override void CreateSink()
        {
            //MAKE SURE TO CALL THE BASE or the normal events won't fire
            base.CreateSink();
            base.CreateSink();
            events = new WebBrowserExtendedEvents(this);
            cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, events, typeof(DWebBrowserEvents2));
        }

        public object Application
        {
            get
            {
                IWebBrowser2 axWebBrowser = this.ActiveXInstance as IWebBrowser2;
                if (axWebBrowser != null)
                {
                    return axWebBrowser.Application;
                }
                else
                    return null;
            }
        }
        protected override void DetachSink()
        {
            if (null != cookie)
            {
                cookie.Disconnect();
                cookie = null;
            }
            base.DetachSink();
        }

        public void OnNavigateError(WebBrowserNavigateErrorEventArgs e)
        {
            if (NavigateError != null)
            {
                NavigateError(this, e);
            }
        }

        public void OnNewWindow2(ref object ppDisp, ref bool cancel)
        {
            NewWindow2EventArgs args = new NewWindow2EventArgs(ref ppDisp, ref cancel);

            if (null != NewWindow2)
            {
                NewWindow2(this, args);
            }
            //Pass the cancellation chosen back out to the events
            //Pass the ppDisp chosen back out to the events
            cancel = args.Cancel;
            ppDisp = args.PPDisp;
        }

        public void OnNewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext,
            string bstrUrl)
        {
            NewWindow3EventArgs args = new NewWindow3EventArgs(ref ppDisp, ref Cancel, bstrUrl, bstrUrlContext);
            if (null != NewWindow3)
            {
                NewWindow3(this, args);
            }
            Cancel = args.Cancel;
            ppDisp = args.PPDisp;
        }

        public void OnWindowClosing(bool isChildWindow, bool cancel)
        {
            WindowClosingEventArgs arg = new WindowClosingEventArgs(isChildWindow, cancel);
            if (null != WindowClosing)
            {
                WindowClosing(this, arg);
            }
        }

        public void OnDocumentComplete(object ppDisp, object url)
        {
            EventHandler<DocumentCompleteEventArgs> h = DocumentComplete;
            DocumentCompleteEventArgs args = new DocumentCompleteEventArgs(ppDisp, url);
            if (null != DocumentComplete)
            {
                DocumentComplete(this, args);
            }
            //Pass the ppDisp chosen back out to the events
            ppDisp = args.PPDisp;
            //I think url is readonly
        }

        public void OnCommandStateChange(long command, ref bool enable)
        {
            CommandStateChangeEventArgs args = new CommandStateChangeEventArgs(command, ref enable);
            if (null != CommandStateChange)
            {
                CommandStateChange(this, args);
            }
        }
        public event EventHandler<WebBrowserNavigateErrorEventArgs> NavigateError;
        public event EventHandler<NewWindow2EventArgs> NewWindow2;
        public event EventHandler<NewWindow3EventArgs> NewWindow3;
        public event EventHandler<WindowClosingEventArgs> WindowClosing;
        public event EventHandler<DocumentCompleteEventArgs> DocumentComplete;
        public event EventHandler<CommandStateChangeEventArgs> CommandStateChange;
    }

    public static class ExtendedWebBrowserExtension
    {
        enum InternetCookieState
        {
            COOKIE_STATE_UNKNOWN = 0x0,
            COOKIE_STATE_ACCEPT = 0x1,
            COOKIE_STATE_PROMPT = 0x2,
            COOKIE_STATE_LEASH = 0x3,
            COOKIE_STATE_DOWNGRADE = 0x4,
            COOKIE_STATE_REJECT = 0x5,
            COOKIE_STATE_MAX = COOKIE_STATE_REJECT
        }

        /// <summary>
        /// 返回uri的cookie集合
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(this ExtendedWebBrowser browser,Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        /// <summary>
        /// 给uri设置cookie,cookie为"field=value"
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static bool SetUriCookie(this ExtendedWebBrowser browser, string uri, string cookie)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(cookie))
                return false;
            if (!cookie.Contains("="))
            {
                return false;
            }
            InternetSetCookieEx(uri, null, cookie, 0, 0);
            return true;
        }

        public static bool SupressCookiePersist(this ExtendedWebBrowser browser)
        {
            // 3 = INTERNET_SUPPRESS_COOKIE_PERSIST 
            // 81 = INTERNET_OPTION_SUPPRESS_BEHAVIOR
            return SetOption(81, 3);
        }

        public static bool EndBrowserSession(this ExtendedWebBrowser browser)
        {
            // 42 = INTERNET_OPTION_END_BROWSER_SESSION 
            return SetOption(42, null);
        }

        static bool SetOption(int settingCode, int? option)
        {
            IntPtr optionPtr = IntPtr.Zero;
            int size = 0;
            if (option.HasValue)
            {
                size = sizeof(int);
                optionPtr = Marshal.AllocCoTaskMem(size);
                Marshal.WriteInt32(optionPtr, option.Value);
            }

            bool success = InternetSetOption(0, settingCode, optionPtr, size);

            if (optionPtr != IntPtr.Zero) Marshal.FreeCoTaskMem(optionPtr);

            return success;
        }

        [DllImport("wininet.dll")]
        static extern InternetCookieState InternetSetCookieEx(
            string lpszURL,
            string lpszCookieName,
            string lpszCookieData,
            int dwFlags,
            int dwReserved);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(
            int hInternet,
            int dwOption,
            IntPtr lpBuffer,
            int dwBufferLength
        );
        private const Int32 InternetCookieHttponly = 0x2000;
    }
}
