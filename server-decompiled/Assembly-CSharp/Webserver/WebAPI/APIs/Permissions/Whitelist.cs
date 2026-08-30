using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE9 RID: 6889
	[Preserve]
	public class Whitelist : AbsRestApi
	{
		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600CF4D RID: 53069 RVA: 0x004B78EB File Offset: 0x004B5AEB
		public static AdminWhitelist WhitelistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Whitelist;
			}
		}

		// Token: 0x0600CF4E RID: 53070 RVA: 0x004B78FC File Offset: 0x004B5AFC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (string.IsNullOrEmpty(requestPath))
			{
				jsonWriter.WriteRaw(Whitelist.jsonKeyUsers);
				jsonWriter.WriteBeginArray();
				bool flag = true;
				foreach (KeyValuePair<PlatformUserIdentifierAbs, AdminWhitelist.WhitelistUser> keyValuePair in Whitelist.WhitelistInstance.GetUsers())
				{
					PlatformUserIdentifierAbs platformUserIdentifierAbs;
					AdminWhitelist.WhitelistUser whitelistUser;
					keyValuePair.Deconstruct(out platformUserIdentifierAbs, out whitelistUser);
					AdminWhitelist.WhitelistUser whitelistUser2 = whitelistUser;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeUserJson(ref jsonWriter, whitelistUser2);
				}
				jsonWriter.WriteEndArray();
				jsonWriter.WriteRaw(Whitelist.jsonKeyGroups);
				jsonWriter.WriteBeginArray();
				flag = true;
				foreach (KeyValuePair<string, AdminWhitelist.WhitelistGroup> keyValuePair2 in Whitelist.WhitelistInstance.GetGroups())
				{
					string text;
					AdminWhitelist.WhitelistGroup whitelistGroup;
					keyValuePair2.Deconstruct(out text, out whitelistGroup);
					AdminWhitelist.WhitelistGroup whitelistGroup2 = whitelistGroup;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeGroupJson(ref jsonWriter, whitelistGroup2);
				}
				jsonWriter.WriteEndArray();
				jsonWriter.WriteEndObject();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CF4F RID: 53071 RVA: 0x004B7A5C File Offset: 0x004B5C5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWhitelist.WhitelistUser _userPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_userPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _userPermission.UserIdentifier);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF50 RID: 53072 RVA: 0x004B7A9B File Offset: 0x004B5C9B
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeGroupJson(ref JsonWriter _writer, AdminWhitelist.WhitelistGroup _groupPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_groupPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyGroupId);
			_writer.WriteString(_groupPermission.SteamIdGroup);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF51 RID: 53073 RVA: 0x004B7ADC File Offset: 0x004B5CDC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			string text;
			if (!PermissionsApiHelpers.TryParseId(_context, _jsonInputData, out platformUserIdentifierAbs, out text))
			{
				return;
			}
			string text2;
			JsonCommons.TryGetJsonField(_jsonInput, "name", out text2);
			if (platformUserIdentifierAbs != null)
			{
				Whitelist.WhitelistInstance.AddUser(text2, platformUserIdentifierAbs);
			}
			else
			{
				Whitelist.WhitelistInstance.AddGroup(text2, text);
			}
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CF52 RID: 53074 RVA: 0x004B7B30 File Offset: 0x004B5D30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			string text;
			if (!PermissionsApiHelpers.TryParseId(_context, null, out platformUserIdentifierAbs, out text))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, ((platformUserIdentifierAbs != null) ? Whitelist.WhitelistInstance.RemoveUser(platformUserIdentifierAbs) : Whitelist.WhitelistInstance.RemoveGroup(text)) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x0600CF53 RID: 53075 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF54 RID: 53076 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF55 RID: 53077 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Whitelist()
			: base(null)
		{
		}

		// Token: 0x04009D5C RID: 40284
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009D5D RID: 40285
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009D5E RID: 40286
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009D5F RID: 40287
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009D60 RID: 40288
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009D61 RID: 40289
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D62 RID: 40290
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009D63 RID: 40291
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");
	}
}
