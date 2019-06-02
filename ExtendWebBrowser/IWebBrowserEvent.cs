using System;
using System.ComponentModel;

namespace ExtendWebBrowser
{
    public class WebBrowserNavigateErrorEventArgs : EventArgs
    {
        private String urlValue;
        private String frameValue;
        private Int32 statusCodeValue;
        private Boolean cancelValue;

        public WebBrowserNavigateErrorEventArgs(
            String url, String frame, Int32 statusCode, Boolean cancel)
        {
            urlValue = url;
            frameValue = frame;
            statusCodeValue = statusCode;
            cancelValue = cancel;
        }

        public String Url
        {
            get { return urlValue; }
            set { urlValue = value; }
        }

        public String Frame
        {
            get { return frameValue; }
            set { frameValue = value; }
        }

        public Int32 StatusCode
        {
            get { return statusCodeValue; }
            set { statusCodeValue = value; }
        }

        public Boolean Cancel
        {
            get { return cancelValue; }
            set { cancelValue = value; }
        }
    }

    public class NewWindow2EventArgs : CancelEventArgs
    {

        object ppDisp;

        public object PPDisp
        {
            get { return ppDisp; }
            set { ppDisp = value; }
        }


        public NewWindow2EventArgs(ref object ppDisp, ref bool cancel)
            : base()
        {
            this.ppDisp = ppDisp;
            this.Cancel = cancel;
        }
    }

    public class NewWindow3EventArgs : CancelEventArgs
    {

        object ppDisp;

        public object PPDisp
        {
            get { return ppDisp; }
            set { ppDisp = value; }
        }
        public string BstUrl { get; set; }
        public string BstrUrlContext { get; set; }
        public NewWindow3EventArgs(ref object ppDisp, ref bool cancel, string bstUrl, string bstrUrlContext)
            : base()
        {
            this.ppDisp = ppDisp;
            this.Cancel = cancel;
            BstUrl = bstUrl;
            BstrUrlContext = bstrUrlContext;
        }
    }

    public class DocumentCompleteEventArgs : EventArgs
    {
        private object ppDisp;
        private object url;

        public object PPDisp
        {
            get { return ppDisp; }
            set { ppDisp = value; }
        }

        public object Url
        {
            get { return url; }
            set { url = value; }
        }

        public DocumentCompleteEventArgs(object ppDisp, object url)
        {
            this.ppDisp = ppDisp;
            this.url = url;

        }
    }

    public class CommandStateChangeEventArgs : EventArgs
    {
        private long command;
        private bool enable;
        public CommandStateChangeEventArgs(long command, ref bool enable)
        {
            this.command = command;
            this.enable = enable;
        }

        public long Command
        {
            get { return command; }
            set { command = value; }
        }

        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }
    }

    public class WindowClosingEventArgs : EventArgs
    {
        private bool _isChildWindow;
        private bool _cancel;

        public WindowClosingEventArgs(bool isChildWindow, bool cancel)
        {
            _isChildWindow = isChildWindow;
            _cancel = cancel;
        }

        public bool IsChildWindow
        {
            get { return _isChildWindow; }
            set
            {
                if (value != _isChildWindow)
                    _isChildWindow = value;
            }
        }

        public bool Cancel
        {
            get { return _cancel; }
            set
            {
                if (value != _cancel)
                    _cancel = value;
            }
        }
    }

    interface IWebBrowserEvent
    {
        event EventHandler<WebBrowserNavigateErrorEventArgs> NavigateError;
        event EventHandler<NewWindow2EventArgs> NewWindow2;
        event EventHandler<NewWindow3EventArgs> NewWindow3;
        event EventHandler<WindowClosingEventArgs> WindowClosing;
        event EventHandler<DocumentCompleteEventArgs> DocumentComplete;
        event EventHandler<CommandStateChangeEventArgs> CommandStateChange;
    }
}
