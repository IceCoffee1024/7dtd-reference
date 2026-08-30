using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AC2 RID: 6850
	[Preserve]
	public class Whitelist : AbsRestApi
	{
		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x0600CDC4 RID: 52676 RVA: 0x004B14F7 File Offset: 0x004AF6F7
		public static AdminWhitelist WhitelistInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Whitelist;
			}
		}

		// Token: 0x0600CDC5 RID: 52677 RVA: 0x004B1508 File Offset: 0x004AF708
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

		// Token: 0x0600CDC6 RID: 52678 RVA: 0x004B1668 File Offset: 0x004AF868
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWhitelist.WhitelistUser _userPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_userPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _userPermission.UserIdentifier);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CDC7 RID: 52679 RVA: 0x004B16A7 File Offset: 0x004AF8A7
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeGroupJson(ref JsonWriter _writer, AdminWhitelist.WhitelistGroup _groupPermission)
		{
			_writer.WriteRaw(Whitelist.jsonKeyName);
			_writer.WriteString(_groupPermission.Name ?? "");
			_writer.WriteRaw(Whitelist.jsonKeyGroupId);
			_writer.WriteString(_groupPermission.SteamIdGroup);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CDC8 RID: 52680 RVA: 0x004B16E8 File Offset: 0x004AF8E8
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

		// Token: 0x0600CDC9 RID: 52681 RVA: 0x004B173C File Offset: 0x004AF93C
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

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x0600CDCA RID: 52682 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CDCB RID: 52683 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CDCC RID: 52684 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public Whitelist()
			: base(null)
		{
		}

		// Token: 0x04009C2B RID: 39979
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009C2C RID: 39980
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009C2D RID: 39981
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009C2E RID: 39982
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009C2F RID: 39983
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009C30 RID: 39984
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009C31 RID: 39985
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009C32 RID: 39986
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");
	}
}
