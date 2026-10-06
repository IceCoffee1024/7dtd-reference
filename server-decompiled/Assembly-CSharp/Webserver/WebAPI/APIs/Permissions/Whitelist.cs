using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B15 RID: 6933
	[Preserve]
	public class Whitelist : AbsRestApi
	{
		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600D0A5 RID: 53413 RVA: 0x004BBDB7 File Offset: 0x004B9FB7
		public static AdminWhitelist WhitelistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Whitelist;
			}
		}

		// Token: 0x0600D0A6 RID: 53414 RVA: 0x004BBDC8 File Offset: 0x004B9FC8
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

		// Token: 0x0600D0A7 RID: 53415 RVA: 0x004BBF28 File Offset: 0x004BA128
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWhitelist.WhitelistUser _userPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_userPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _userPermission.UserIdentifier);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D0A8 RID: 53416 RVA: 0x004BBF67 File Offset: 0x004BA167
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeGroupJson(ref JsonWriter _writer, AdminWhitelist.WhitelistGroup _groupPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_groupPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyGroupId);
			_writer.WriteString(_groupPermission.SteamIdGroup);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D0A9 RID: 53417 RVA: 0x004BBFA8 File Offset: 0x004BA1A8
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

		// Token: 0x0600D0AA RID: 53418 RVA: 0x004BBFFC File Offset: 0x004BA1FC
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

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x0600D0AB RID: 53419 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D0AC RID: 53420 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D0AD RID: 53421 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public Whitelist()
			: base(null)
		{
		}

		// Token: 0x04009E22 RID: 40482
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009E23 RID: 40483
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009E24 RID: 40484
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009E25 RID: 40485
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009E26 RID: 40486
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009E27 RID: 40487
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E28 RID: 40488
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009E29 RID: 40489
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");
	}
}
