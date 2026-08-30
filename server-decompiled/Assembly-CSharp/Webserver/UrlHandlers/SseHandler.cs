using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Webserver.Permissions;
using Webserver.SSE;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AF7 RID: 6903
	public class SseHandler : AbsHandler
	{
		// Token: 0x0600CF9C RID: 53148 RVA: 0x004B9678 File Offset: 0x004B7878
		public SseHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
			SseHandler.ctorParams[0] = this;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsEvent), new Action<Type>(this.apiFoundCallback), false);
		}

		// Token: 0x0600CF9D RID: 53149 RVA: 0x004B96D4 File Offset: 0x004B78D4
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

		// Token: 0x0600CF9E RID: 53150 RVA: 0x004B9715 File Offset: 0x004B7915
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			this.queueThead = ThreadManager.StartThread("SSE-Processing_" + this.urlBasePath, new ThreadManager.ThreadFunctionDelegate(this.QueueProcessThread), ThreadPriority.BelowNormal, null, true, false);
		}

		// Token: 0x0600CF9F RID: 53151 RVA: 0x004B974F File Offset: 0x004B794F
		public override void Shutdown()
		{
			base.Shutdown();
			this.shutdown = true;
			this.SignalSendQueue();
		}

		// Token: 0x0600CFA0 RID: 53152 RVA: 0x004B9764 File Offset: 0x004B7964
		public void AddEvent(string _eventName, AbsEvent _eventInstance)
		{
			this.events.Add(_eventName, _eventInstance);
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule("webevent." + _eventName, _eventInstance.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600CFA1 RID: 53153 RVA: 0x004B9794 File Offset: 0x004B7994
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

		// Token: 0x0600CFA2 RID: 53154 RVA: 0x004B9928 File Offset: 0x004B7B28
		[PublicizedFrom(EAccessModifier.Private)]
		public bool IsAuthorizedForEvent(string _eventName, int _permissionLevel)
		{
			return AdminWebModules.Instance.ModuleAllowedWithLevel("webevent." + _eventName, _permissionLevel);
		}

		// Token: 0x0600CFA3 RID: 53155 RVA: 0x004B9940 File Offset: 0x004B7B40
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

		// Token: 0x0600CFA4 RID: 53156 RVA: 0x004B9A24 File Offset: 0x004B7C24
		public void SignalSendQueue()
		{
			this.evSendRequest.Set();
		}

		// Token: 0x0600CFA5 RID: 53157 RVA: 0x004B9A34 File Offset: 0x004B7C34
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

		// Token: 0x04009DB0 RID: 40368
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsEvent> events = new CaseInsensitiveStringDictionary<AbsEvent>();

		// Token: 0x04009DB1 RID: 40369
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo queueThead;

		// Token: 0x04009DB2 RID: 40370
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AutoResetEvent evSendRequest = new AutoResetEvent(false);

		// Token: 0x04009DB3 RID: 40371
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009DB4 RID: 40372
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] ctorTypes = new Type[] { typeof(SseHandler) };

		// Token: 0x04009DB5 RID: 40373
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] ctorParams = new object[1];

		// Token: 0x04009DB6 RID: 40374
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<SseClient> clients = new List<SseClient>();
	}
}
