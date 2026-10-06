using System;
using System.Collections.Generic;
using Platform.Local;
using Platform.Steam;

// Token: 0x0200076D RID: 1901
public class ClientInfo : IEquatable<ClientInfo>
{
	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06003893 RID: 14483 RVA: 0x001724D5 File Offset: 0x001706D5
	public PlatformUserIdentifierAbs InternalId
	{
		get
		{
			return this.CrossplatformId ?? this.PlatformId;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06003894 RID: 14484 RVA: 0x001724E7 File Offset: 0x001706E7
	public string ip
	{
		get
		{
			return this.network.GetIP(this);
		}
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x001724F8 File Offset: 0x001706F8
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

	// Token: 0x06003896 RID: 14486 RVA: 0x00172580 File Offset: 0x00170780
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

	// Token: 0x06003897 RID: 14487 RVA: 0x0017262F File Offset: 0x0017082F
	public void UpdatePing()
	{
		this.ping = this.network.GetPing(this);
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x00172644 File Offset: 0x00170844
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

	// Token: 0x06003899 RID: 14489 RVA: 0x001726A0 File Offset: 0x001708A0
	public void SetAntiCheatEncryption(IEncryptionModule encryptionModule)
	{
		INetConnection[] array = this.netConnection;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetEncryptionModule(encryptionModule);
		}
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x001726CB File Offset: 0x001708CB
	public bool Equals(ClientInfo _other)
	{
		return this == _other;
	}

	// Token: 0x04002E40 RID: 11840
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClientNumber;

	// Token: 0x04002E41 RID: 11841
	public INetworkServer network;

	// Token: 0x04002E42 RID: 11842
	public readonly int ClientNumber;

	// Token: 0x04002E43 RID: 11843
	public long litenetPeerConnectId = -1L;

	// Token: 0x04002E44 RID: 11844
	public PlatformUserIdentifierAbs PlatformId = new UserIdentifierLocal("<none>");

	// Token: 0x04002E45 RID: 11845
	public PlatformUserIdentifierAbs CrossplatformId;

	// Token: 0x04002E46 RID: 11846
	public ulong DiscordUserId;

	// Token: 0x04002E47 RID: 11847
	public bool requiresAntiCheat = true;

	// Token: 0x04002E48 RID: 11848
	public ClientInfo.EDeviceType device = ClientInfo.EDeviceType.Unknown;

	// Token: 0x04002E49 RID: 11849
	public bool loginDone;

	// Token: 0x04002E4A RID: 11850
	public bool acAuthDone;

	// Token: 0x04002E4B RID: 11851
	public INetConnection[] netConnection;

	// Token: 0x04002E4C RID: 11852
	public bool bAttachedToEntity;

	// Token: 0x04002E4D RID: 11853
	public int entityId = -1;

	// Token: 0x04002E4E RID: 11854
	public string playerName;

	// Token: 0x04002E4F RID: 11855
	public string compatibilityVersion;

	// Token: 0x04002E50 RID: 11856
	public readonly Dictionary<string, int> groupMemberships = new Dictionary<string, int>(StringComparer.Ordinal);

	// Token: 0x04002E51 RID: 11857
	public int groupMembershipsWaiting;

	// Token: 0x04002E52 RID: 11858
	public PlayerDataFile latestPlayerData;

	// Token: 0x04002E53 RID: 11859
	public int ping;

	// Token: 0x04002E54 RID: 11860
	public bool disconnecting;

	// Token: 0x0200076E RID: 1902
	public enum EDeviceType
	{
		// Token: 0x04002E56 RID: 11862
		Linux,
		// Token: 0x04002E57 RID: 11863
		Mac,
		// Token: 0x04002E58 RID: 11864
		Windows,
		// Token: 0x04002E59 RID: 11865
		PlayStation,
		// Token: 0x04002E5A RID: 11866
		Xbox,
		// Token: 0x04002E5B RID: 11867
		Unknown
	}
}
