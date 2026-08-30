using System;
using UnityEngine.Networking;

// Token: 0x02000877 RID: 2167
public interface INetworkClient
{
	// Token: 0x06003F0E RID: 16142
	void Update();

	// Token: 0x06003F0F RID: 16143
	void LateUpdate();

	// Token: 0x06003F10 RID: 16144
	void Connect(GameServerInfo _gsi);

	// Token: 0x06003F11 RID: 16145
	void Disconnect();

	// Token: 0x06003F12 RID: 16146
	NetworkError SendData(int _channel, ArrayListMP<byte> _data);

	// Token: 0x06003F13 RID: 16147
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F14 RID: 16148
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F15 RID: 16149
	void EnableStatistics();

	// Token: 0x06003F16 RID: 16150
	void DisableStatistics();

	// Token: 0x06003F17 RID: 16151
	string PrintNetworkStatistics();

	// Token: 0x06003F18 RID: 16152
	void ResetNetworkStatistics();
}
