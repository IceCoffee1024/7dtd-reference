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
	// Token: 0x02001AF6 RID: 6902
	public static class WebUtils
	{
		// Token: 0x0600CFEB RID: 53227 RVA: 0x004B7AB0 File Offset: 0x004B5CB0
		[PublicizedFrom(EAccessModifier.Private)]
		static WebUtils()
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			jsonWriter.WriteEndArray();
			WebUtils.JsonEmptyData = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600CFEC RID: 53228 RVA: 0x004B7B88 File Offset: 0x004B5D88
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

		// Token: 0x0600CFED RID: 53229 RVA: 0x004B7BF0 File Offset: 0x004B5DF0
		public static bool IsSslRedirected(SpaceWizards.HttpListener.HttpListenerRequest _req)
		{
			string text = _req.Headers["X-Forwarded-Proto"];
			return !string.IsNullOrEmpty(text) && text.Equals("https", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600CFEE RID: 53230 RVA: 0x004B7C24 File Offset: 0x004B5E24
		public static string GenerateGuid()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x0600CFEF RID: 53231 RVA: 0x004B7C44 File Offset: 0x004B5E44
		public static void WriteJsonData(SpaceWizards.HttpListener.HttpListenerResponse _resp, ref JsonWriter _jsonWriter, HttpStatusCode _statusCode = HttpStatusCode.OK)
		{
			ArraySegment<byte> buffer = _jsonWriter.GetBuffer();
			_resp.StatusCode = (int)_statusCode;
			_resp.ContentType = "application/json";
			_resp.ContentEncoding = Encoding.UTF8;
			_resp.ContentLength64 = (long)buffer.Count;
			_resp.OutputStream.Write(buffer.Array, 0, buffer.Count);
		}

		// Token: 0x0600CFF0 RID: 53232 RVA: 0x004B7C9D File Offset: 0x004B5E9D
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			_writer = default(JsonWriter);
			_writer.WriteRaw(WebUtils.jsonRawDataKey);
		}

		// Token: 0x0600CFF1 RID: 53233 RVA: 0x004B7CB4 File Offset: 0x004B5EB4
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

		// Token: 0x0600CFF2 RID: 53234 RVA: 0x004B7DA0 File Offset: 0x004B5FA0
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			WebUtils.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			WebUtils.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600CFF3 RID: 53235 RVA: 0x004B7DCD File Offset: 0x004B5FCD
		public static bool TryGetValue(this NameValueCollection _nameValueCollection, string _name, out string _result)
		{
			_result = _nameValueCollection[_name];
			return _result != null;
		}

		// Token: 0x04009D58 RID: 40280
		public const string MimePlain = "text/plain";

		// Token: 0x04009D59 RID: 40281
		public const string MimeHtml = "text/html";

		// Token: 0x04009D5A RID: 40282
		public const string MimeJson = "application/json";

		// Token: 0x04009D5B RID: 40283
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler envelopeBuildSampler = CustomSampler.Create("JSON_EnvelopeBuilding", false);

		// Token: 0x04009D5C RID: 40284
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler netWriteSampler = CustomSampler.Create("JSON_Write", false);

		// Token: 0x04009D5D RID: 40285
		public static readonly byte[] JsonEmptyData;

		// Token: 0x04009D5E RID: 40286
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawDataKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("data");

		// Token: 0x04009D5F RID: 40287
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("meta");

		// Token: 0x04009D60 RID: 40288
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaServertimeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("serverTime");

		// Token: 0x04009D61 RID: 40289
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestMethodKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestMethod");

		// Token: 0x04009D62 RID: 40290
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestSubpathKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestSubpath");

		// Token: 0x04009D63 RID: 40291
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestBodyKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestBody");

		// Token: 0x04009D64 RID: 40292
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaErrorCodeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorCode");

		// Token: 0x04009D65 RID: 40293
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionMessageKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionMessage");

		// Token: 0x04009D66 RID: 40294
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionTraceKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionTrace");
	}
}
