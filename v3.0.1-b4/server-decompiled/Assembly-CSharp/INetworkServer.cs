using System;
using UnityEngine.Networking;

// Token: 0x02000878 RID: 2168
public interface INetworkServer
{
	// Token: 0x06003F19 RID: 16153
	void Update();

	// Token: 0x06003F1A RID: 16154
	void LateUpdate();

	// Token: 0x06003F1B RID: 16155
	NetworkConnectionError StartServer(int _basePort, string _password);

	// Token: 0x06003F1C RID: 16156
	void SetServerPassword(string _password);

	// Token: 0x06003F1D RID: 16157
	void StopServer();

	// Token: 0x06003F1E RID: 16158
	void DropClient(ClientInfo _clientInfo, bool _clientDisconnect);

	// Token: 0x06003F1F RID: 16159
	NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool reliableDelivery = true);

	// Token: 0x06003F20 RID: 16160
	string GetIP(ClientInfo _cInfo);

	// Token: 0x06003F21 RID: 16161
	int GetPing(ClientInfo _cInfo);

	// Token: 0x06003F22 RID: 16162
	string GetServerPorts(int _basePort);

	// Token: 0x06003F23 RID: 16163
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F24 RID: 16164
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F25 RID: 16165
	void EnableStatistics();

	// Token: 0x06003F26 RID: 16166
	void DisableStatistics();

	// Token: 0x06003F27 RID: 16167
	string PrintNetworkStatistics();

	// Token: 0x06003F28 RID: 16168
	void ResetNetworkStatistics();

	// Token: 0x06003F29 RID: 16169
	int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false);

	// Token: 0x06003F2A RID: 16170
	int GetBadPacketCount(ClientInfo _cInfo);
}
