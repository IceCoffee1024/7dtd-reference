using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using UnityEngine;
using Utf8Json;

namespace Webserver
{
	// Token: 0x02001A9E RID: 6814
	public class WebCommandResult : IConsoleConnection
	{
		// Token: 0x0600CCF3 RID: 52467 RVA: 0x004ACD3C File Offset: 0x004AAF3C
		public WebCommandResult(string _command, string _parameters, WebCommandResult.ResultType _responseType, RequestContext _context)
		{
			this.context = _context;
			this.command = _command;
			this.parameters = _parameters;
			this.responseType = _responseType;
			WebConnection connection = _context.Connection;
			this.sourceName = ((connection != null) ? connection.Username : null) ?? ("Unauth-PermLevel-" + _context.PermissionLevel.ToString());
		}

		// Token: 0x0600CCF4 RID: 52468 RVA: 0x004ACDA4 File Offset: 0x004AAFA4
		public void SendLines(List<string> _output)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in _output)
			{
				stringBuilder.AppendLine(text);
			}
			string text2 = stringBuilder.ToString();
			try
			{
				if (this.responseType == WebCommandResult.ResultType.Raw)
				{
					WebUtils.WriteText(this.context.Response, text2, HttpStatusCode.OK, null);
				}
				else
				{
					JsonWriter jsonWriter;
					WebUtils.PrepareEnvelopedResult(out jsonWriter);
					if (this.responseType == WebCommandResult.ResultType.ResultOnly)
					{
						jsonWriter.WriteRaw(WebCommandResult.jsonRawKey);
						jsonWriter.WriteString(text2);
						jsonWriter.WriteEndObject();
					}
					else
					{
						jsonWriter.WriteRaw(WebCommandResult.jsonCommandKey);
						jsonWriter.WriteString(this.command);
						jsonWriter.WriteRaw(WebCommandResult.jsonParametersKey);
						jsonWriter.WriteString(this.parameters);
						jsonWriter.WriteRaw(WebCommandResult.jsonResultKey);
						jsonWriter.WriteString(text2);
						jsonWriter.WriteEndObject();
					}
					WebUtils.SendEnvelopedResult(this.context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				}
			}
			catch (IOException ex)
			{
				if (ex.InnerException is SocketException)
				{
					Log.Warning("[Web] Error in WebCommandResult.SendLines(): Remote host closed connection: " + ex.InnerException.Message);
				}
				else
				{
					Log.Warning(string.Format("[Web] Error (IO) in WebCommandResult.SendLines(): {0}", ex));
				}
			}
			catch (Exception ex2)
			{
				Log.Warning(string.Format("[Web] Error in WebCommandResult.SendLines(): {0}", ex2));
			}
			finally
			{
				RequestContext requestContext = this.context;
				if (requestContext != null)
				{
					SpaceWizards.HttpListener.HttpListenerResponse response = requestContext.Response;
					if (response != null)
					{
						response.Close();
					}
				}
			}
		}

		// Token: 0x0600CCF5 RID: 52469 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLine(string _text)
		{
		}

		// Token: 0x0600CCF6 RID: 52470 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600CCF7 RID: 52471 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableLogLevel(LogType _type, bool _enable)
		{
		}

		// Token: 0x0600CCF8 RID: 52472 RVA: 0x004ACF7C File Offset: 0x004AB17C
		public string GetDescription()
		{
			return "WebCommandResult_for_" + this.command + "_by_" + this.sourceName;
		}

		// Token: 0x04009B43 RID: 39747
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string command;

		// Token: 0x04009B44 RID: 39748
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string parameters;

		// Token: 0x04009B45 RID: 39749
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string sourceName;

		// Token: 0x04009B46 RID: 39750
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly RequestContext context;

		// Token: 0x04009B47 RID: 39751
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WebCommandResult.ResultType responseType;

		// Token: 0x04009B48 RID: 39752
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("resultRaw");

		// Token: 0x04009B49 RID: 39753
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009B4A RID: 39754
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonParametersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parameters");

		// Token: 0x04009B4B RID: 39755
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonResultKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("result");

		// Token: 0x02001A9F RID: 6815
		public enum ResultType
		{
			// Token: 0x04009B4D RID: 39757
			Full,
			// Token: 0x04009B4E RID: 39758
			ResultOnly,
			// Token: 0x04009B4F RID: 39759
			Raw
		}
	}
}
