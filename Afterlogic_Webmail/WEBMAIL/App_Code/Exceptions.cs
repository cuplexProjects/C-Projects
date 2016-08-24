using System;
using MailBee;

namespace WebMail
{
	public class WebMailException : ApplicationException
	{
		public WebMailException() {}

        public WebMailException(string message) : base(message) { }

        public WebMailException(string message, Exception inner)
            : base((inner is WebMailException) ? message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}

        public WebMailException(Exception inner) : this((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailDatabaseException : WebMailException
	{
		public WebMailDatabaseException() {}

		public WebMailDatabaseException(string message) : base(message) {}

		public WebMailDatabaseException(string message, Exception inner) : base(message, inner) {}

		public WebMailDatabaseException(Exception inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailIOException : WebMailException
	{
		public WebMailIOException() {}

		public WebMailIOException(string message) : base(message) {}

		public WebMailIOException(string message, Exception inner) : base(message, inner) {}

		public WebMailIOException(Exception inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailMailBeeException : WebMailException
	{
		public WebMailMailBeeException() {}

		public WebMailMailBeeException(string message) : base(message) {}

		public WebMailMailBeeException(string message, MailBeeException inner) : base(message, inner) {}

		public WebMailMailBeeException(MailBeeException inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}


	public class WebMailMailBoxException : WebMailException
	{
		public WebMailMailBoxException() {}

		public WebMailMailBoxException(string message) : base(message) {}

		public WebMailMailBoxException(string message, MailBeeException inner) : base(message, inner) {}

		public WebMailMailBoxException(MailBeeException inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailSettingsException : WebMailException
	{
		public WebMailSettingsException() {}

		public WebMailSettingsException(string message) : base(message) {}

		public WebMailSettingsException(string message, Exception inner) : base(message, inner) {}

		public WebMailSettingsException(Exception inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailWebException : WebMailException
	{
		public WebMailWebException() {}

		public WebMailWebException(string message) : base(message) {}

		public WebMailWebException(string message, Exception inner) : base(message, inner) {}

		public WebMailWebException(Exception inner) : base((inner != null) ? inner.Message : (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("WebMailException"), inner) {}
	}

	public class WebMailSessionException : WebMailWebException
	{
		public WebMailSessionException() {}

		public WebMailSessionException(string message) : base(message) {}

		public WebMailSessionException(string message, Exception inner) : base(message, inner) {}

		public WebMailSessionException(Exception inner) : base(inner) {}
	}

	public class WebMailWMServerException : WebMailException
	{
		public WebMailWMServerException() {}

		public WebMailWMServerException(string message) : base(message) {}

		public WebMailWMServerException(string message, Exception inner) : base(message, inner) {}

		public WebMailWMServerException(Exception inner) : base(inner) {}
	}
}