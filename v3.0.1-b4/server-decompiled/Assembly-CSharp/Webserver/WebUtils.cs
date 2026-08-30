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
	// Token: 0x02001AA3 RID: 6819
	public static class WebUtils
	{
		// Token: 0x0600CD0A RID: 52490 RVA: 0x004AD1F0 File Offset: 0x004AB3F0
		[PublicizedFrom(EAccessModifier.Private)]
		static WebUtils()
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			jsonWriter.WriteEndArray();
			WebUtils.JsonEmptyData = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600CD0B RID: 52491 RVA: 0x004AD2C8 File Offset: 0x004AB4C8
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

		// Token: 0x0600CD0C RID: 52492 RVA: 0x004AD330 File Offset: 0x004AB530
		public static bool IsSslRedirected(SpaceWizards.HttpListener.HttpListenerRequest _req)
		{
			string text = _req.Headers["X-Forwarded-Proto"];
			return !string.IsNullOrEmpty(text) && text.Equals("https", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600CD0D RID: 52493 RVA: 0x004AD364 File Offset: 0x004AB564
		public static string GenerateGuid()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x0600CD0E RID: 52494 RVA: 0x004AD384 File Offset: 0x004AB584
		public static void WriteJsonData(SpaceWizards.HttpListener.HttpListenerResponse _resp, ref JsonWriter _jsonWriter, HttpStatusCode _statusCode = HttpStatusCode.OK)
		{
			ArraySegment<byte> buffer = _jsonWriter.GetBuffer();
			_resp.StatusCode = (int)_statusCode;
			_resp.ContentType = "application/json";
			_resp.ContentEncoding = Encoding.UTF8;
			_resp.ContentLength64 = (long)buffer.Count;
			_resp.OutputStream.Write(buffer.Array, 0, buffer.Count);
		}

		// Token: 0x0600CD0F RID: 52495 RVA: 0x004AD3DD File Offset: 0x004AB5DD
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			_writer = default(JsonWriter);
			_writer.WriteRaw(WebUtils.jsonRawDataKey);
		}

		// Token: 0x0600CD10 RID: 52496 RVA: 0x004AD3F4 File Offset: 0x004AB5F4
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

		// Token: 0x0600CD11 RID: 52497 RVA: 0x004AD4E0 File Offset: 0x004AB6E0
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			WebUtils.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			WebUtils.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600CD12 RID: 52498 RVA: 0x004AD50D File Offset: 0x004AB70D
		public static bool TryGetValue(this NameValueCollection _nameValueCollection, string _name, out string _result)
		{
			_result = _nameValueCollection[_name];
			return _result != null;
		}

		// Token: 0x04009B61 RID: 39777
		public const string MimePlain = "text/plain";

		// Token: 0x04009B62 RID: 39778
		public const string MimeHtml = "text/html";

		// Token: 0x04009B63 RID: 39779
		public const string MimeJson = "application/json";

		// Token: 0x04009B64 RID: 39780
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler envelopeBuildSampler = CustomSampler.Create("JSON_EnvelopeBuilding", false);

		// Token: 0x04009B65 RID: 39781
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler netWriteSampler = CustomSampler.Create("JSON_Write", false);

		// Token: 0x04009B66 RID: 39782
		public static readonly byte[] JsonEmptyData;

		// Token: 0x04009B67 RID: 39783
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawDataKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("data");

		// Token: 0x04009B68 RID: 39784
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("meta");

		// Token: 0x04009B69 RID: 39785
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaServertimeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("serverTime");

		// Token: 0x04009B6A RID: 39786
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestMethodKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestMethod");

		// Token: 0x04009B6B RID: 39787
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestSubpathKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestSubpath");

		// Token: 0x04009B6C RID: 39788
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaRequestBodyKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("requestBody");

		// Token: 0x04009B6D RID: 39789
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaErrorCodeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("errorCode");

		// Token: 0x04009B6E RID: 39790
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionMessageKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionMessage");

		// Token: 0x04009B6F RID: 39791
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonRawMetaExceptionTraceKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("exceptionTrace");
	}
}
