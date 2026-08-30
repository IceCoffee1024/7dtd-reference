using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Webserver.Permissions;
using Webserver.SSE;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AD0 RID: 6864
	public class SseHandler : AbsHandler
	{
		// Token: 0x0600CE13 RID: 52755 RVA: 0x004B3284 File Offset: 0x004B1484
		public SseHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
			SseHandler.ctorParams[0] = this;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsEvent), new Action<Type>(this.apiFoundCallback), false);
		}

		// Token: 0x0600CE14 RID: 52756 RVA: 0x004B32E0 File Offset: 0x004B14E0
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

		// Token: 0x0600CE15 RID: 52757 RVA: 0x004B3321 File Offset: 0x004B1521
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			this.queueThead = ThreadManager.StartThread("SSE-Processing_" + this.urlBasePath, new ThreadManager.ThreadFunctionDelegate(this.QueueProcessThread), ThreadPriority.BelowNormal, null, true, false);
		}

		// Token: 0x0600CE16 RID: 52758 RVA: 0x004B335B File Offset: 0x004B155B
		public override void Shutdown()
		{
			base.Shutdown();
			this.shutdown = true;
			this.SignalSendQueue();
		}

		// Token: 0x0600CE17 RID: 52759 RVA: 0x004B3370 File Offset: 0x004B1570
		public void AddEvent(string _eventName, AbsEvent _eventInstance)
		{
			this.events.Add(_eventName, _eventInstance);
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule("webevent." + _eventName, _eventInstance.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600CE18 RID: 52760 RVA: 0x004B33A0 File Offset: 0x004B15A0
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

		// Token: 0x0600CE19 RID: 52761 RVA: 0x004B3534 File Offset: 0x004B1734
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAuthorizedForEvent(string _eventName, int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webevent." + _eventName, _permissionLevel);
		}

		// Token: 0x0600CE1A RID: 52762 RVA: 0x004B354C File Offset: 0x004B174C
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

		// Token: 0x0600CE1B RID: 52763 RVA: 0x004B3630 File Offset: 0x004B1830
		public void SignalSendQueue()
		{
			this.evSendRequest.Set();
		}

		// Token: 0x0600CE1C RID: 52764 RVA: 0x004B3640 File Offset: 0x004B1840
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

		// Token: 0x04009C7F RID: 40063
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsEvent> events = new CaseInsensitiveStringDictionary<AbsEvent>();

		// Token: 0x04009C80 RID: 40064
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo queueThead;

		// Token: 0x04009C81 RID: 40065
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent evSendRequest = new AutoResetEvent(false);

		// Token: 0x04009C82 RID: 40066
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009C83 RID: 40067
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] ctorTypes = new Type[] { typeof(SseHandler) };

		// Token: 0x04009C84 RID: 40068
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] ctorParams = new object[1];

		// Token: 0x04009C85 RID: 40069
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> clients = new List<SseClient>();
	}
}
