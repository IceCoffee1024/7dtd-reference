using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B46 RID: 6982
	[Preserve]
	public class Whitelist : AbsRestApi
	{
		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x0600D1B9 RID: 53689 RVA: 0x004BF8B3 File Offset: 0x004BDAB3
		public static AdminWhitelist WhitelistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Whitelist;
			}
		}

		// Token: 0x0600D1BA RID: 53690 RVA: 0x004BF8C4 File Offset: 0x004BDAC4
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

		// Token: 0x0600D1BB RID: 53691 RVA: 0x004BFA24 File Offset: 0x004BDC24
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWhitelist.WhitelistUser _userPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_userPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _userPermission.UserIdentifier);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D1BC RID: 53692 RVA: 0x004BFA63 File Offset: 0x004BDC63
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeGroupJson(ref JsonWriter _writer, AdminWhitelist.WhitelistGroup _groupPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_groupPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyGroupId);
			_writer.WriteString(_groupPermission.SteamIdGroup);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D1BD RID: 53693 RVA: 0x004BFAA4 File Offset: 0x004BDCA4
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

		// Token: 0x0600D1BE RID: 53694 RVA: 0x004BFAF8 File Offset: 0x004BDCF8
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

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x0600D1BF RID: 53695 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D1C0 RID: 53696 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D1C1 RID: 53697 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public Whitelist()
			: base(null)
		{
		}

		// Token: 0x04009F0B RID: 40715
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009F0C RID: 40716
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009F0D RID: 40717
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009F0E RID: 40718
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009F0F RID: 40719
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009F10 RID: 40720
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009F11 RID: 40721
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009F12 RID: 40722
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");
	}
}
