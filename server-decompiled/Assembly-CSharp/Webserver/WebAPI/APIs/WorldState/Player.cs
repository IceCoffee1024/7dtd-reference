using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001AD8 RID: 6872
	[Preserve]
	public class Player : AbsRestApi
	{
		// Token: 0x0600CEE8 RID: 52968 RVA: 0x004B5364 File Offset: 0x004B3564
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			bool flag = PermissionUtils.CanViewAllPlayers(_context.PermissionLevel);
			WebConnection connection = _context.Connection;
			PlatformUserIdentifierAbs platformUserIdentifierAbs = ((connection != null) ? connection.UserId : null);
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(Player.jsonPlayersKey);
			jsonWriter.WriteBeginArray();
			int num = 0;
			if (string.IsNullOrEmpty(requestPath))
			{
				for (int i = 0; i < SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List.Count; i++)
				{
					ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List[i];
					this.writePlayerJson(ref jsonWriter, ref num, clientInfo.PlatformId, flag, platformUserIdentifierAbs);
				}
			}
			else
			{
				int num2;
				ClientInfo clientInfo2;
				if (!int.TryParse(requestPath, out num2) || (clientInfo2 = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num2)) == null)
				{
					jsonWriter.WriteEndArray();
					jsonWriter.WriteEndObject();
					AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.NotFound, null, null, null);
					return;
				}
				this.writePlayerJson(ref jsonWriter, ref num, clientInfo2.PlatformId, flag, platformUserIdentifierAbs);
			}
			jsonWriter.WriteEndArray();
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CEE9 RID: 52969 RVA: 0x004B5480 File Offset: 0x004B3680
		[PublicizedFrom(EAccessModifier.Private)]
		public void writePlayerJson(ref JsonWriter _writer, ref int _written, PlatformUserIdentifierAbs _nativeUserId, bool _allowViewAll, PlatformUserIdentifierAbs _requesterNativeUserId)
		{
			if (!_allowViewAll && (_requesterNativeUserId == null || !_requesterNativeUserId.Equals(_nativeUserId)))
			{
				return;
			}
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(_nativeUserId);
			if (clientInfo == null)
			{
				Log.Warning("[Web] Player.GET: ClientInfo null");
				return;
			}
			int entityId = clientInfo.entityId;
			EntityPlayer entityPlayer;
			GameManager.Instance.World.Players.dict.TryGetValue(entityId, out entityPlayer);
			if (entityPlayer == null)
			{
				Log.Warning("[Web] Player.GET: EntityPlayer null");
				return;
			}
			if (_written > 0)
			{
				_writer.WriteValueSeparator();
			}
			_written++;
			bool flag = true;
			_writer.WriteRaw(Player.jsonEntityIdKey);
			_writer.WriteInt32(entityId);
			_writer.WriteRaw(Player.jsonNameKey);
			_writer.WriteString(clientInfo.playerName);
			_writer.WriteRaw(Player.jsonPlatformIdKey);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _nativeUserId);
			_writer.WriteRaw(Player.jsonCrossplatformIdKey);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, clientInfo.CrossplatformId);
			_writer.WriteRaw(Player.jsonTotalPlayTimeKey);
			_writer.WriteNull();
			_writer.WriteRaw(Player.jsonLastOnlineKey);
			_writer.WriteNull();
			_writer.WriteRaw(Player.jsonOnlineKey);
			_writer.WriteBoolean(flag);
			_writer.WriteRaw(Player.jsonIpKey);
			if (flag)
			{
				_writer.WriteString(clientInfo.ip);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteRaw(Player.jsonPingKey);
			if (flag)
			{
				_writer.WriteInt32(clientInfo.ping);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteRaw(Player.jsonPositionKey);
			if (flag)
			{
				JsonCommons.WriteVector3(ref _writer, entityPlayer.GetPosition());
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteRaw(Player.jsonLevelKey);
			_writer.WriteInt32(entityPlayer.Progression.Level);
			_writer.WriteRaw(Player.jsonHealthKey);
			_writer.WriteInt32(entityPlayer.Health);
			_writer.WriteRaw(Player.jsonStaminaKey);
			_writer.WriteSingle(entityPlayer.Stamina);
			_writer.WriteRaw(Player.jsonScoreKey);
			_writer.WriteInt32(entityPlayer.Score);
			_writer.WriteRaw(Player.jsonDeathsKey);
			_writer.WriteInt32(entityPlayer.Died);
			_writer.WriteRaw(Player.jsonKillsKey);
			_writer.WriteRaw(Player.jsonKillsZombiesKey);
			_writer.WriteInt32(entityPlayer.KilledZombies);
			_writer.WriteRaw(Player.jsonKillsPlayersKey);
			_writer.WriteInt32(entityPlayer.KilledPlayers);
			_writer.WriteEndObject();
			_writer.WriteRaw(Player.jsonBannedKey);
			DateTime dateTime;
			string text;
			bool flag2 = GameManager.Instance.adminTools.Blacklist.IsBanned(_nativeUserId, out dateTime, out text);
			if (!flag2 && clientInfo.CrossplatformId != null)
			{
				flag2 = GameManager.Instance.adminTools.Blacklist.IsBanned(clientInfo.CrossplatformId, out dateTime, out text);
			}
			_writer.WriteRaw(Player.jsonBanActiveKey);
			_writer.WriteBoolean(flag2);
			_writer.WriteRaw(Player.jsonBanReasonKey);
			if (flag2)
			{
				_writer.WriteString(text);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteRaw(Player.jsonBanUntilKey);
			if (flag2)
			{
				JsonCommons.WriteDateTime(ref _writer, dateTime);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteEndObject();
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEEA RID: 52970 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x0600CEEB RID: 52971 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public Player()
			: base(null)
		{
		}

		// Token: 0x04009CF3 RID: 40179
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPlayersKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("players");

		// Token: 0x04009CF4 RID: 40180
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonEntityIdKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("entityId");

		// Token: 0x04009CF5 RID: 40181
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonNameKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009CF6 RID: 40182
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPlatformIdKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009CF7 RID: 40183
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCrossplatformIdKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformId");

		// Token: 0x04009CF8 RID: 40184
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonTotalPlayTimeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("totalPlayTimeSeconds");

		// Token: 0x04009CF9 RID: 40185
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLastOnlineKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastOnline");

		// Token: 0x04009CFA RID: 40186
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOnlineKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("online");

		// Token: 0x04009CFB RID: 40187
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonIpKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip");

		// Token: 0x04009CFC RID: 40188
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPingKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ping");

		// Token: 0x04009CFD RID: 40189
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPositionKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");

		// Token: 0x04009CFE RID: 40190
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLevelKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("level");

		// Token: 0x04009CFF RID: 40191
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonHealthKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("health");

		// Token: 0x04009D00 RID: 40192
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonStaminaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stamina");

		// Token: 0x04009D01 RID: 40193
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonScoreKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("score");

		// Token: 0x04009D02 RID: 40194
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonDeathsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deaths");

		// Token: 0x04009D03 RID: 40195
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("kills");

		// Token: 0x04009D04 RID: 40196
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsZombiesKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("zombies");

		// Token: 0x04009D05 RID: 40197
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsPlayersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players");

		// Token: 0x04009D06 RID: 40198
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBannedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banned");

		// Token: 0x04009D07 RID: 40199
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanActiveKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("banActive");

		// Token: 0x04009D08 RID: 40200
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanReasonKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason");

		// Token: 0x04009D09 RID: 40201
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanUntilKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("until");
	}
}
