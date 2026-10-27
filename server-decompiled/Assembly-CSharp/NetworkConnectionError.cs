using System;

// Token: 0x0200089E RID: 2206
public enum NetworkConnectionError
{
	// Token: 0x04003384 RID: 13188
	InternalDirectConnectFailed = -5,
	// Token: 0x04003385 RID: 13189
	EmptyConnectTarget,
	// Token: 0x04003386 RID: 13190
	IncorrectParameters,
	// Token: 0x04003387 RID: 13191
	CreateSocketOrThreadFailure,
	// Token: 0x04003388 RID: 13192
	AlreadyConnectedToAnotherServer,
	// Token: 0x04003389 RID: 13193
	NoError,
	// Token: 0x0400338A RID: 13194
	ConnectionFailed = 15,
	// Token: 0x0400338B RID: 13195
	AlreadyConnectedToServer,
	// Token: 0x0400338C RID: 13196
	TooManyConnectedPlayers = 18,
	// Token: 0x0400338D RID: 13197
	RSAPublicKeyMismatch = 21,
	// Token: 0x0400338E RID: 13198
	ConnectionBanned,
	// Token: 0x0400338F RID: 13199
	InvalidPassword,
	// Token: 0x04003390 RID: 13200
	NATTargetNotConnected = 69,
	// Token: 0x04003391 RID: 13201
	NATTargetConnectionLost = 71,
	// Token: 0x04003392 RID: 13202
	NATPunchthroughFailed = 73,
	// Token: 0x04003393 RID: 13203
	InvalidPort,
	// Token: 0x04003394 RID: 13204
	RestartRequired
}
