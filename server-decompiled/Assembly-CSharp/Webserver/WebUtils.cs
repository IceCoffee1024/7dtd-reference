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
	// Token: 0x02001B27 RID: 6951
	public static class WebUtils
	{
		// Token: 0x0600D0FF RID: 53503 RVA: 0x004BB5AC File Offset: 0x004B97AC
		[PublicizedFrom(EAccessModifier.Private)]
		static WebUtils()
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			jsonWriter.WriteEndArray();
			WebUtils.JsonEmptyData = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600D100 RID: 53504 RVA: 0x004BB684 File Offset: 0x004B9884
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

		// Token: 0x0600D101 RID: 53505 RVA: 0x004BB6EC File Offset: 0x004B98EC
		public static bool IsSslRedirected(SpaceWizards.HttpListener.HttpListenerRequest _req)
		{
			string text = _req.Headers["X-Forwarded-Proto"];
			return !string.IsNullOrEmpty(text) && text.Equals("https", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600D102 RID: 53506 RVA: 0x004BB720 File Offset: 0x004B9920
		public static string GenerateGuid()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x0600D103 RID: 53507 RVA: 0x004BB740 File Offset: 0x004B9940
		public static void WriteJsonData(SpaceWizards.HttpListener.HttpListenerResponse _resp, ref JsonWriter _jsonWriter, HttpStatusCode _statusCode = HttpStatusCode.OK)
		{
			ArraySegment<byte> buffer = _jsonWriter.GetBuffer();
			_resp.StatusCode = (int)_statusCode;
			_resp.ContentType = "application/json";
			_resp.ContentEncoding = Encoding.UTF8;
			_resp.ContentLength64 = (long)buffer.Count;
			_resp.OutputStream.Write(buffer.Array, 0, buffer.Count);
		}

		// Token: 0x0600D104 RID: 53508 RVA: 0x004BB799 File Offset: 0x004B9999
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			_writer = default(JsonWriter);
			_writer.WriteRaw(WebUtils.jsonRawDataKey);
		}

		// Token: 0x0600D105 RID: 53509 RVA: 0x004BB7B0 File Offset: 0x004B99B0
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

		// Token: 0x0600D106 RID: 53510 RVA: 0x004BB89C File Offset: 0x004B9A9C
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			WebUtils.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			WebUtils.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600D107 RID: 53511 RVA: 0x004BB8C9 File Offset: 0x004B9AC9
		public static bool TryGetValue(this NameValueCollection _nameValueCollection, string _name, out string _result)
		{
			_result = _nameValueCollection[_name];
			return _result != null;
		}

		// Token: 0x04009E41 RID: 40513
		public const string MimePlain = "text/plain";

		// Token: 0x04009E42 RID: 40514
		public const string MimeHtml = "text/html";

		// Token: 0x04009E43 RID: 40515
		public const string MimeJson = "application/json";

		// Token: 0x04009E44 RID: 40516
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler envelopeBuildSampler = CustomSampler.Create("JSON_EnvelopeBuilding", false);

		// Token: 0x04009E45 RID: 40517
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler netWriteSampler = CustomSampler.Create("JSON_Write", false);

		// Token: 0x04009E46 RID: 40518
		public static readonly byte[] JsonEmptyData;

		// Token: 0x04009E47 RID: 40519
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawDataKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("data");

		// Token: 0x04009E48 RID: 40520
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("meta");

		// Token: 0x04009E49 RID: 40521
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaServertimeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("serverTime");

		// Token: 0x04009E4A RID: 40522
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestMethodKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestMethod");

		// Token: 0x04009E4B RID: 40523
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestSubpathKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestSubpath");

		// Token: 0x04009E4C RID: 40524
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestBodyKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestBody");

		// Token: 0x04009E4D RID: 40525
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaErrorCodeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorCode");

		// Token: 0x04009E4E RID: 40526
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionMessageKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionMessage");

		// Token: 0x04009E4F RID: 40527
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionTraceKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionTrace");
	}
}
