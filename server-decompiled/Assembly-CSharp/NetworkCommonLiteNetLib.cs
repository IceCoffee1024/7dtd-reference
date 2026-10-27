using System;
using System.Text;
using LiteNetLib;

// Token: 0x02000896 RID: 2198
public static class NetworkCommonLiteNetLib
{
	// Token: 0x06003F5A RID: 16218 RVA: 0x0018F935 File Offset: 0x0018DB35
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

	// Token: 0x06003F5B RID: 16219 RVA: 0x0018F96C File Offset: 0x0018DB6C
	public static byte[] CreateRejectMessage(string _customText)
	{
		int byteCount = Encoding.UTF8.GetByteCount(_customText);
		byte[] array = new byte[2 + byteCount];
		array[0] = byte.MaxValue;
		array[1] = (byte)Encoding.UTF8.GetBytes(_customText, 0, _customText.Length, array, 2);
		return array;
	}

	// Token: 0x0400335A RID: 13146
	public const int PORT_OFFSET = 2;

	// Token: 0x0400335B RID: 13147
	public const int MTU_OVERRIDE = 1024;

	// Token: 0x0400335C RID: 13148
	public const byte ChallengePackageChannelId = 202;

	// Token: 0x02000897 RID: 2199
	public enum EAdditionalDisconnectCause : byte
	{
		// Token: 0x0400335E RID: 13150
		InvalidPassword,
		// Token: 0x0400335F RID: 13151
		RateLimit,
		// Token: 0x04003360 RID: 13152
		PendingConnection,
		// Token: 0x04003361 RID: 13153
		ServerShutdown,
		// Token: 0x04003362 RID: 13154
		ClientSideDisconnect,
		// Token: 0x04003363 RID: 13155
		Other = 255
	}
}
