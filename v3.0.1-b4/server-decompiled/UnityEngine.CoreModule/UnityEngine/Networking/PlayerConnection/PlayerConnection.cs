using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine.Scripting;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class PlayerConnection : ScriptableObject, IEditorPlayerConnection
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002120 RID: 8480 RVA: 0x00036E64 File Offset: 0x00035064
		public static PlayerConnection instance
		{
			get
			{
				bool flag = PlayerConnection.s_Instance == null;
				PlayerConnection playerConnection;
				if (flag)
				{
					playerConnection = PlayerConnection.CreateInstance();
				}
				else
				{
					playerConnection = PlayerConnection.s_Instance;
				}
				return playerConnection;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x00036E94 File Offset: 0x00035094
		public bool isConnected
		{
			get
			{
				return this.GetConnectionNativeApi().IsConnected();
			}
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00036EB4 File Offset: 0x000350B4
		private static PlayerConnection CreateInstance()
		{
			PlayerConnection.s_Instance = ScriptableObject.CreateInstance<PlayerConnection>();
			PlayerConnection.s_Instance.hideFlags = HideFlags.HideAndDontSave;
			return PlayerConnection.s_Instance;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00036EE4 File Offset: 0x000350E4
		public void OnEnable()
		{
			bool isInitilized = this.m_IsInitilized;
			if (!isInitilized)
			{
				this.m_IsInitilized = true;
				this.GetConnectionNativeApi().Initialize();
			}
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00036F14 File Offset: 0x00035114
		private IPlayerEditorConnectionNative GetConnectionNativeApi()
		{
			return PlayerConnection.connectionNative ?? new PlayerConnectionInternal();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00036F34 File Offset: 0x00035134
		public void Register(Guid messageId, UnityAction<MessageEventArgs> callback)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("Cant be Guid.Empty", "messageId");
			}
			bool flag2 = !this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			if (flag2)
			{
				this.GetConnectionNativeApi().RegisterInternal(messageId);
			}
			this.m_PlayerEditorConnectionEvents.AddAndCreate(messageId).AddListener(callback);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00036FC4 File Offset: 0x000351C4
		public void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback)
		{
			this.m_PlayerEditorConnectionEvents.UnregisterManagedCallback(messageId, callback);
			bool flag = !this.m_PlayerEditorConnectionEvents.messageTypeSubscribers.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>((PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			if (flag)
			{
				this.GetConnectionNativeApi().UnregisterInternal(messageId);
			}
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0003702C File Offset: 0x0003522C
		public void RegisterConnection(UnityAction<int> callback)
		{
			foreach (int num in this.m_connectedPlayers)
			{
				callback(num);
			}
			this.m_PlayerEditorConnectionEvents.connectionEvent.AddListener(callback);
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00037098 File Offset: 0x00035298
		public void RegisterDisconnection(UnityAction<int> callback)
		{
			this.m_PlayerEditorConnectionEvents.disconnectionEvent.AddListener(callback);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000370AD File Offset: 0x000352AD
		public void UnregisterConnection(UnityAction<int> callback)
		{
			this.m_PlayerEditorConnectionEvents.connectionEvent.RemoveListener(callback);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000370C2 File Offset: 0x000352C2
		public void UnregisterDisconnection(UnityAction<int> callback)
		{
			this.m_PlayerEditorConnectionEvents.disconnectionEvent.RemoveListener(callback);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000370D8 File Offset: 0x000352D8
		public void Send(Guid messageId, byte[] data)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("Cant be Guid.Empty", "messageId");
			}
			this.GetConnectionNativeApi().SendMessage(messageId, data, 0);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00037118 File Offset: 0x00035318
		public bool TrySend(Guid messageId, byte[] data)
		{
			bool flag = messageId == Guid.Empty;
			if (flag)
			{
				throw new ArgumentException("Cant be Guid.Empty", "messageId");
			}
			return this.GetConnectionNativeApi().TrySendMessage(messageId, data, 0);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x00037158 File Offset: 0x00035358
		public bool BlockUntilRecvMsg(Guid messageId, int timeout)
		{
			bool msgReceived = false;
			UnityAction<MessageEventArgs> unityAction = delegate(MessageEventArgs args)
			{
				msgReceived = true;
			};
			DateTime now = DateTime.Now;
			this.Register(messageId, unityAction);
			while ((DateTime.Now - now).TotalMilliseconds < (double)timeout && !msgReceived)
			{
				this.GetConnectionNativeApi().Poll();
			}
			this.Unregister(messageId, unityAction);
			return msgReceived;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000371DA File Offset: 0x000353DA
		public void DisconnectAll()
		{
			this.GetConnectionNativeApi().DisconnectAll();
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x000371EC File Offset: 0x000353EC
		[RequiredByNativeCode]
		private static void MessageCallbackInternal(IntPtr data, ulong size, ulong guid, string messageId)
		{
			byte[] array = null;
			bool flag = size > 0UL;
			if (flag)
			{
				array = new byte[size];
				Marshal.Copy(data, array, 0, (int)size);
			}
			PlayerConnection.instance.m_PlayerEditorConnectionEvents.InvokeMessageIdSubscribers(new Guid(messageId), array, (int)guid);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00037233 File Offset: 0x00035433
		[RequiredByNativeCode]
		private static void ConnectedCallbackInternal(int playerId)
		{
			PlayerConnection.instance.m_connectedPlayers.Add(playerId);
			PlayerConnection.instance.m_PlayerEditorConnectionEvents.connectionEvent.Invoke(playerId);
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0003725D File Offset: 0x0003545D
		[RequiredByNativeCode]
		private static void DisconnectedCallback(int playerId)
		{
			PlayerConnection.instance.m_connectedPlayers.Remove(playerId);
			PlayerConnection.instance.m_PlayerEditorConnectionEvents.disconnectionEvent.Invoke(playerId);
		}

		// Token: 0x04000AFF RID: 2815
		internal static IPlayerEditorConnectionNative connectionNative;

		// Token: 0x04000B00 RID: 2816
		[SerializeField]
		private PlayerEditorConnectionEvents m_PlayerEditorConnectionEvents = new PlayerEditorConnectionEvents();

		// Token: 0x04000B01 RID: 2817
		[SerializeField]
		private List<int> m_connectedPlayers = new List<int>();

		// Token: 0x04000B02 RID: 2818
		private bool m_IsInitilized;

		// Token: 0x04000B03 RID: 2819
		private static PlayerConnection s_Instance;
	}
}
