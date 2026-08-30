using System;
using System.ComponentModel;
using UnityEngine.Networking.PlayerConnection;

namespace UnityEngine.Diagnostics
{
	// Token: 0x020004B0 RID: 1200
	public static class PlayerConnection
	{
		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060029D6 RID: 10710 RVA: 0x00046E90 File Offset: 0x00045090
		[Obsolete("Use UnityEngine.Networking.PlayerConnection.PlayerConnection.instance.isConnected instead.")]
		public static bool connected
		{
			get
			{
				return PlayerConnection.instance.isConnected;
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("PlayerConnection.SendFile is no longer supported.", true)]
		public static void SendFile(string remoteFilePath, byte[] data)
		{
		}
	}
}
