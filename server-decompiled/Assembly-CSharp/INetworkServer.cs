using System;
using UnityEngine.Networking;

// Token: 0x02000884 RID: 2180
public interface INetworkServer
{
	// Token: 0x06003F7E RID: 16254
	void Update();

	// Token: 0x06003F7F RID: 16255
	void LateUpdate();

	// Token: 0x06003F80 RID: 16256
	NetworkConnectionError StartServer(int _basePort, string _password);

	// Token: 0x06003F81 RID: 16257
	void SetServerPassword(string _password);

	// Token: 0x06003F82 RID: 16258
	void StopServer();

	// Token: 0x06003F83 RID: 16259
	void DropClient(ClientInfo _clientInfo, bool _clientDisconnect);

	// Token: 0x06003F84 RID: 16260
	NetworkError SendData(ClientInfo _clientInfo, int _channel, ArrayListMP<byte> _data, bool reliableDelivery = true);

	// Token: 0x06003F85 RID: 16261
	string GetIP(ClientInfo _cInfo);

	// Token: 0x06003F86 RID: 16262
	int GetPing(ClientInfo _cInfo);

	// Token: 0x06003F87 RID: 16263
	string GetServerPorts(int _basePort);

	// Token: 0x06003F88 RID: 16264
	void SetLatencySimulation(bool _enable, int _min, int _max);

	// Token: 0x06003F89 RID: 16265
	void SetPacketLossSimulation(bool _enable, int _chance);

	// Token: 0x06003F8A RID: 16266
	void EnableStatistics();

	// Token: 0x06003F8B RID: 16267
	void DisableStatistics();

	// Token: 0x06003F8C RID: 16268
	string PrintNetworkStatistics();

	// Token: 0x06003F8D RID: 16269
	void ResetNetworkStatistics();

	// Token: 0x06003F8E RID: 16270
	int GetMaximumPacketSize(ClientInfo _cInfo, bool reliable = false);

	// Token: 0x06003F8F RID: 16271
	int GetBadPacketCount(ClientInfo _cInfo);
}
