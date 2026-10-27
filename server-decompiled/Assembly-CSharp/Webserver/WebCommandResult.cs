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
	// Token: 0x02001B22 RID: 6946
	public class WebCommandResult : IConsoleConnection
	{
		// Token: 0x0600D0E8 RID: 53480 RVA: 0x004BB0F8 File Offset: 0x004B92F8
		public WebCommandResult(string _command, string _parameters, WebCommandResult.ResultType _responseType, RequestContext _context)
		{
			this.context = _context;
			this.command = _command;
			this.parameters = _parameters;
			this.responseType = _responseType;
			WebConnection connection = _context.Connection;
			this.sourceName = ((connection != null) ? connection.Username : null) ?? ("Unauth-PermLevel-" + _context.PermissionLevel.ToString());
		}

		// Token: 0x0600D0E9 RID: 53481 RVA: 0x004BB160 File Offset: 0x004B9360
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

		// Token: 0x0600D0EA RID: 53482 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLine(string _text)
		{
		}

		// Token: 0x0600D0EB RID: 53483 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600D0EC RID: 53484 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableLogLevel(LogType _type, bool _enable)
		{
		}

		// Token: 0x0600D0ED RID: 53485 RVA: 0x004BB338 File Offset: 0x004B9538
		public string GetDescription()
		{
			return "WebCommandResult_for_" + this.command + "_by_" + this.sourceName;
		}

		// Token: 0x04009E23 RID: 40483
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string command;

		// Token: 0x04009E24 RID: 40484
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string parameters;

		// Token: 0x04009E25 RID: 40485
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string sourceName;

		// Token: 0x04009E26 RID: 40486
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly RequestContext context;

		// Token: 0x04009E27 RID: 40487
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WebCommandResult.ResultType responseType;

		// Token: 0x04009E28 RID: 40488
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("resultRaw");

		// Token: 0x04009E29 RID: 40489
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009E2A RID: 40490
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonParametersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parameters");

		// Token: 0x04009E2B RID: 40491
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonResultKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("result");

		// Token: 0x02001B23 RID: 6947
		public enum ResultType
		{
			// Token: 0x04009E2D RID: 40493
			Full,
			// Token: 0x04009E2E RID: 40494
			ResultOnly,
			// Token: 0x04009E2F RID: 40495
			Raw
		}
	}
}
