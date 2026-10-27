using System;
using System.Collections.Generic;
using Platform.Local;
using Platform.Steam;

// Token: 0x02000775 RID: 1909
public class ClientInfo : IEquatable<ClientInfo>
{
	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x060038AA RID: 14506 RVA: 0x0017346D File Offset: 0x0017166D
	public PlatformUserIdentifierAbs InternalId
	{
		get
		{
			return this.CrossplatformId ?? this.PlatformId;
		}
	}

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x060038AB RID: 14507 RVA: 0x0017347F File Offset: 0x0017167F
	public string ip
	{
		get
		{
			return this.network.GetIP(this);
		}
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x00173490 File Offset: 0x00171690
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

	// Token: 0x060038AD RID: 14509 RVA: 0x00173518 File Offset: 0x00171718
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

	// Token: 0x060038AE RID: 14510 RVA: 0x001735C7 File Offset: 0x001717C7
	public void UpdatePing()
	{
		this.ping = this.network.GetPing(this);
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x001735DC File Offset: 0x001717DC
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

	// Token: 0x060038B0 RID: 14512 RVA: 0x00173638 File Offset: 0x00171838
	public void SetAntiCheatEncryption(IEncryptionModule encryptionModule)
	{
		INetConnection[] array = this.netConnection;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetEncryptionModule(encryptionModule);
		}
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x00173663 File Offset: 0x00171863
	public bool Equals(ClientInfo _other)
	{
		return this == _other;
	}

	// Token: 0x04002E5C RID: 11868
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastClientNumber;

	// Token: 0x04002E5D RID: 11869
	public INetworkServer network;

	// Token: 0x04002E5E RID: 11870
	public readonly int ClientNumber;

	// Token: 0x04002E5F RID: 11871
	public long litenetPeerConnectId = -1L;

	// Token: 0x04002E60 RID: 11872
	public PlatformUserIdentifierAbs PlatformId = new UserIdentifierLocal("<none>");

	// Token: 0x04002E61 RID: 11873
	public PlatformUserIdentifierAbs CrossplatformId;

	// Token: 0x04002E62 RID: 11874
	public ulong DiscordUserId;

	// Token: 0x04002E63 RID: 11875
	public bool requiresAntiCheat = true;

	// Token: 0x04002E64 RID: 11876
	public ClientInfo.EDeviceType device = ClientInfo.EDeviceType.Unknown;

	// Token: 0x04002E65 RID: 11877
	public bool loginDone;

	// Token: 0x04002E66 RID: 11878
	public bool acAuthDone;

	// Token: 0x04002E67 RID: 11879
	public INetConnection[] netConnection;

	// Token: 0x04002E68 RID: 11880
	public bool bAttachedToEntity;

	// Token: 0x04002E69 RID: 11881
	public int entityId = -1;

	// Token: 0x04002E6A RID: 11882
	public string playerName;

	// Token: 0x04002E6B RID: 11883
	public string compatibilityVersion;

	// Token: 0x04002E6C RID: 11884
	public readonly Dictionary<string, int> groupMemberships = new Dictionary<string, int>(StringComparer.Ordinal);

	// Token: 0x04002E6D RID: 11885
	public int groupMembershipsWaiting;

	// Token: 0x04002E6E RID: 11886
	public PlayerDataFile latestPlayerData;

	// Token: 0x04002E6F RID: 11887
	public int ping;

	// Token: 0x04002E70 RID: 11888
	public bool disconnecting;

	// Token: 0x02000776 RID: 1910
	public enum EDeviceType
	{
		// Token: 0x04002E72 RID: 11890
		Linux,
		// Token: 0x04002E73 RID: 11891
		Mac,
		// Token: 0x04002E74 RID: 11892
		Windows,
		// Token: 0x04002E75 RID: 11893
		PlayStation,
		// Token: 0x04002E76 RID: 11894
		Xbox,
		// Token: 0x04002E77 RID: 11895
		Unknown
	}
}
