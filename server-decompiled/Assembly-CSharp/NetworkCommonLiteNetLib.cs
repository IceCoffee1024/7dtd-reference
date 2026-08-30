using System;
using System.Text;
using LiteNetLib;

// Token: 0x02000889 RID: 2185
public static class NetworkCommonLiteNetLib
{
	// Token: 0x06003FA9 RID: 16297 RVA: 0x0018EA9D File Offset: 0x0018CC9D
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

	// Token: 0x06003FAA RID: 16298 RVA: 0x0018EAD4 File Offset: 0x0018CCD4
	public static byte[] CreateRejectMessage(string _customText)
	{
		int byteCount = Encoding.UTF8.GetByteCount(_customText);
		byte[] array = new byte[2 + byteCount];
		array[0] = byte.MaxValue;
		array[1] = (byte)Encoding.UTF8.GetBytes(_customText, 0, _customText.Length, array, 2);
		return array;
	}

	// Token: 0x0400334F RID: 13135
	public const int PORT_OFFSET = 2;

	// Token: 0x04003350 RID: 13136
	public const int MTU_OVERRIDE = 1024;

	// Token: 0x04003351 RID: 13137
	public const byte ChallengePackageChannelId = 202;

	// Token: 0x0200088A RID: 2186
	public enum EAdditionalDisconnectCause : byte
	{
		// Token: 0x04003353 RID: 13139
		InvalidPassword,
		// Token: 0x04003354 RID: 13140
		RateLimit,
		// Token: 0x04003355 RID: 13141
		PendingConnection,
		// Token: 0x04003356 RID: 13142
		ServerShutdown,
		// Token: 0x04003357 RID: 13143
		ClientSideDisconnect,
		// Token: 0x04003358 RID: 13144
		Other = 255
	}
}
