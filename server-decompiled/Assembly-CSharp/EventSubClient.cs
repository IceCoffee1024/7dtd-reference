using System;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000CA0 RID: 3232
public class EventSubClient
{
	// Token: 0x1400008A RID: 138
	// (add) Token: 0x060062A7 RID: 25255 RVA: 0x0027163C File Offset: 0x0026F83C
	// (remove) Token: 0x060062A8 RID: 25256 RVA: 0x00271674 File Offset: 0x0026F874
	public event Action<JObject> OnEventReceived;

	// Token: 0x060062A9 RID: 25257 RVA: 0x002716A9 File Offset: 0x0026F8A9
	public EventSubClient(string broadcasterID, string accessToken, string clientId)
	{
		this.broadcasterUserID = broadcasterID;
		this.accessToken = accessToken;
		this.clientId = clientId;
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x002716E4 File Offset: 0x0026F8E4
	public void Connect()
	{
		if (this.isRunning)
		{
			return;
		}
		this.ws = new ClientWebSocket();
		this.ws.ConnectAsync(this.twitchWsUri, CancellationToken.None).Wait();
		Log.Out("Connected to Twitch EventSub WebSocket");
		this.isRunning = true;
		this.receiveCoroutine = GameManager.Instance.StartCoroutine(this.ReceiveLoopCoroutine());
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x00271747 File Offset: 0x0026F947
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator ReceiveLoopCoroutine()
	{
		byte[] buffer = new byte[8192];
		while (this.isRunning && this.ws.State == WebSocketState.Open)
		{
			ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
			Task<WebSocketReceiveResult> receiveTask = this.ws.ReceiveAsync(segment, CancellationToken.None);
			while (!receiveTask.IsCompleted)
			{
				yield return null;
			}
			WebSocketReceiveResult webSocketReceiveResult = receiveTask.Result;
			if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
			{
				Log.Out("WebSocket closed by server.");
				this.isRunning = false;
				yield break;
			}
			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, webSocketReceiveResult.Count));
			while (!webSocketReceiveResult.EndOfMessage)
			{
				receiveTask = this.ws.ReceiveAsync(segment, CancellationToken.None);
				while (!receiveTask.IsCompleted)
				{
					yield return null;
				}
				webSocketReceiveResult = receiveTask.Result;
				messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, webSocketReceiveResult.Count));
			}
			string text = messageBuilder.ToString();
			Log.Out("Received raw message: " + text);
			this.HandleMessage(text);
			segment = default(ArraySegment<byte>);
			receiveTask = null;
			messageBuilder = null;
		}
		yield break;
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x00271758 File Offset: 0x0026F958
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleMessage(string json)
	{
		EventSubMessage eventSubMessage = JsonConvert.DeserializeObject<EventSubMessage>(json);
		if (eventSubMessage == null)
		{
			Log.Warning("Invalid EventSub message received.");
			return;
		}
		string messageType = eventSubMessage.Metadata.MessageType;
		if (messageType == "session_welcome")
		{
			JToken jtoken = eventSubMessage.Payload["session"];
			string text;
			if (jtoken == null)
			{
				text = null;
			}
			else
			{
				JToken jtoken2 = jtoken["id"];
				text = ((jtoken2 != null) ? jtoken2.ToString() : null);
			}
			this.sessionID = text ?? string.Empty;
			Log.Out("Session ID: " + this.sessionID);
			GameManager.Instance.StartCoroutine(this.SubscribeToEvents());
			return;
		}
		if (!(messageType == "notification"))
		{
			if (!(messageType == "session_keepalive"))
			{
				if (!(messageType == "session_reconnect"))
				{
					Log.Out("Unhandled message type: " + eventSubMessage.Metadata.MessageType);
				}
				else
				{
					JToken jtoken3 = eventSubMessage.Payload["session"];
					string text2;
					if (jtoken3 == null)
					{
						text2 = null;
					}
					else
					{
						JToken jtoken4 = jtoken3["reconnect_url"];
						text2 = ((jtoken4 != null) ? jtoken4.ToString() : null);
					}
					string text3 = text2;
					if (!string.IsNullOrEmpty(text3))
					{
						Log.Out("Reconnecting to: " + text3);
						this.Reconnect(text3);
						return;
					}
				}
				return;
			}
			Log.Out("Received Keep-Alive");
			return;
		}
		else
		{
			Action<JObject> onEventReceived = this.OnEventReceived;
			if (onEventReceived == null)
			{
				return;
			}
			onEventReceived(eventSubMessage.Payload);
			return;
		}
	}

	// Token: 0x060062AD RID: 25261 RVA: 0x002718B5 File Offset: 0x0026FAB5
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SubscribeToEvents()
	{
		yield return this.CreateEventSubSubscription("channel.subscribe");
		yield return this.CreateEventSubSubscription("channel.channel_points_custom_reward_redemption.add");
		yield return this.CreateEventSubSubscription("channel.subscription.message");
		yield return this.CreateEventSubSubscription("channel.subscription.gift");
		yield return this.CreateEventSubSubscription("channel.bits.use");
		yield return this.CreateEventSubSubscription("channel.hype_train.begin");
		yield return this.CreateEventSubSubscription("channel.hype_train.progress");
		yield return this.CreateEventSubSubscription("channel.hype_train.end");
		yield break;
	}

	// Token: 0x060062AE RID: 25262 RVA: 0x002718C4 File Offset: 0x0026FAC4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator CreateEventSubSubscription(string eventType)
	{
		JObject jobject = new JObject();
		jobject["type"] = eventType;
		jobject["version"] = (eventType.Contains("hype_train") ? "2" : "1");
		string text = "condition";
		JObject jobject2 = new JObject();
		jobject2["broadcaster_user_id"] = this.broadcasterUserID;
		jobject[text] = jobject2;
		string text2 = "transport";
		JObject jobject3 = new JObject();
		jobject3["method"] = "websocket";
		jobject3["session_id"] = this.sessionID;
		jobject[text2] = jobject3;
		string text3 = jobject.ToString();
		byte[] bytes = Encoding.UTF8.GetBytes(text3);
		UnityWebRequest req = new UnityWebRequest("https://api.twitch.tv/helix/eventsub/subscriptions", "POST");
		req.uploadHandler = new UploadHandlerRaw(bytes);
		req.downloadHandler = new DownloadHandlerBuffer();
		req.SetRequestHeader("Content-Type", "application/json");
		req.SetRequestHeader("Authorization", "Bearer " + this.accessToken);
		req.SetRequestHeader("Client-Id", this.clientId);
		yield return req.SendWebRequest();
		if (req.result == UnityWebRequest.Result.Success)
		{
			Log.Out("Successfully subscribed to " + eventType);
		}
		else
		{
			Log.Warning(string.Concat(new string[]
			{
				"Failed to subscribe to ",
				eventType,
				": ",
				req.error,
				" | ",
				req.downloadHandler.text
			}));
		}
		yield break;
	}

	// Token: 0x060062AF RID: 25263 RVA: 0x002718DC File Offset: 0x0026FADC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reconnect(string newUrl)
	{
		if (!this.isRunning)
		{
			return;
		}
		this.Disconnect();
		this.ws = new ClientWebSocket();
		this.ws.ConnectAsync(new Uri(newUrl), CancellationToken.None).Wait();
		Log.Out("Reconnected to new WebSocket URL");
		this.isRunning = true;
		this.receiveCoroutine = GameManager.Instance.StartCoroutine(this.ReceiveLoopCoroutine());
	}

	// Token: 0x060062B0 RID: 25264 RVA: 0x00271948 File Offset: 0x0026FB48
	public void Disconnect()
	{
		if (!this.isRunning)
		{
			return;
		}
		this.isRunning = false;
		if (this.receiveCoroutine != null)
		{
			GameManager.Instance.StopCoroutine(this.receiveCoroutine);
		}
		if (this.ws.State == WebSocketState.Open)
		{
			this.ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None).Wait();
		}
		Log.Out("Disconnected from Twitch EventSub WebSocket");
	}

	// Token: 0x060062B1 RID: 25265 RVA: 0x002719B4 File Offset: 0x0026FBB4
	public void Cleanup()
	{
		if (this.cleanedUp)
		{
			return;
		}
		this.cleanedUp = true;
		this.Disconnect();
		ClientWebSocket clientWebSocket = this.ws;
		if (clientWebSocket != null)
		{
			clientWebSocket.Dispose();
		}
		Log.Out("EventSubClient resources cleaned up.");
	}

	// Token: 0x04004D05 RID: 19717
	[PublicizedFrom(EAccessModifier.Private)]
	public ClientWebSocket ws;

	// Token: 0x04004D06 RID: 19718
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Uri twitchWsUri = new Uri("wss://eventsub.wss.twitch.tv/ws");

	// Token: 0x04004D07 RID: 19719
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine receiveCoroutine;

	// Token: 0x04004D08 RID: 19720
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRunning;

	// Token: 0x04004D09 RID: 19721
	[PublicizedFrom(EAccessModifier.Private)]
	public string sessionID = string.Empty;

	// Token: 0x04004D0A RID: 19722
	[PublicizedFrom(EAccessModifier.Private)]
	public string broadcasterUserID;

	// Token: 0x04004D0B RID: 19723
	[PublicizedFrom(EAccessModifier.Private)]
	public string accessToken;

	// Token: 0x04004D0C RID: 19724
	[PublicizedFrom(EAccessModifier.Private)]
	public string clientId;

	// Token: 0x04004D0D RID: 19725
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
