using System;
using System.Collections.Generic;
using Platform.Local;
using Platform.Steam;

// Token: 0x02000761 RID: 1889
public class ClientInfo : IEquatable<ClientInfo>
{
	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x060037FA RID: 14330 RVA: 0x0016F955 File Offset: 0x0016DB55
	public PlatformUserIdentifierAbs InternalId
	{
		get
		{
			return this.CrossplatformId ?? this.PlatformId;
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x060037FB RID: 14331 RVA: 0x0016F967 File Offset: 0x0016DB67
	public string ip
	{
		get
		{
			return this.network.GetIP(this);
		}
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x0016F978 File Offset: 0x0016DB78
	public ClientInfo()
	{
		int num;
		do
		{
			num = ++ClientInfo.lastClientNumber;
			if (num > 1000000)
			{
				num = (ClientInfo.lastClientNumber = 1);
			}
		}
		while (SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForClientNumber(num) != null);
		this.ClientNumber = num;
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x0016FA00 File Offset: 0x0016DC00
	public override string ToString()
	{
		string text = null;
		UserIdentifierSteam userIdentifierSteam = this.PlatformId as UserIdentifierSteam;
		if (userIdentifierSteam != null)
		{
			UserIdentifierSteam ownerId = userIdentifierSteam.OwnerId;
			text = ((ownerId != null) ? ownerId.CombinedString : null);
		}
		string text2 = "EntityID={0}, PltfmId='{1}', CrossId='{2}', OwnerID='{3}', PlayerName='{4}', ClientNumber='{5}'";
		object[] array = new object[6];
		array[0] = this.entityId;
		int num = 1;
		PlatformUserIdentifierAbs platformId = this.PlatformId;
		array[num] = ((platformId != null) ? platformId.CombinedString : null) ?? "<unknown>";
		int num2 = 2;
		PlatformUserIdentifierAbs crossplatformId = this.CrossplatformId;
		array[num2] = ((crossplatformId != null) ? crossplatformId.CombinedString : null) ?? "<unknown/none>";
		array[3] = text ?? "<unknown/none>";
		array[4] = this.playerName;
		array[5] = this.ClientNumber;
		return string.Format(text2, array);
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x0016FAAF File Offset: 0x0016DCAF
	public void UpdatePing()
	{
		this.ping = this.network.GetPing(this);
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x0016FAC4 File Offset: 0x0016DCC4
	public void SendPackage(NetPackage _package)
	{
		if (!_package.AllowedBeforeAuth && !this.loginDone)
		{
			Log.Warning(string.Format("Ignoring {0}, not logged in yet", _package));
			return;
		}
		this.netConnection[_package.Channel].AddToSendQueue(_package);
		if (_package.FlushQueue)
		{
			this.netConnection[_package.Channel].FlushSendQueue();
		}
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x0016FB20 File Offset: 0x0016DD20
	public void SetAntiCheatEncryption(IEncryptionModule encryptionModule)
	{
		INetConnection[] array = this.netConnection;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetEncryptionModule(encryptionModule);
		}
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x0016FB4B File Offset: 0x0016DD4B
	public bool Equals(ClientInfo _other)
	{
		return this == _other;
	}

	// Token: 0x04002DCF RID: 11727
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClientNumber;

	// Token: 0x04002DD0 RID: 11728
	public INetworkServer network;

	// Token: 0x04002DD1 RID: 11729
	public readonly int ClientNumber;

	// Token: 0x04002DD2 RID: 11730
	public long litenetPeerConnectId = -1L;

	// Token: 0x04002DD3 RID: 11731
	public PlatformUserIdentifierAbs PlatformId = new UserIdentifierLocal("<none>");

	// Token: 0x04002DD4 RID: 11732
	public PlatformUserIdentifierAbs CrossplatformId;

	// Token: 0x04002DD5 RID: 11733
	public ulong DiscordUserId;

	// Token: 0x04002DD6 RID: 11734
	public bool requiresAntiCheat = true;

	// Token: 0x04002DD7 RID: 11735
	public ClientInfo.EDeviceType device = ClientInfo.EDeviceType.Unknown;

	// Token: 0x04002DD8 RID: 11736
	public bool loginDone;

	// Token: 0x04002DD9 RID: 11737
	public bool acAuthDone;

	// Token: 0x04002DDA RID: 11738
	public INetConnection[] netConnection;

	// Token: 0x04002DDB RID: 11739
	public bool bAttachedToEntity;

	// Token: 0x04002DDC RID: 11740
	public int entityId = -1;

	// Token: 0x04002DDD RID: 11741
	public string playerName;

	// Token: 0x04002DDE RID: 11742
	public string compatibilityVersion;

	// Token: 0x04002DDF RID: 11743
	public readonly Dictionary<string, int> groupMemberships = new Dictionary<string, int>(StringComparer.Ordinal);

	// Token: 0x04002DE0 RID: 11744
	public int groupMembershipsWaiting;

	// Token: 0x04002DE1 RID: 11745
	public PlayerDataFile latestPlayerData;

	// Token: 0x04002DE2 RID: 11746
	public int ping;

	// Token: 0x04002DE3 RID: 11747
	public bool disconnecting;

	// Token: 0x02000762 RID: 1890
	public enum EDeviceType
	{
		// Token: 0x04002DE5 RID: 11749
		Linux,
		// Token: 0x04002DE6 RID: 11750
		Mac,
		// Token: 0x04002DE7 RID: 11751
		Windows,
		// Token: 0x04002DE8 RID: 11752
		PlayStation,
		// Token: 0x04002DE9 RID: 11753
		Xbox,
		// Token: 0x04002DEA RID: 11754
		Unknown
	}
}
