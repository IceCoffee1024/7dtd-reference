using System;
using System.Text;
using LiteNetLib;

// Token: 0x0200087D RID: 2173
public static class NetworkCommonLiteNetLib
{
	// Token: 0x06003F44 RID: 16196 RVA: 0x0018C9CD File Offset: 0x0018ABCD
	public static bool InitConfig(NetManager _manager)
	{
		_manager.UnsyncedEvents = true;
		_manager.UnsyncedDeliveryEvent = true;
		_manager.UnsyncedReceiveEvent = true;
		_manager.AutoRecycle = true;
		_manager.DisconnectOnUnreachable = true;
		if (GameManager.IsDedicatedServer)
		{
			_manager.UseNativeSockets = true;
		}
		return true;
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x0018CA04 File Offset: 0x0018AC04
	public static byte[] CreateRejectMessage(string _customText)
	{
		int byteCount = Encoding.UTF8.GetByteCount(_customText);
		byte[] array = new byte[2 + byteCount];
		array[0] = byte.MaxValue;
		array[1] = (byte)Encoding.UTF8.GetBytes(_customText, 0, _customText.Length, array, 2);
		return array;
	}

	// Token: 0x040032CE RID: 13006
	public const int PORT_OFFSET = 2;

	// Token: 0x040032CF RID: 13007
	public const int MTU_OVERRIDE = 1024;

	// Token: 0x040032D0 RID: 13008
	public const byte ChallengePackageChannelId = 202;

	// Token: 0x0200087E RID: 2174
	public enum EAdditionalDisconnectCause : byte
	{
		// Token: 0x040032D2 RID: 13010
		InvalidPassword,
		// Token: 0x040032D3 RID: 13011
		RateLimit,
		// Token: 0x040032D4 RID: 13012
		PendingConnection,
		// Token: 0x040032D5 RID: 13013
		ServerShutdown,
		// Token: 0x040032D6 RID: 13014
		ClientSideDisconnect,
		// Token: 0x040032D7 RID: 13015
		Other = 255
	}
}
