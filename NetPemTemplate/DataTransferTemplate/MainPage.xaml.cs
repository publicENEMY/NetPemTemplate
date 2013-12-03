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
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
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

		private void SendStreamingResultsToJs(object sender, RoutedEventArgs e)
		{
			var r = new Random();
			//rebuffering count
			int buffer = r.Next(0, 10);
			//frame drops
			int frame = r.Next(0, 10);
			//estimate bandwidth
			double bandwidth = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessStreamingResults", buffer, frame, bandwidth);
		}

		private void SendDownloadResultsToJs(object sender, RoutedEventArgs e)
		{
			var r = new Random();
			//estimate bandwidth
			double rate = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessDownloadResults", rate);
		}

		private void SendUploadResultsToJs(object sender, RoutedEventArgs e)
		{
			var r = new Random();
			//estimate bandwidth
			double rate = r.Next(0, 10) + r.NextDouble();

			HtmlPage.Window.Invoke("ProcessUploadResults", rate);
		}

	}
}
