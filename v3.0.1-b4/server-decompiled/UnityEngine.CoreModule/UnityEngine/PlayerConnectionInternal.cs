using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000206 RID: 518
	[NativeHeader("Runtime/Export/PlayerConnection/PlayerConnectionInternal.bindings.h")]
	internal class PlayerConnectionInternal : IPlayerEditorConnectionNative
	{
		// Token: 0x06001770 RID: 6000 RVA: 0x00027234 File Offset: 0x00025434
		void IPlayerEditorConnectionNative.SendMessage(Guid messageId, byte[] data, int playerId)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			PlayerConnectionInternal.SendMessage(messageId.ToString("N"), data, playerId);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00027274 File Offset: 0x00025474
		bool IPlayerEditorConnectionNative.TrySendMessage(Guid messageId, byte[] data, int playerId)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			return PlayerConnectionInternal.TrySendMessage(messageId.ToString("N"), data, playerId);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000272B4 File Offset: 0x000254B4
		void IPlayerEditorConnectionNative.Poll()
		{
			PlayerConnectionInternal.PollInternal();
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000272BD File Offset: 0x000254BD
		void IPlayerEditorConnectionNative.RegisterInternal(Guid messageId)
		{
			PlayerConnectionInternal.RegisterInternal(messageId.ToString("N"));
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000272D2 File Offset: 0x000254D2
		void IPlayerEditorConnectionNative.UnregisterInternal(Guid messageId)
		{
			PlayerConnectionInternal.UnregisterInternal(messageId.ToString("N"));
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000272E7 File Offset: 0x000254E7
		void IPlayerEditorConnectionNative.Initialize()
		{
			PlayerConnectionInternal.Initialize();
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000272F0 File Offset: 0x000254F0
		bool IPlayerEditorConnectionNative.IsConnected()
		{
			return PlayerConnectionInternal.IsConnected();
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00027307 File Offset: 0x00025507
		void IPlayerEditorConnectionNative.DisconnectAll()
		{
			PlayerConnectionInternal.DisconnectAll();
		}

		// Token: 0x06001778 RID: 6008
		[FreeFunction("PlayerConnection_Bindings::IsConnected")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsConnected();

		// Token: 0x06001779 RID: 6009
		[FreeFunction("PlayerConnection_Bindings::Initialize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Initialize();

		// Token: 0x0600177A RID: 6010
		[FreeFunction("PlayerConnection_Bindings::RegisterInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterInternal(string messageId);

		// Token: 0x0600177B RID: 6011
		[FreeFunction("PlayerConnection_Bindings::UnregisterInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnregisterInternal(string messageId);

		// Token: 0x0600177C RID: 6012
		[FreeFunction("PlayerConnection_Bindings::SendMessage")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendMessage(string messageId, [Unmarshalled] byte[] data, int playerId);

		// Token: 0x0600177D RID: 6013
		[FreeFunction("PlayerConnection_Bindings::TrySendMessage")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TrySendMessage(string messageId, [Unmarshalled] byte[] data, int playerId);

		// Token: 0x0600177E RID: 6014
		[FreeFunction("PlayerConnection_Bindings::PollInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PollInternal();

		// Token: 0x0600177F RID: 6015
		[FreeFunction("PlayerConnection_Bindings::DisconnectAll")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisconnectAll();

		// Token: 0x02000207 RID: 519
		[Flags]
		public enum MulticastFlags
		{
			// Token: 0x0400086C RID: 2156
			kRequestImmediateConnect = 1,
			// Token: 0x0400086D RID: 2157
			kSupportsProfile = 2,
			// Token: 0x0400086E RID: 2158
			kCustomMessage = 4,
			// Token: 0x0400086F RID: 2159
			kUseAlternateIP = 8
		}
	}
}
