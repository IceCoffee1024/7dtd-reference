using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using SpaceWizards.HttpListener;
using UnityEngine.Profiling;
using Utf8Json;
using Webserver.WebAPI;

namespace Webserver
{
	// Token: 0x02001ACA RID: 6858
	public static class WebUtils
	{
		// Token: 0x0600CE93 RID: 52883 RVA: 0x004B35E4 File Offset: 0x004B17E4
		[PublicizedFrom(EAccessModifier.Private)]
		static WebUtils()
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			jsonWriter.WriteEndArray();
			WebUtils.JsonEmptyData = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600CE94 RID: 52884 RVA: 0x004B36BC File Offset: 0x004B18BC
		public static void WriteText(SpaceWizards.HttpListener.HttpListenerResponse _resp, string _text, HttpStatusCode _statusCode = HttpStatusCode.OK, string _mimeType = null)
		{
			_resp.StatusCode = (int)_statusCode;
			_resp.ContentType = _mimeType ?? "text/plain";
			_resp.ContentEncoding = Encoding.UTF8;
			if (string.IsNullOrEmpty(_text))
			{
				_resp.ContentLength64 = 0L;
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(_text);
			_resp.ContentLength64 = (long)bytes.Length;
			_resp.OutputStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600CE95 RID: 52885 RVA: 0x004B3724 File Offset: 0x004B1924
		public static bool IsSslRedirected(SpaceWizards.HttpListener.HttpListenerRequest _req)
		{
			string text = _req.Headers["X-Forwarded-Proto"];
			return !string.IsNullOrEmpty(text) && text.Equals("https", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600CE96 RID: 52886 RVA: 0x004B3758 File Offset: 0x004B1958
		public static string GenerateGuid()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x0600CE97 RID: 52887 RVA: 0x004B3778 File Offset: 0x004B1978
		public static void WriteJsonData(SpaceWizards.HttpListener.HttpListenerResponse _resp, ref JsonWriter _jsonWriter, HttpStatusCode _statusCode = HttpStatusCode.OK)
		{
			ArraySegment<byte> buffer = _jsonWriter.GetBuffer();
			_resp.StatusCode = (int)_statusCode;
			_resp.ContentType = "application/json";
			_resp.ContentEncoding = Encoding.UTF8;
			_resp.ContentLength64 = (long)buffer.Count;
			_resp.OutputStream.Write(buffer.Array, 0, buffer.Count);
		}

		// Token: 0x0600CE98 RID: 52888 RVA: 0x004B37D1 File Offset: 0x004B19D1
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			_writer = default(JsonWriter);
			_writer.WriteRaw(WebUtils.jsonRawDataKey);
		}

		// Token: 0x0600CE99 RID: 52889 RVA: 0x004B37E8 File Offset: 0x004B19E8
		public static void SendEnvelopedResult(RequestContext _context, ref JsonWriter _writer, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			_writer.WriteRaw(WebUtils.jsonRawMetaKey);
			_writer.WriteRaw(WebUtils.jsonRawMetaServertimeKey);
			JsonCommons.WriteDateTime(ref _writer, DateTime.Now);
			if (!string.IsNullOrEmpty(_errorCode))
			{
				_writer.WriteRaw(WebUtils.jsonRawMetaRequestMethodKey);
				_writer.WriteString(_context.Request.HttpMethod);
				_writer.WriteRaw(WebUtils.jsonRawMetaRequestSubpathKey);
				_writer.WriteString(_context.RequestPath);
				_writer.WriteRaw(WebUtils.jsonRawMetaRequestBodyKey);
				if (_jsonInputData != null)
				{
					_writer.WriteRaw(_jsonInputData);
				}
				else
				{
					_writer.WriteNull();
				}
				_writer.WriteRaw(WebUtils.jsonRawMetaErrorCodeKey);
				_writer.WriteString(_errorCode);
				if (_exception != null)
				{
					_writer.WriteRaw(WebUtils.jsonRawMetaExceptionMessageKey);
					_writer.WriteString(_exception.Message);
					_writer.WriteRaw(WebUtils.jsonRawMetaExceptionTraceKey);
					_writer.WriteString(_exception.StackTrace);
				}
			}
			_writer.WriteEndObject();
			_writer.WriteEndObject();
			WebUtils.WriteJsonData(_context.Response, ref _writer, _statusCode);
		}

		// Token: 0x0600CE9A RID: 52890 RVA: 0x004B38D4 File Offset: 0x004B1AD4
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			WebUtils.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			WebUtils.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600CE9B RID: 52891 RVA: 0x004B3901 File Offset: 0x004B1B01
		public static bool TryGetValue(this NameValueCollection _nameValueCollection, string _name, out string _result)
		{
			_result = _nameValueCollection[_name];
			return _result != null;
		}

		// Token: 0x04009C92 RID: 40082
		public const string MimePlain = "text/plain";

		// Token: 0x04009C93 RID: 40083
		public const string MimeHtml = "text/html";

		// Token: 0x04009C94 RID: 40084
		public const string MimeJson = "application/json";

		// Token: 0x04009C95 RID: 40085
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler envelopeBuildSampler = CustomSampler.Create("JSON_EnvelopeBuilding", false);

		// Token: 0x04009C96 RID: 40086
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler netWriteSampler = CustomSampler.Create("JSON_Write", false);

		// Token: 0x04009C97 RID: 40087
		public static readonly byte[] JsonEmptyData;

		// Token: 0x04009C98 RID: 40088
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawDataKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("data");

		// Token: 0x04009C99 RID: 40089
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("meta");

		// Token: 0x04009C9A RID: 40090
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaServertimeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("serverTime");

		// Token: 0x04009C9B RID: 40091
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestMethodKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestMethod");

		// Token: 0x04009C9C RID: 40092
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestSubpathKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestSubpath");

		// Token: 0x04009C9D RID: 40093
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestBodyKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestBody");

		// Token: 0x04009C9E RID: 40094
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaErrorCodeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorCode");

		// Token: 0x04009C9F RID: 40095
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionMessageKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionMessage");

		// Token: 0x04009CA0 RID: 40096
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionTraceKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionTrace");
	}
}
