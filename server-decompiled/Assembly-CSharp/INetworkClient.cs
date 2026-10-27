using System;
using UnityEngine.Networking;

// Token: 0x02000890 RID: 2192
public interface INetworkClient
{
	// Token: 0x06003F24 RID: 16164
	void Update();

	// Token: 0x06003F25 RID: 16165
	void LateUpdate();

	// Token: 0x06003F26 RID: 16166
	void Connect(GameServerInfo _gsi);

	// Token: 0x06003F27 RID: 16167
	void Disconnect();

	// Token: 0x06003F28 RID: 16168
	NetworkError SendData(int _channel, ArrayListMP<byte> _data);

	// Token: 0x06003F29 RID: 16169
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F2A RID: 16170
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F2B RID: 16171
	void EnableStatistics();

	// Token: 0x06003F2C RID: 16172
	void DisableStatistics();

	// Token: 0x06003F2D RID: 16173
	string PrintNetworkStatistics();

	// Token: 0x06003F2E RID: 16174
	void ResetNetworkStatistics();
}
