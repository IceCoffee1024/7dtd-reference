using System;
using UnityEngine.Networking;

// Token: 0x02000891 RID: 2193
public interface INetworkServer
{
	// Token: 0x06003F2F RID: 16175
	void Update();

	// Token: 0x06003F30 RID: 16176
	void LateUpdate();

	// Token: 0x06003F31 RID: 16177
	NetworkConnectionError StartServer(int _basePort, string _password);

	// Token: 0x06003F32 RID: 16178
	void SetServerPassword(string _password);

	// Token: 0x06003F33 RID: 16179
	void StopServer();

	// Token: 0x06003F34 RID: 16180
	void DropClient(ClientInfo _clientInfo, bool _clientDisconnect);

	// Token: 0x06003F35 RID: 16181
	NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool reliableDelivery = true);

	// Token: 0x06003F36 RID: 16182
	string GetIP(ClientInfo _cInfo);

	// Token: 0x06003F37 RID: 16183
	int GetPing(ClientInfo _cInfo);

	// Token: 0x06003F38 RID: 16184
	string GetServerPorts(int _basePort);

	// Token: 0x06003F39 RID: 16185
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F3A RID: 16186
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F3B RID: 16187
	void EnableStatistics();

	// Token: 0x06003F3C RID: 16188
	void DisableStatistics();

	// Token: 0x06003F3D RID: 16189
	string PrintNetworkStatistics();

	// Token: 0x06003F3E RID: 16190
	void ResetNetworkStatistics();

	// Token: 0x06003F3F RID: 16191
	int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false);

	// Token: 0x06003F40 RID: 16192
	int GetBadPacketCount(ClientInfo _cInfo);
}
