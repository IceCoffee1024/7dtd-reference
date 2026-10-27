using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Webserver.Permissions;
using Webserver.SSE;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B54 RID: 6996
	public class SseHandler : AbsHandler
	{
		// Token: 0x0600D208 RID: 53768 RVA: 0x004C1640 File Offset: 0x004BF840
		public SseHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
			SseHandler.ctorParams[0] = this;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsEvent), new Action<Type>(this.apiFoundCallback), false);
		}

		// Token: 0x0600D209 RID: 53769 RVA: 0x004C169C File Offset: 0x004BF89C
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiFoundCallback(Type _type)
		{
			ConstructorInfo constructor = _type.GetConstructor(SseHandler.ctorTypes);
			if (constructor == null)
			{
				return;
			}
			AbsEvent absEvent = (AbsEvent)constructor.Invoke(SseHandler.ctorParams);
			this.AddEvent(absEvent.Name, absEvent);
		}

		// Token: 0x0600D20A RID: 53770 RVA: 0x004C16DD File Offset: 0x004BF8DD
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			this.queueThead = ThreadManager.StartThread("SSE-Processing_" + this.urlBasePath, new ThreadManager.ThreadFunctionDelegate(this.QueueProcessThread), ThreadPriority.BelowNormal, null, true, false);
		}

		// Token: 0x0600D20B RID: 53771 RVA: 0x004C1717 File Offset: 0x004BF917
		public override void Shutdown()
		{
			base.Shutdown();
			this.shutdown = true;
			this.SignalSendQueue();
		}

		// Token: 0x0600D20C RID: 53772 RVA: 0x004C172C File Offset: 0x004BF92C
		public void AddEvent(string _eventName, AbsEvent _eventInstance)
		{
			this.events.Add(_eventName, _eventInstance);
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule("webevent." + _eventName, _eventInstance.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600D20D RID: 53773 RVA: 0x004C175C File Offset: 0x004BF95C
		public override void HandleRequest(RequestContext _context)
		{
			string text = _context.QueryParameters["events"];
			if (string.IsNullOrEmpty(text))
			{
				Log.Warning("[Web] [SSE] In SseHandler.HandleRequest(): No 'events' query parameter given");
				_context.Response.StatusCode = 400;
				return;
			}
			SseClient sseClient;
			try
			{
				sseClient = new SseClient(this, _context);
			}
			catch (Exception ex)
			{
				Log.Error("[Web] [SSE] In SseHandler.HandleRequest(): Could not create client:");
				Log.Exception(ex);
				_context.Response.StatusCode = 500;
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (string text2 in text.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				AbsEvent absEvent;
				if (!this.events.TryGetValue(text2, out absEvent))
				{
					Log.Warning("[Web] [SSE] In SseHandler.HandleRequest(): No handler found for event \"" + text2 + "\"");
				}
				else
				{
					num++;
					if (this.IsAuthorizedForEvent(text2, _context.PermissionLevel))
					{
						num2++;
						try
						{
							absEvent.AddListener(sseClient);
						}
						catch (Exception ex2)
						{
							Log.Error("[Web] [SSE] In SseHandler.HandleRequest(): Handler " + absEvent.Name + " threw an exception:");
							Log.Exception(ex2);
							_context.Response.StatusCode = 500;
						}
						num3++;
					}
				}
			}
			if (num == 0)
			{
				_context.Response.StatusCode = 400;
				_context.Response.Close();
				return;
			}
			if (num2 == 0)
			{
				_context.Response.StatusCode = 403;
				WebConnection connection = _context.Connection;
				_context.Response.Close();
				return;
			}
			this.clients.Add(sseClient);
		}

		// Token: 0x0600D20E RID: 53774 RVA: 0x004C18F0 File Offset: 0x004BFAF0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAuthorizedForEvent(string _eventName, int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webevent." + _eventName, _permissionLevel);
		}

		// Token: 0x0600D20F RID: 53775 RVA: 0x004C1908 File Offset: 0x004BFB08
		[PublicizedFrom(EAccessModifier.Private)]
		public void QueueProcessThread(ThreadManager.ThreadInfo _threadInfo)
		{
			while (!this.shutdown && !_threadInfo.TerminationRequested())
			{
				this.evSendRequest.WaitOne(500);
				foreach (KeyValuePair<string, AbsEvent> keyValuePair in this.events)
				{
					string text;
					AbsEvent absEvent;
					keyValuePair.Deconstruct(out text, out absEvent);
					string text2 = text;
					AbsEvent absEvent2 = absEvent;
					try
					{
						absEvent2.ProcessSendQueue();
					}
					catch (Exception ex)
					{
						Log.Error("[Web] [SSE] '" + text2 + "': Error processing send queue");
						Log.Exception(ex);
					}
				}
				for (int i = this.clients.Count - 1; i >= 0; i--)
				{
					this.clients[i].HandleKeepAlive();
				}
			}
		}

		// Token: 0x0600D210 RID: 53776 RVA: 0x004C19EC File Offset: 0x004BFBEC
		public void SignalSendQueue()
		{
			this.evSendRequest.Set();
		}

		// Token: 0x0600D211 RID: 53777 RVA: 0x004C19FC File Offset: 0x004BFBFC
		public void ClientClosed(SseClient _client)
		{
			foreach (KeyValuePair<string, AbsEvent> keyValuePair in this.events)
			{
				string text;
				AbsEvent absEvent;
				keyValuePair.Deconstruct(out text, out absEvent);
				string text2 = text;
				AbsEvent absEvent2 = absEvent;
				try
				{
					absEvent2.ClientClosed(_client);
				}
				catch (Exception ex)
				{
					Log.Error("[Web] [SSE] '" + text2 + "': Error closing client");
					Log.Exception(ex);
				}
			}
			this.clients.Remove(_client);
		}

		// Token: 0x04009F5F RID: 40799
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsEvent> events = new CaseInsensitiveStringDictionary<AbsEvent>();

		// Token: 0x04009F60 RID: 40800
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo queueThead;

		// Token: 0x04009F61 RID: 40801
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent evSendRequest = new AutoResetEvent(false);

		// Token: 0x04009F62 RID: 40802
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009F63 RID: 40803
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] ctorTypes = new Type[] { typeof(SseHandler) };

		// Token: 0x04009F64 RID: 40804
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] ctorParams = new object[1];

		// Token: 0x04009F65 RID: 40805
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> clients = new List<SseClient>();
	}
}
