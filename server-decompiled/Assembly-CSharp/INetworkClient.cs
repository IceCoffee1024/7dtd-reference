using System;
using UnityEngine.Networking;

// Token: 0x02000886 RID: 2182
public interface INetworkClient
{
	// Token: 0x06003F01 RID: 16129
	void Update();

	// Token: 0x06003F02 RID: 16130
	void LateUpdate();

	// Token: 0x06003F03 RID: 16131
	void Connect(GameServerInfo _gsi);

	// Token: 0x06003F04 RID: 16132
	void Disconnect();

	// Token: 0x06003F05 RID: 16133
	NetworkError SendData(int _channel, ArrayListMP<byte> _data);

	// Token: 0x06003F06 RID: 16134
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F07 RID: 16135
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F08 RID: 16136
	void EnableStatistics();

	// Token: 0x06003F09 RID: 16137
	void DisableStatistics();

	// Token: 0x06003F0A RID: 16138
	string PrintNetworkStatistics();

	// Token: 0x06003F0B RID: 16139
	void ResetNetworkStatistics();
}
