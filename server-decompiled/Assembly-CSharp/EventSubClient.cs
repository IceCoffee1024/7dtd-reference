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

// Token: 0x02000C95 RID: 3221
public class EventSubClient
{
	// Token: 0x1400008A RID: 138
	// (add) Token: 0x06006282 RID: 25218 RVA: 0x00270114 File Offset: 0x0026E314
	// (remove) Token: 0x06006283 RID: 25219 RVA: 0x0027014C File Offset: 0x0026E34C
	public event Action<JObject> OnEventReceived;

	// Token: 0x06006284 RID: 25220 RVA: 0x00270181 File Offset: 0x0026E381
	public EventSubClient(string broadcasterID, string accessToken, string clientId)
	{
		this.broadcasterUserID = broadcasterID;
		this.accessToken = accessToken;
		this.clientId = clientId;
	}

	// Token: 0x06006285 RID: 25221 RVA: 0x002701BC File Offset: 0x0026E3BC
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

	// Token: 0x06006286 RID: 25222 RVA: 0x0027021F File Offset: 0x0026E41F
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

	// Token: 0x06006287 RID: 25223 RVA: 0x00270230 File Offset: 0x0026E430
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

	// Token: 0x06006288 RID: 25224 RVA: 0x0027038D File Offset: 0x0026E58D
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

	// Token: 0x06006289 RID: 25225 RVA: 0x0027039C File Offset: 0x0026E59C
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

	// Token: 0x0600628A RID: 25226 RVA: 0x002703B4 File Offset: 0x0026E5B4
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

	// Token: 0x0600628B RID: 25227 RVA: 0x00270420 File Offset: 0x0026E620
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

	// Token: 0x0600628C RID: 25228 RVA: 0x0027048C File Offset: 0x0026E68C
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

	// Token: 0x04004CE0 RID: 19680
	[PublicizedFrom(EAccessModifier.Private)]
	public ClientWebSocket ws;

	// Token: 0x04004CE1 RID: 19681
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Uri twitchWsUri = new Uri("wss://eventsub.wss.twitch.tv/ws");

	// Token: 0x04004CE2 RID: 19682
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine receiveCoroutine;

	// Token: 0x04004CE3 RID: 19683
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRunning;

	// Token: 0x04004CE4 RID: 19684
	[PublicizedFrom(EAccessModifier.Private)]
	public string sessionID = string.Empty;

	// Token: 0x04004CE5 RID: 19685
	[PublicizedFrom(EAccessModifier.Private)]
	public string broadcasterUserID;

	// Token: 0x04004CE6 RID: 19686
	[PublicizedFrom(EAccessModifier.Private)]
	public string accessToken;

	// Token: 0x04004CE7 RID: 19687
	[PublicizedFrom(EAccessModifier.Private)]
	public string clientId;

	// Token: 0x04004CE8 RID: 19688
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cleanedUp;
}
