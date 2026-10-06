using System;
using UnityEngine.Networking;

// Token: 0x02000887 RID: 2183
public interface INetworkServer
{
	// Token: 0x06003F0C RID: 16140
	void Update();

	// Token: 0x06003F0D RID: 16141
	void LateUpdate();

	// Token: 0x06003F0E RID: 16142
	NetworkConnectionError StartServer(int _basePort, string _password);

	// Token: 0x06003F0F RID: 16143
	void SetServerPassword(string _password);

	// Token: 0x06003F10 RID: 16144
	void StopServer();

	// Token: 0x06003F11 RID: 16145
	void DropClient(ClientInfo _clientInfo, bool _clientDisconnect);

	// Token: 0x06003F12 RID: 16146
	NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool reliableDelivery = true);

	// Token: 0x06003F13 RID: 16147
	string GetIP(ClientInfo _cInfo);

	// Token: 0x06003F14 RID: 16148
	int GetPing(ClientInfo _cInfo);

	// Token: 0x06003F15 RID: 16149
	string GetServerPorts(int _basePort);

	// Token: 0x06003F16 RID: 16150
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F17 RID: 16151
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F18 RID: 16152
	void EnableStatistics();

	// Token: 0x06003F19 RID: 16153
	void DisableStatistics();

	// Token: 0x06003F1A RID: 16154
	string PrintNetworkStatistics();

	// Token: 0x06003F1B RID: 16155
	void ResetNetworkStatistics();

	// Token: 0x06003F1C RID: 16156
	int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false);

	// Token: 0x06003F1D RID: 16157
	int GetBadPacketCount(ClientInfo _cInfo);
}
