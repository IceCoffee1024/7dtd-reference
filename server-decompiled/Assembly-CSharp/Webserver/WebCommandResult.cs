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
	// Token: 0x02001AC5 RID: 6853
	public class WebCommandResult : IConsoleConnection
	{
		// Token: 0x0600CE7C RID: 52860 RVA: 0x004B3130 File Offset: 0x004B1330
		public WebCommandResult(string _command, string _parameters, WebCommandResult.ResultType _responseType, RequestContext _context)
		{
			this.context = _context;
			this.command = _command;
			this.parameters = _parameters;
			this.responseType = _responseType;
			WebConnection connection = _context.Connection;
			this.sourceName = ((connection != null) ? connection.Username : null) ?? ("Unauth-PermLevel-" + _context.PermissionLevel.ToString());
		}

		// Token: 0x0600CE7D RID: 52861 RVA: 0x004B3198 File Offset: 0x004B1398
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

		// Token: 0x0600CE7E RID: 52862 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLine(string _text)
		{
		}

		// Token: 0x0600CE7F RID: 52863 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600CE80 RID: 52864 RVA: 0x000027FC File Offset: 0x000009FC
		public void EnableLogLevel(LogType _type, bool _enable)
		{
		}

		// Token: 0x0600CE81 RID: 52865 RVA: 0x004B3370 File Offset: 0x004B1570
		public string GetDescription()
		{
			return "WebCommandResult_for_" + this.command + "_by_" + this.sourceName;
		}

		// Token: 0x04009C74 RID: 40052
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string command;

		// Token: 0x04009C75 RID: 40053
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string parameters;

		// Token: 0x04009C76 RID: 40054
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string sourceName;

		// Token: 0x04009C77 RID: 40055
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly RequestContext context;

		// Token: 0x04009C78 RID: 40056
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WebCommandResult.ResultType responseType;

		// Token: 0x04009C79 RID: 40057
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("resultRaw");

		// Token: 0x04009C7A RID: 40058
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCommandKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009C7B RID: 40059
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonParametersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("parameters");

		// Token: 0x04009C7C RID: 40060
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonResultKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("result");

		// Token: 0x02001AC6 RID: 6854
		public enum ResultType
		{
			// Token: 0x04009C7E RID: 40062
			Full,
			// Token: 0x04009C7F RID: 40063
			ResultOnly,
			// Token: 0x04009C80 RID: 40064
			Raw
		}
	}
}
