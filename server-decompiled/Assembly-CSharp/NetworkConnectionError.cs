using System;

// Token: 0x02000885 RID: 2181
public enum NetworkConnectionError
{
	// Token: 0x040032F8 RID: 13048
	InternalDirectConnectFailed = -5,
	// Token: 0x040032F9 RID: 13049
	EmptyConnectTarget,
	// Token: 0x040032FA RID: 13050
	IncorrectParameters,
	// Token: 0x040032FB RID: 13051
	CreateSocketOrThreadFailure,
	// Token: 0x040032FC RID: 13052
	AlreadyConnectedToAnotherServer,
	// Token: 0x040032FD RID: 13053
	NoError,
	// Token: 0x040032FE RID: 13054
	ConnectionFailed = 15,
	// Token: 0x040032FF RID: 13055
	AlreadyConnectedToServer,
	// Token: 0x04003300 RID: 13056
	TooManyConnectedPlayers = 18,
	// Token: 0x04003301 RID: 13057
	RSAPublicKeyMismatch = 21,
	// Token: 0x04003302 RID: 13058
	ConnectionBanned,
	// Token: 0x04003303 RID: 13059
	InvalidPassword,
	// Token: 0x04003304 RID: 13060
	NATTargetNotConnected = 69,
	// Token: 0x04003305 RID: 13061
	NATTargetConnectionLost = 71,
	// Token: 0x04003306 RID: 13062
	NATPunchthroughFailed = 73,
	// Token: 0x04003307 RID: 13063
	InvalidPort,
	// Token: 0x04003308 RID: 13064
	RestartRequired
}
