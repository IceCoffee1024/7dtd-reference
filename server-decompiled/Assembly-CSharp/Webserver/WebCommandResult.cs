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
	// Token: 0x02001AF1 RID: 6897
	public class WebCommandResult : IConsoleConnection
	{
		// Token: 0x0600CFD4 RID: 53204 RVA: 0x004B75FC File Offset: 0x004B57FC
		public WebCommandResult(string _command, string _parameters, WebCommandResult.ResultType _responseType, RequestContext _context)
		{
			this.context = _context;
			this.command = _command;
			this.parameters = _parameters;
			this.responseType = _responseType;
			WebConnection connection = _context.Connection;
			this.sourceName = ((connection != null) ? connection.Username : null) ?? ("Unauth-PermLevel-" + _context.PermissionLevel.ToString());
		}

		// Token: 0x0600CFD5 RID: 53205 RVA: 0x004B7664 File Offset: 0x004B5864
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

		// Token: 0x0600CFD6 RID: 53206 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLine(string _text)
		{
		}

		// Token: 0x0600CFD7 RID: 53207 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600CFD8 RID: 53208 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableLogLevel(LogType _type, bool _enable)
		{
		}

		// Token: 0x0600CFD9 RID: 53209 RVA: 0x004B783C File Offset: 0x004B5A3C
		public string GetDescription()
		{
			return "WebCommandResult_for_" + this.command + "_by_" + this.sourceName;
		}

		// Token: 0x04009D3A RID: 40250
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string command;

		// Token: 0x04009D3B RID: 40251
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string parameters;

		// Token: 0x04009D3C RID: 40252
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string sourceName;

		// Token: 0x04009D3D RID: 40253
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly RequestContext context;

		// Token: 0x04009D3E RID: 40254
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WebCommandResult.ResultType responseType;

		// Token: 0x04009D3F RID: 40255
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("resultRaw");

		// Token: 0x04009D40 RID: 40256
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009D41 RID: 40257
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonParametersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parameters");

		// Token: 0x04009D42 RID: 40258
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonResultKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("result");

		// Token: 0x02001AF2 RID: 6898
		public enum ResultType
		{
			// Token: 0x04009D44 RID: 40260
			Full,
			// Token: 0x04009D45 RID: 40261
			ResultOnly,
			// Token: 0x04009D46 RID: 40262
			Raw
		}
	}
}
