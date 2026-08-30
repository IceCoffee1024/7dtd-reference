using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AC1 RID: 6849
	[Preserve]
	public class WebUsers : AbsRestApi
	{
		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x0600CDBA RID: 52666 RVA: 0x004B1264 File Offset: 0x004AF464
		public static AdminWebUsers WebUsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebUsers.Instance;
			}
		}

		// Token: 0x0600CDBB RID: 52667 RVA: 0x004B126C File Offset: 0x004AF46C
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
				foreach (KeyValuePair<string, AdminWebUsers.WebUser> keyValuePair in WebUsers.WebUsersInstance.GetUsers())
				{
					string text;
					AdminWebUsers.WebUser webUser;
					keyValuePair.Deconstruct(out text, out webUser);
					AdminWebUsers.WebUser webUser2 = webUser;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeUserJson(ref jsonWriter, webUser2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CDBC RID: 52668 RVA: 0x004B1334 File Offset: 0x004AF534
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWebUsers.WebUser _user)
		{
			_writer.WriteRaw(WebUsers.jsonKeyName);
			_writer.WriteString(_user.Name ?? "");
			_writer.WriteRaw(WebUsers.jsonKeyPlatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.PlatformUser);
			_writer.WriteRaw(WebUsers.jsonKeyCrossplatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.CrossPlatformUser);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CDBD RID: 52669 RVA: 0x004B1398 File Offset: 0x004AF598
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string text;
			if (!this.TryParseName(_context, _jsonInputData, out text))
			{
				return;
			}
			string text2;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "password", out text2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PASSWORD, null);
				return;
			}
			IDictionary<string, object> dictionary;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "platformUserId", out dictionary))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (!JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = null;
			if (JsonCommons.TryGetJsonField(_jsonInput, "crossplatformUserId", out dictionary) && !JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_CROSSPLATFORM_USER_ID, null);
				return;
			}
			WebUsers.WebUsersInstance.AddUser(text, text2, platformUserIdentifierAbs, platformUserIdentifierAbs2);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CDBE RID: 52670 RVA: 0x004B1454 File Offset: 0x004AF654
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string text;
			if (!this.TryParseName(_context, null, out text))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, WebUsers.WebUsersInstance.RemoveUser(text) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x0600CDBF RID: 52671 RVA: 0x004B1494 File Offset: 0x004AF694
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryParseName(RequestContext _context, byte[] _jsonInputData, out string _userName)
		{
			string requestPath = _context.RequestPath;
			_userName = null;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_USERNAME, null);
				return false;
			}
			_userName = requestPath;
			return true;
		}

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600CDC0 RID: 52672 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CDC1 RID: 52673 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CDC2 RID: 52674 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public WebUsers()
			: base(null)
		{
		}

		// Token: 0x04009C24 RID: 39972
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009C25 RID: 39973
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPassword = "password";

		// Token: 0x04009C26 RID: 39974
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPlatformUserId = "platformUserId";

		// Token: 0x04009C27 RID: 39975
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCrossplatformUserId = "crossplatformUserId";

		// Token: 0x04009C28 RID: 39976
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009C29 RID: 39977
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId");

		// Token: 0x04009C2A RID: 39978
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCrossplatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformUserId");
	}
}
