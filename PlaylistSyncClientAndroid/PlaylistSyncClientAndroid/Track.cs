using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PlaylistSyncClientAndroid
{
	struct Track
	{
		public string Name;
		public string Length;
		public string Url;
		public string NowPlaying;
	}
}