using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Webserver.Permissions;
using Webserver.SSE;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B23 RID: 6947
	public class SseHandler : AbsHandler
	{
		// Token: 0x0600D0F4 RID: 53492 RVA: 0x004BDB44 File Offset: 0x004BBD44
		public SseHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
			SseHandler.ctorParams[0] = this;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsEvent), new Action<Type>(this.apiFoundCallback), false);
		}

		// Token: 0x0600D0F5 RID: 53493 RVA: 0x004BDBA0 File Offset: 0x004BBDA0
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

		// Token: 0x0600D0F6 RID: 53494 RVA: 0x004BDBE1 File Offset: 0x004BBDE1
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			this.queueThead = ThreadManager.StartThread("SSE-Processing_" + this.urlBasePath, new ThreadManager.ThreadFunctionDelegate(this.QueueProcessThread), ThreadPriority.BelowNormal, null, true, false);
		}

		// Token: 0x0600D0F7 RID: 53495 RVA: 0x004BDC1B File Offset: 0x004BBE1B
		public override void Shutdown()
		{
			base.Shutdown();
			this.shutdown = true;
			this.SignalSendQueue();
		}

		// Token: 0x0600D0F8 RID: 53496 RVA: 0x004BDC30 File Offset: 0x004BBE30
		public void AddEvent(string _eventName, AbsEvent _eventInstance)
		{
			this.events.Add(_eventName, _eventInstance);
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule("webevent." + _eventName, _eventInstance.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600D0F9 RID: 53497 RVA: 0x004BDC60 File Offset: 0x004BBE60
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

		// Token: 0x0600D0FA RID: 53498 RVA: 0x004BDDF4 File Offset: 0x004BBFF4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAuthorizedForEvent(string _eventName, int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webevent." + _eventName, _permissionLevel);
		}

		// Token: 0x0600D0FB RID: 53499 RVA: 0x004BDE0C File Offset: 0x004BC00C
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

		// Token: 0x0600D0FC RID: 53500 RVA: 0x004BDEF0 File Offset: 0x004BC0F0
		public void SignalSendQueue()
		{
			this.evSendRequest.Set();
		}

		// Token: 0x0600D0FD RID: 53501 RVA: 0x004BDF00 File Offset: 0x004BC100
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

		// Token: 0x04009E76 RID: 40566
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsEvent> events = new CaseInsensitiveStringDictionary<AbsEvent>();

		// Token: 0x04009E77 RID: 40567
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo queueThead;

		// Token: 0x04009E78 RID: 40568
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent evSendRequest = new AutoResetEvent(false);

		// Token: 0x04009E79 RID: 40569
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009E7A RID: 40570
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] ctorTypes = new Type[] { typeof(SseHandler) };

		// Token: 0x04009E7B RID: 40571
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] ctorParams = new object[1];

		// Token: 0x04009E7C RID: 40572
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> clients = new List<SseClient>();
	}
}
