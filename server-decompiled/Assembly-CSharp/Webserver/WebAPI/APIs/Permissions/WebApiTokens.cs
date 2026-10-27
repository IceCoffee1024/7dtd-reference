using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B43 RID: 6979
	[Preserve]
	public class WebApiTokens : AbsRestApi
	{
		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x0600D19D RID: 53661 RVA: 0x004BEFC9 File Offset: 0x004BD1C9
		public static AdminApiTokens ApiTokensInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminApiTokens.Instance;
			}
		}

		// Token: 0x0600D19E RID: 53662 RVA: 0x004BEFD0 File Offset: 0x004BD1D0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (string.IsNullOrEmpty(requestPath))
			{
				jsonWriter.WriteBeginArray();
				bool flag = true;
				foreach (KeyValuePair<string, AdminApiTokens.ApiToken> keyValuePair in WebApiTokens.ApiTokensInstance.GetTokens())
				{
					string text;
					AdminApiTokens.ApiToken apiToken;
					keyValuePair.Deconstruct(out text, out apiToken);
					AdminApiTokens.ApiToken apiToken2 = apiToken;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeTokenJson(ref jsonWriter, apiToken2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600D19F RID: 53663 RVA: 0x004BF098 File Offset: 0x004BD298
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeTokenJson(ref JsonWriter _writer, AdminApiTokens.ApiToken _token)
		{
			_writer.WriteRaw(WebApiTokens.jsonKeyName);
			_writer.WriteString(_token.Name);
			_writer.WriteRaw(WebApiTokens.jsonKeySecret);
			_writer.WriteString(_token.Secret);
			_writer.WriteRaw(WebApiTokens.jsonKeyPermissionLevel);
			_writer.WriteInt32(_token.PermissionLevel);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D1A0 RID: 53664 RVA: 0x004BF0F0 File Offset: 0x004BD2F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string requestPath = _context.RequestPath;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_NAME, null);
				return;
			}
			string text;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "secret", out text))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_SECRET, null);
				return;
			}
			int num;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevel", out num))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL, null);
				return;
			}
			WebApiTokens.ApiTokensInstance.AddToken(requestPath, text, num);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600D1A1 RID: 53665 RVA: 0x004BF174 File Offset: 0x004BD374
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			AbsRestApi.SendEmptyResponse(_context, WebApiTokens.ApiTokensInstance.RemoveToken(requestPath) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x0600D1A2 RID: 53666 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D1A3 RID: 53667 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D1A4 RID: 53668 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public WebApiTokens()
			: base(null)
		{
		}

		// Token: 0x04009EF5 RID: 40693
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009EF6 RID: 40694
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertySecret = "secret";

		// Token: 0x04009EF7 RID: 40695
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009EF8 RID: 40696
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009EF9 RID: 40697
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeySecret = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("secret");

		// Token: 0x04009EFA RID: 40698
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");
	}
}
