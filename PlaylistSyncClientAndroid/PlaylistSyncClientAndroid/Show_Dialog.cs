﻿using Android.App;
using System.Threading.Tasks;

public class Show_Dialog
{
	public enum MessageResult
	{
		NONE = 0,
		OK = 1,
		CANCEL = 2,
		ABORT = 3,
		RETRY = 4,
		IGNORE = 5,
		YES = 6,
		NO = 7
	}

	Activity mcontext;
	public Show_Dialog(Activity activity) : base()
	{
		this.mcontext = activity;
		Xamarin.Forms.Forms.Init(activity, new Android.OS.Bundle());
	}



	public Task<MessageResult> ShowDialog(string Title, string Message, bool SetCancelable = false, bool SetInverseBackgroundForced = false, MessageResult PositiveButton = MessageResult.OK, MessageResult NegativeButton = MessageResult.NONE, MessageResult NeutralButton = MessageResult.NONE, int IconAttribute = Android.Resource.Attribute.AlertDialogIcon)
	{
		var tcs = new TaskCompletionSource<MessageResult>();

		var builder = new AlertDialog.Builder(mcontext);
		builder.SetIconAttribute(IconAttribute);
		builder.SetTitle(Title);
		builder.SetMessage(Message);
		builder.SetInverseBackgroundForced(SetInverseBackgroundForced);
		builder.SetCancelable(SetCancelable);

		builder.SetPositiveButton((PositiveButton != MessageResult.NONE) ? PositiveButton.ToString() : string.Empty, (senderAlert, args) =>
		{
			tcs.SetResult(PositiveButton);
		});
		builder.SetNegativeButton((NegativeButton != MessageResult.NONE) ? NegativeButton.ToString() : string.Empty, delegate
		{
			tcs.SetResult(NegativeButton);
		});
		builder.SetNeutralButton((NeutralButton != MessageResult.NONE) ? NeutralButton.ToString() : string.Empty, delegate
		{
			tcs.SetResult(NeutralButton);
		});

		Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
		{
		});

		Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
		{
			builder.Show();
		});


		// builder.Show();
		return tcs.Task;
	}
}