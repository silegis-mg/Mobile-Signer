using Almg.MobileSigner.Resources;
using System;
using System.Net;

namespace Almg.MobileSigner.Helpers
{
	public class ExceptionHelper
	{
		public static string GetMessage(string userMessage, Exception ex)
		{
			string message = userMessage;

			if (ex is HttpRequest.HttpException)
			{
				message += " (" + AppResources.EXCEPTION_CODE + ": " + (int)(ex as HttpRequest.HttpException).HttpStatusCode + ")";
			}
			else
			{
				message += " (HR: " + ex.HResult.ToString("X") + ")";
			}

			return message;
		}
	}
}
