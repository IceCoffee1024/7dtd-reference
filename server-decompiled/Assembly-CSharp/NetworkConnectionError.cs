using System;

// Token: 0x02000891 RID: 2193
public enum NetworkConnectionError
{
	// Token: 0x04003379 RID: 13177
	InternalDirectConnectFailed = -5,
	// Token: 0x0400337A RID: 13178
	EmptyConnectTarget,
	// Token: 0x0400337B RID: 13179
	IncorrectParameters,
	// Token: 0x0400337C RID: 13180
	CreateSocketOrThreadFailure,
	// Token: 0x0400337D RID: 13181
	AlreadyConnectedToAnotherServer,
	// Token: 0x0400337E RID: 13182
	NoError,
	// Token: 0x0400337F RID: 13183
	ConnectionFailed = 15,
	// Token: 0x04003380 RID: 13184
	AlreadyConnectedToServer,
	// Token: 0x04003381 RID: 13185
	TooManyConnectedPlayers = 18,
	// Token: 0x04003382 RID: 13186
	RSAPublicKeyMismatch = 21,
	// Token: 0x04003383 RID: 13187
	ConnectionBanned,
	// Token: 0x04003384 RID: 13188
	InvalidPassword,
	// Token: 0x04003385 RID: 13189
	NATTargetNotConnected = 69,
	// Token: 0x04003386 RID: 13190
	NATTargetConnectionLost = 71,
	// Token: 0x04003387 RID: 13191
	NATPunchthroughFailed = 73,
	// Token: 0x04003388 RID: 13192
	InvalidPort,
	// Token: 0x04003389 RID: 13193
	RestartRequired
}
