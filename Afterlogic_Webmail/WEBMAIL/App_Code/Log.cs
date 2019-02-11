using System;
using System.Diagnostics;
#if DEBUG
using System.Collections;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
#endif

namespace WebMail
{
    /// <summary>
    /// This class provides methods for logging events.
    /// </summary>
    public class Log
    {
        private Log() { }

        public static void Write(string message)
        {
            try
            {
                Trace.Write(message);
            }
            catch (Exception)
            {
            }
        }

        public static void WriteLine(string methodName, string message)
        {
            try
            {
                string output = string.Format("[{2}][{0}] - {1}", methodName, message, DateTime.Now);
                Trace.WriteLine(output);
            }
            catch (Exception)
            {
            }
        }

		public static void WriteLine(string methodName, string format, params object[] list)
		{
			WriteLine(methodName, string.Format(format, list));
		}

        public static void WriteException(Exception ex)
        {
            try
            {
                Trace.WriteLine("------------------------------------------------------------");
                Trace.WriteLine(string.Format("[{0}] ERROR!!!", DateTime.Now));
                Trace.WriteLine(string.Format("[Message]\r\n{0}", ex.Message));
                if (ex.InnerException != null)
                {
                    Trace.WriteLine(string.Format("[TargetSite]\r\n{0}", ex.InnerException.TargetSite));
                    Trace.WriteLine(string.Format("[Stack Trace]\r\n{0}", ex.InnerException.StackTrace));
                }
                else
                {
                    Trace.WriteLine(string.Format("[TargetSite]\r\n{0}", ex.TargetSite));
                    Trace.WriteLine(string.Format("[Stack Trace]\r\n{0}", ex.StackTrace));
                }

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);

                Trace.WriteLine("[Method]: " + trace.GetFrame(0).GetMethod().Name);
                Trace.WriteLine("[Line, Column]: [" + trace.GetFrame(0).GetFileLineNumber() + ", " + trace.GetFrame(0).GetFileColumnNumber() + "]");
                Trace.WriteLine("------------------------------------------------------------");
            }
            catch (Exception)
            {
            }
        }
    }

#if DEBUG

	public class DebugWatch
	{
		private struct DebugWatchInfo
		{
			public long ID;
			public string Operation;
			public Stopwatch Watch;
			public XmlElement OperationElement;
			public XmlElement ParentOperations;

			public DebugWatchInfo(long id, string operation, Stopwatch watch, XmlElement parentOperations)
			{
				ID = id;
				Operation = operation;
				Watch = watch;
				ParentOperations = parentOperations;

				OperationElement = ParentOperations.OwnerDocument.CreateElement("operation");
				OperationElement.AppendChild(ParentOperations.OwnerDocument.CreateElement("operations"));

				parentOperations.AppendChild(OperationElement);
			}

			public void ToXml()
			{
				XmlAttribute idAttr = OperationElement.OwnerDocument.CreateAttribute("id");
				idAttr.Value = ID.ToString(CultureInfo.InvariantCulture);
				OperationElement.Attributes.Append(idAttr);

				XmlAttribute dateAttr = OperationElement.OwnerDocument.CreateAttribute("date");
				dateAttr.Value = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
				OperationElement.Attributes.Append(dateAttr);

				XmlAttribute nameAttr = OperationElement.OwnerDocument.CreateAttribute("name");
				nameAttr.Value = Operation;
				OperationElement.Attributes.Append(nameAttr);

				XmlAttribute ticksAttr = OperationElement.OwnerDocument.CreateAttribute("durationByTicks");
				ticksAttr.Value = Watch.ElapsedTicks.ToString(CultureInfo.InvariantCulture);
				OperationElement.Attributes.Append(ticksAttr);

				XmlAttribute millisecondsAttr = OperationElement.OwnerDocument.CreateAttribute("durationByMilliseconds");
				millisecondsAttr.Value = Watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
				OperationElement.Attributes.Append(millisecondsAttr);
			}
		}

		private static Stack<DebugWatchInfo> _watchStack = new Stack<DebugWatchInfo>();
		private static long _id = 0;
		private static XmlDocument _xmlDoc = null;
		private static XmlElement _operations = null;

		public static long Start(string operation)
		{
			if (_xmlDoc == null)
			{
				_xmlDoc = new XmlDocument();
				_xmlDoc.AppendChild(_xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
				_operations = _xmlDoc.CreateElement("operations");
				_xmlDoc.AppendChild(_operations);
			}
			if (_id == 0) _id = DateTime.Now.ToFileTimeUtc();

			System.Threading.Interlocked.Increment(ref _id);
			DebugWatchInfo dwi = new DebugWatchInfo(_id, operation, Stopwatch.StartNew(), _operations);
			_watchStack.Push(dwi);
			_operations = (XmlElement)dwi.OperationElement.LastChild;

			return _id;
		}

		public static void Stop()
		{
			DebugWatchInfo dwi = _watchStack.Pop();
			_operations = dwi.ParentOperations;
			if (dwi.Watch != null)
			{
				dwi.Watch.Stop();
				WriteTicksToLog(dwi);
			}
		}

		private static void WriteTicksToLog(DebugWatchInfo dwi)
		{
            string filename = ConfigurationManager.AppSettings["DebugLogFile"] as string;
			if (filename != null)
			{
				dwi.ToXml();
				_xmlDoc.Save(filename);
			}
		}
	}

#endif
}
