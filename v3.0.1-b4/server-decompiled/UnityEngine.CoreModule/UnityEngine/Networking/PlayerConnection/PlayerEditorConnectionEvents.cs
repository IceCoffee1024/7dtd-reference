using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x020003D4 RID: 980
	[Serializable]
	internal class PlayerEditorConnectionEvents
	{
		// Token: 0x06002139 RID: 8505 RVA: 0x000372D8 File Offset: 0x000354D8
		public void InvokeMessageIdSubscribers(Guid messageId, byte[] data, int playerId)
		{
			IEnumerable<PlayerEditorConnectionEvents.MessageTypeSubscribers> enumerable = this.messageTypeSubscribers.Where<PlayerEditorConnectionEvents.MessageTypeSubscribers>((PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = !enumerable.Any<PlayerEditorConnectionEvents.MessageTypeSubscribers>();
			if (flag)
			{
				string text = "No actions found for messageId: ";
				Guid messageId2 = messageId;
				Debug.LogError(text + messageId2.ToString());
			}
			else
			{
				MessageEventArgs e = new MessageEventArgs
				{
					playerId = playerId,
					data = data
				};
				foreach (PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers in enumerable)
				{
					messageTypeSubscribers.messageCallback.Invoke(e);
				}
			}
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000373A0 File Offset: 0x000355A0
		public UnityEvent<MessageEventArgs> AddAndCreate(Guid messageId)
		{
			PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = this.messageTypeSubscribers.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>((PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = messageTypeSubscribers == null;
			if (flag)
			{
				messageTypeSubscribers = new PlayerEditorConnectionEvents.MessageTypeSubscribers
				{
					MessageTypeId = messageId,
					messageCallback = new PlayerEditorConnectionEvents.MessageEvent()
				};
				this.messageTypeSubscribers.Add(messageTypeSubscribers);
			}
			messageTypeSubscribers.subscriberCount++;
			return messageTypeSubscribers.messageCallback;
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x00037420 File Offset: 0x00035620
		public void UnregisterManagedCallback(Guid messageId, UnityAction<MessageEventArgs> callback)
		{
			PlayerEditorConnectionEvents.MessageTypeSubscribers messageTypeSubscribers = this.messageTypeSubscribers.SingleOrDefault<PlayerEditorConnectionEvents.MessageTypeSubscribers>((PlayerEditorConnectionEvents.MessageTypeSubscribers x) => x.MessageTypeId == messageId);
			bool flag = messageTypeSubscribers == null;
			if (!flag)
			{
				messageTypeSubscribers.subscriberCount--;
				messageTypeSubscribers.messageCallback.RemoveListener(callback);
				bool flag2 = messageTypeSubscribers.subscriberCount <= 0;
				if (flag2)
				{
					this.messageTypeSubscribers.Remove(messageTypeSubscribers);
				}
			}
		}

		// Token: 0x04000B07 RID: 2823
		[SerializeField]
		public List<PlayerEditorConnectionEvents.MessageTypeSubscribers> messageTypeSubscribers = new List<PlayerEditorConnectionEvents.MessageTypeSubscribers>();

		// Token: 0x04000B08 RID: 2824
		[SerializeField]
		public PlayerEditorConnectionEvents.ConnectionChangeEvent connectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();

		// Token: 0x04000B09 RID: 2825
		[SerializeField]
		public PlayerEditorConnectionEvents.ConnectionChangeEvent disconnectionEvent = new PlayerEditorConnectionEvents.ConnectionChangeEvent();

		// Token: 0x020003D5 RID: 981
		[Serializable]
		public class MessageEvent : UnityEvent<MessageEventArgs>
		{
		}

		// Token: 0x020003D6 RID: 982
		[Serializable]
		public class ConnectionChangeEvent : UnityEvent<int>
		{
		}

		// Token: 0x020003D7 RID: 983
		[Serializable]
		public class MessageTypeSubscribers
		{
			// Token: 0x1700064B RID: 1611
			// (get) Token: 0x0600213F RID: 8511 RVA: 0x000374D4 File Offset: 0x000356D4
			// (set) Token: 0x06002140 RID: 8512 RVA: 0x000374F1 File Offset: 0x000356F1
			public Guid MessageTypeId
			{
				get
				{
					return new Guid(this.m_messageTypeId);
				}
				set
				{
					this.m_messageTypeId = value.ToString();
				}
			}

			// Token: 0x04000B0A RID: 2826
			[SerializeField]
			private string m_messageTypeId;

			// Token: 0x04000B0B RID: 2827
			public int subscriberCount = 0;

			// Token: 0x04000B0C RID: 2828
			public PlayerEditorConnectionEvents.MessageEvent messageCallback = new PlayerEditorConnectionEvents.MessageEvent();
		}
	}
}
