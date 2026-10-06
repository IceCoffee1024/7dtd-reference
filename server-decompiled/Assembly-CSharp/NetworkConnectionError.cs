using System;

// Token: 0x02000894 RID: 2196
public enum NetworkConnectionError
{
	// Token: 0x04003364 RID: 13156
	InternalDirectConnectFailed = -5,
	// Token: 0x04003365 RID: 13157
	EmptyConnectTarget,
	// Token: 0x04003366 RID: 13158
	IncorrectParameters,
	// Token: 0x04003367 RID: 13159
	CreateSocketOrThreadFailure,
	// Token: 0x04003368 RID: 13160
	AlreadyConnectedToAnotherServer,
	// Token: 0x04003369 RID: 13161
	NoError,
	// Token: 0x0400336A RID: 13162
	ConnectionFailed = 15,
	// Token: 0x0400336B RID: 13163
	AlreadyConnectedToServer,
	// Token: 0x0400336C RID: 13164
	TooManyConnectedPlayers = 18,
	// Token: 0x0400336D RID: 13165
	RSAPublicKeyMismatch = 21,
	// Token: 0x0400336E RID: 13166
	ConnectionBanned,
	// Token: 0x0400336F RID: 13167
	InvalidPassword,
	// Token: 0x04003370 RID: 13168
	NATTargetNotConnected = 69,
	// Token: 0x04003371 RID: 13169
	NATTargetConnectionLost = 71,
	// Token: 0x04003372 RID: 13170
	NATPunchthroughFailed = 73,
	// Token: 0x04003373 RID: 13171
	InvalidPort,
	// Token: 0x04003374 RID: 13172
	RestartRequired
}
