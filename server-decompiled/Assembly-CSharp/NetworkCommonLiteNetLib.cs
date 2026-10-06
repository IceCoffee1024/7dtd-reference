using System;
using System.Text;
using LiteNetLib;

// Token: 0x0200088C RID: 2188
public static class NetworkCommonLiteNetLib
{
	// Token: 0x06003F37 RID: 16183 RVA: 0x0018E88D File Offset: 0x0018CA8D
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

	// Token: 0x06003F38 RID: 16184 RVA: 0x0018E8C4 File Offset: 0x0018CAC4
	public static byte[] CreateRejectMessage(string _customText)
	{
		int byteCount = Encoding.UTF8.GetByteCount(_customText);
		byte[] array = new byte[2 + byteCount];
		array[0] = byte.MaxValue;
		array[1] = (byte)Encoding.UTF8.GetBytes(_customText, 0, _customText.Length, array, 2);
		return array;
	}

	// Token: 0x0400333A RID: 13114
	public const int PORT_OFFSET = 2;

	// Token: 0x0400333B RID: 13115
	public const int MTU_OVERRIDE = 1024;

	// Token: 0x0400333C RID: 13116
	public const byte ChallengePackageChannelId = 202;

	// Token: 0x0200088D RID: 2189
	public enum EAdditionalDisconnectCause : byte
	{
		// Token: 0x0400333E RID: 13118
		InvalidPassword,
		// Token: 0x0400333F RID: 13119
		RateLimit,
		// Token: 0x04003340 RID: 13120
		PendingConnection,
		// Token: 0x04003341 RID: 13121
		ServerShutdown,
		// Token: 0x04003342 RID: 13122
		ClientSideDisconnect,
		// Token: 0x04003343 RID: 13123
		Other = 255
	}
}
