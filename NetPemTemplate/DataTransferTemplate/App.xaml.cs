using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DataTransferTemplate
{
	public partial class App : Application
	{

		public App()
		{
			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;

			InitializeComponent();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			this.RootVisual = new MainPage();

			// HtmlDocument requires using System.Windows.Browser.
			HtmlDocument doc = HtmlPage.Document;

			// Hook up the simple JavaScript method HTML button.
			doc.GetElementById("btnCallJSMethod").AttachEvent("click",
				new EventHandler(this.CallGlobalJSMethod));

			// Hook up the JavaScript method.
			doc.GetElementById("SendStreamingResults").AttachEvent("click",
				new EventHandler(SendStreamingResultsToJs));
			// Hook up the JavaScript method.
			doc.GetElementById("SendDownloadResults").AttachEvent("click",
				new EventHandler(SendDownloadResultsToJs));
			// Hook up the JavaScript method.
			doc.GetElementById("SendUploadResults").AttachEvent("click",
				new EventHandler(SendUploadResultsToJs));
		}

		private void SendStreamingResultsToJs(object sender, EventArgs e)
		{
			var r = new Random();
			//rebuffering count
			int buffer = r.Next(0,10);
			//frame drops
			int frame = r.Next(0, 10);
			//estimate bandwidth
			double bandwidth = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessStreamingResults", buffer, frame, bandwidth);
		}

		private void SendDownloadResultsToJs(object sender, EventArgs e)
		{
			var r = new Random();
			//estimate bandwidth
			double rate = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessDownloadResults", rate);
		}
		
		private void SendUploadResultsToJs(object sender, EventArgs e)
		{
			var r = new Random();
			//estimate bandwidth
			double rate = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessUploadResults", rate);
		}



		// Call a global JavaScript method defined on the HTML page.
		// Pass result to javascript here
		// Results
		// sample_1080p_buffer - int rebuffering count
		// sample_1080p_frame - int total frame lost
		// sample_1080p_bandwidth - megabits?

		// sample_720p_buffer
		// sample_720p_frame
		// sample_720p_bandwidth

		// sample_480p_buffer
		// sample_480p_frame
		// sample_480p_bandwidth

		// sample_360p_buffer
		// sample_360p_frame
		// sample_360p_bandwidth

		// upload download are similar
		// sample_size1_rate - megabits?

		// sample_size2_rate

		// sample_size3_rate


		private void CallGlobalJSMethod(object o, EventArgs e)
		{

			string strMS = DateTime.Now.Millisecond.ToString();

			string strTime = "Time came from managed code \n"
				+ DateTime.Now.ToLongTimeString()
				+ " MS = " + strMS;

			HtmlPage.Window.Invoke("globalJSMethod", strTime);
		}

		private void Application_Exit(object sender, EventArgs e)
		{

		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			// If the app is running outside of the debugger then report the exception using
			// the browser's exception mechanism. On IE this will display it a yellow alert 
			// icon in the status bar and Firefox will display a script error.
			if (!System.Diagnostics.Debugger.IsAttached)
			{

				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
			}
		}

		private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
		{
			try
			{
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

				System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
			}
			catch (Exception)
			{
			}
		}
	}
}
