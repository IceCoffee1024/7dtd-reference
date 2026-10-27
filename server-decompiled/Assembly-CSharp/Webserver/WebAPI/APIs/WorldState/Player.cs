using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001B35 RID: 6965
	[Preserve]
	public class Player : AbsRestApi
	{
		// Token: 0x0600D154 RID: 53588 RVA: 0x004BD32C File Offset: 0x004BB52C
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

		// Token: 0x0600D155 RID: 53589 RVA: 0x004BD448 File Offset: 0x004BB648
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

		// Token: 0x0600D156 RID: 53590 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x0600D157 RID: 53591 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public Player()
			: base(null)
		{
		}

		// Token: 0x04009EA2 RID: 40610
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPlayersKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("players");

		// Token: 0x04009EA3 RID: 40611
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonEntityIdKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("entityId");

		// Token: 0x04009EA4 RID: 40612
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonNameKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009EA5 RID: 40613
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPlatformIdKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009EA6 RID: 40614
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCrossplatformIdKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformId");

		// Token: 0x04009EA7 RID: 40615
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonTotalPlayTimeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("totalPlayTimeSeconds");

		// Token: 0x04009EA8 RID: 40616
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLastOnlineKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("lastOnline");

		// Token: 0x04009EA9 RID: 40617
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOnlineKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("online");

		// Token: 0x04009EAA RID: 40618
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonIpKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ip");

		// Token: 0x04009EAB RID: 40619
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPingKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ping");

		// Token: 0x04009EAC RID: 40620
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPositionKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");

		// Token: 0x04009EAD RID: 40621
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLevelKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("level");

		// Token: 0x04009EAE RID: 40622
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonHealthKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("health");

		// Token: 0x04009EAF RID: 40623
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonStaminaKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("stamina");

		// Token: 0x04009EB0 RID: 40624
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonScoreKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("score");

		// Token: 0x04009EB1 RID: 40625
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonDeathsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("deaths");

		// Token: 0x04009EB2 RID: 40626
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("kills");

		// Token: 0x04009EB3 RID: 40627
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsZombiesKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("zombies");

		// Token: 0x04009EB4 RID: 40628
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKillsPlayersKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("players");

		// Token: 0x04009EB5 RID: 40629
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBannedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("banned");

		// Token: 0x04009EB6 RID: 40630
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanActiveKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("banActive");

		// Token: 0x04009EB7 RID: 40631
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanReasonKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("reason");

		// Token: 0x04009EB8 RID: 40632
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonBanUntilKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("until");
	}
}
