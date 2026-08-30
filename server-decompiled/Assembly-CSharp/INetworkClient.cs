using System;
using UnityEngine.Networking;

// Token: 0x02000883 RID: 2179
public interface INetworkClient
{
	// Token: 0x06003F73 RID: 16243
	void Update();

	// Token: 0x06003F74 RID: 16244
	void LateUpdate();

	// Token: 0x06003F75 RID: 16245
	void Connect(GameServerInfo _gsi);

	// Token: 0x06003F76 RID: 16246
	void Disconnect();

	// Token: 0x06003F77 RID: 16247
	NetworkError SendData(int _channel, ArrayListMP<byte> _data);

	// Token: 0x06003F78 RID: 16248
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F79 RID: 16249
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F7A RID: 16250
	void EnableStatistics();

	// Token: 0x06003F7B RID: 16251
	void DisableStatistics();

	// Token: 0x06003F7C RID: 16252
	string PrintNetworkStatistics();

	// Token: 0x06003F7D RID: 16253
	void ResetNetworkStatistics();
}
