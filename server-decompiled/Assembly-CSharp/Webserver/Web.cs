using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SpaceWizards.HttpListener;
using UnityEngine;
using UnityEngine.Profiling;
using Webserver.FileCache;
using Webserver.Permissions;
using Webserver.UrlHandlers;
using Webserver.WebAPI;

namespace Webserver
{
	// Token: 0x02001B21 RID: 6945
	public class Web : IConsoleServer
	{
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x0600D0DB RID: 53467 RVA: 0x004BA7F0 File Offset: 0x004B89F0
		// (remove) Token: 0x0600D0DC RID: 53468 RVA: 0x004BA824 File Offset: 0x004B8A24
		public static event Action<Web> ServerInitialized;

		// Token: 0x0600D0DD RID: 53469 RVA: 0x004BA858 File Offset: 0x004B8A58
		public Web()
		{
			try
			{
				if (!GamePrefs.GetBool(EnumUtils.Parse<EnumGamePrefs>("WebDashboardEnabled", false)))
				{
					Log.Out("[Web] Webserver not started, WebDashboardEnabled set to false");
				}
				else
				{
					int @int = GamePrefs.GetInt(EnumUtils.Parse<EnumGamePrefs>("WebDashboardPort", false));
					if (@int < 1 || @int > 65533)
					{
						Log.Out("[Web] Webserver not started (WebDashboardPort not within 1-65535)");
					}
					else if (!SpaceWizards.HttpListener.HttpListener.IsSupported)
					{
						Log.Out("[Web] Webserver not started (HttpListener.IsSupported returned false)");
					}
					else
					{
						if (string.IsNullOrEmpty(GamePrefs.GetString(EnumUtils.Parse<EnumGamePrefs>("WebDashboardUrl", false))))
						{
							Log.Warning("[Web] WebDashboardUrl not set. Recommended to set it to the public URL pointing to your dashboard / reverse proxy");
						}
						this.ConnectionHandler = new ConnectionHandler();
						this.OpenApiHelpers = new OpenApiHelpers();
						this.RegisterDefaultHandlers();
						Action<Web> serverInitialized = Web.ServerInitialized;
						if (serverInitialized != null)
						{
							serverInitialized(this);
						}
						this.listener.Prefixes.Add(string.Format("http://+:{0}/", @int));
						this.listener.Start();
						this.handleRequestDelegate = new AsyncCallback(this.HandleRequest);
						this.listener.BeginGetContext(this.handleRequestDelegate, this.listener);
						SingletonMonoBehaviour<SdtdConsole>.Instance.RegisterServer(this);
						Log.Out(string.Format("[Web] Started Webserver on port {0}", @int));
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("[Web] Error in Web.ctor: ");
				Log.Exception(ex);
			}
		}

		// Token: 0x0600D0DE RID: 53470 RVA: 0x004BAA24 File Offset: 0x004B8C24
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterDefaultHandlers()
		{
			bool flag = StringParsers.ParseBool("false", 0, -1, true);
			string text = Web.DetectWebserverFolder();
			this.RegisterPathHandler("/", new RewriteHandler("/files/", false));
			this.RegisterPathHandler("/app", new RewriteHandler("/files/index.html", true));
			this.RegisterWebMods(flag);
			this.RegisterPathHandler("/session/", new SessionHandler());
			this.RegisterPathHandler("/userstatus", new UserStatusHandler(null));
			this.RegisterPathHandler("/sse/", new SseHandler(null));
			this.RegisterPathHandler("/files/", new StaticHandler(text, flag ? new SimpleCache() : new DirectAccess(), false, null));
			this.RegisterPathHandler("/itemicons/", new ItemIconHandler(true, null));
			this.RegisterPathHandler("/api/", new ApiHandler());
		}

		// Token: 0x0600D0DF RID: 53471 RVA: 0x004BAAF0 File Offset: 0x004B8CF0
		[PublicizedFrom(EAccessModifier.Private)]
		public static string DetectWebserverFolder()
		{
			string text = GameIO.GetGameDir("Data/Web") + "/webroot";
			foreach (Mod mod in ModManager.GetLoadedMods())
			{
				string text2 = mod.Path + "/webroot";
				if (Directory.Exists(text2))
				{
					text = text2;
				}
			}
			Log.Out("[Web] Serving basic webserver files from " + text);
			return text;
		}

		// Token: 0x0600D0E0 RID: 53472 RVA: 0x004BAB7C File Offset: 0x004B8D7C
		public void RegisterPathHandler(string _urlBasePath, AbsHandler _handler)
		{
			using (List<AbsHandler>.Enumerator enumerator = this.handlers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!(enumerator.Current.UrlBasePath != _urlBasePath))
					{
						Log.Error("[Web] Handler for relative path " + _urlBasePath + " already registerd.");
						return;
					}
				}
			}
			this.handlers.Add(_handler);
			_handler.SetBasePathAndParent(this, _urlBasePath);
		}

		// Token: 0x0600D0E1 RID: 53473 RVA: 0x004BAC00 File Offset: 0x004B8E00
		[PublicizedFrom(EAccessModifier.Private)]
		public void RegisterWebMods(bool _useStaticCache)
		{
			foreach (Mod mod in ModManager.GetLoadedMods())
			{
				try
				{
					try
					{
						WebMod webMod = new WebMod(this, mod, _useStaticCache);
						this.WebMods.Add(webMod);
					}
					catch (InvalidDataException ex)
					{
						Log.Error("[Web] Could not load webmod from mod " + mod.Name + ": " + ex.Message);
					}
				}
				catch (Exception ex2)
				{
					Log.Error("[Web] Failed loading web mods from mod " + mod.Name);
					Log.Exception(ex2);
				}
			}
		}

		// Token: 0x0600D0E2 RID: 53474 RVA: 0x004BACBC File Offset: 0x004B8EBC
		public void Disconnect()
		{
			if (this.shutdown)
			{
				return;
			}
			this.shutdown = true;
			try
			{
				foreach (AbsHandler absHandler in this.handlers)
				{
					absHandler.Shutdown();
				}
				this.listener.Stop();
				this.listener.Close();
			}
			catch (Exception ex)
			{
				Log.Out(string.Format("[Web] Error in Web.Disconnect: {0}", ex));
			}
		}

		// Token: 0x0600D0E3 RID: 53475 RVA: 0x004BAD54 File Offset: 0x004B8F54
		public void SendLine(string _line)
		{
			this.ConnectionHandler.SendLine(_line);
		}

		// Token: 0x0600D0E4 RID: 53476 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600D0E5 RID: 53477 RVA: 0x004BAD64 File Offset: 0x004B8F64
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRequest(IAsyncResult _result)
		{
			SpaceWizards.HttpListener.HttpListener httpListener = (SpaceWizards.HttpListener.HttpListener)_result.AsyncState;
			if (!httpListener.IsListening)
			{
				return;
			}
			SpaceWizards.HttpListener.HttpListenerContext httpListenerContext = httpListener.EndGetContext(_result);
			httpListener.BeginGetContext(this.handleRequestDelegate, httpListener);
			try
			{
				SpaceWizards.HttpListener.HttpListenerRequest request = httpListenerContext.Request;
				SpaceWizards.HttpListener.HttpListenerResponse response = httpListenerContext.Response;
				response.SendChunked = false;
				response.ProtocolVersion = this.httpProtocolVersion;
				if (GameManager.Instance.World == null)
				{
					response.StatusCode = 503;
				}
				else if (request.Url == null)
				{
					response.StatusCode = 400;
				}
				else
				{
					WebConnection webConnection;
					int num = this.DoAuthentication(request, out webConnection);
					if (webConnection != null)
					{
						Cookie cookie = new Cookie("sid", webConnection.SessionID, "/")
						{
							Expired = false,
							Expires = DateTime.MinValue,
							HttpOnly = true,
							Secure = false
						};
						response.AppendCookie(cookie);
					}
					string absolutePath = request.Url.AbsolutePath;
					if (absolutePath.Length < 2)
					{
						response.Redirect("/app");
					}
					else
					{
						request.ContentEncoding = Encoding.UTF8;
						RequestContext requestContext = new RequestContext(absolutePath, request, response, webConnection, num);
						if (requestContext.Method == ERequestMethod.Other)
						{
							requestContext.Response.StatusCode = 400;
						}
						else
						{
							this.ApplyPathHandler(requestContext);
						}
					}
				}
			}
			catch (IOException ex)
			{
				if (ex.InnerException is SocketException)
				{
					Log.Out("[Web] Error in Web.HandleRequest(): Remote host closed connection: " + ex.InnerException.Message);
				}
				else
				{
					Log.Out(string.Format("[Web] Error (IO) in Web.HandleRequest(): {0}", ex));
				}
			}
			catch (Exception ex2)
			{
				Log.Error("[Web] Error in Web.HandleRequest(): ");
				Log.Exception(ex2);
			}
			finally
			{
				if (!httpListenerContext.Response.SendChunked)
				{
					httpListenerContext.Response.Close();
				}
			}
		}

		// Token: 0x0600D0E6 RID: 53478 RVA: 0x004BAF64 File Offset: 0x004B9164
		public void ApplyPathHandler(RequestContext _context)
		{
			int i = this.handlers.Count - 1;
			while (i >= 0)
			{
				AbsHandler absHandler = this.handlers[i];
				if (_context.RequestPath.StartsWith(absHandler.UrlBasePath))
				{
					if (!absHandler.IsAuthorizedForHandler(_context))
					{
						_context.Response.StatusCode = 403;
						WebConnection connection = _context.Connection;
						return;
					}
					absHandler.HandleRequest(_context);
					return;
				}
				else
				{
					i--;
				}
			}
			_context.Response.StatusCode = 404;
		}

		// Token: 0x0600D0E7 RID: 53479 RVA: 0x004BAFE4 File Offset: 0x004B91E4
		[PublicizedFrom(EAccessModifier.Private)]
		public int DoAuthentication(SpaceWizards.HttpListener.HttpListenerRequest _req, out WebConnection _con)
		{
			_con = null;
			Cookie cookie = _req.Cookies["sid"];
			string text = ((cookie != null) ? cookie.Value : null);
			IPEndPoint remoteEndPoint = _req.RemoteEndPoint;
			if (remoteEndPoint == null)
			{
				Log.Warning("[Web] No RemoteEndPoint on web request");
				return 2000;
			}
			if (!string.IsNullOrEmpty(text))
			{
				_con = this.ConnectionHandler.IsLoggedIn(text, remoteEndPoint.Address);
				if (_con != null)
				{
					int userPermissionLevel = GameManager.Instance.adminTools.Users.GetUserPermissionLevel(_con.UserId);
					int num = int.MaxValue;
					if (_con.CrossplatformUserId != null)
					{
						num = GameManager.Instance.adminTools.Users.GetUserPermissionLevel(_con.CrossplatformUserId);
					}
					return Math.Min(userPermissionLevel, num);
				}
			}
			string text2;
			string text3;
			if (!_req.Headers.TryGetValue("X-SDTD-API-TOKENNAME", out text2) || !_req.Headers.TryGetValue("X-SDTD-API-SECRET", out text3))
			{
				return 2000;
			}
			int permissionLevel = AdminApiTokens.Instance.GetPermissionLevel(text2, text3);
			if (permissionLevel < 2147483647)
			{
				return permissionLevel;
			}
			Log.Warning(string.Format("[Web] Invalid Admintoken used from {0}", remoteEndPoint));
			return 2000;
		}

		// Token: 0x04009E16 RID: 40470
		public const string DataPath = "Data/Web";

		// Token: 0x04009E17 RID: 40471
		[PublicizedFrom(EAccessModifier.Private)]
		public const string indexPageUrl = "/app";

		// Token: 0x04009E18 RID: 40472
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AbsHandler> handlers = new List<AbsHandler>();

		// Token: 0x04009E19 RID: 40473
		public readonly List<WebMod> WebMods = new List<WebMod>();

		// Token: 0x04009E1A RID: 40474
		public readonly ConnectionHandler ConnectionHandler;

		// Token: 0x04009E1B RID: 40475
		public readonly OpenApiHelpers OpenApiHelpers;

		// Token: 0x04009E1C RID: 40476
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListener listener = new SpaceWizards.HttpListener.HttpListener();

		// Token: 0x04009E1D RID: 40477
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Version httpProtocolVersion = new Version(1, 1);

		// Token: 0x04009E1E RID: 40478
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AsyncCallback handleRequestDelegate;

		// Token: 0x04009E1F RID: 40479
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009E20 RID: 40480
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler authSampler = CustomSampler.Create("Auth", false);

		// Token: 0x04009E21 RID: 40481
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler cookieSampler = CustomSampler.Create("ConCookie", false);

		// Token: 0x04009E22 RID: 40482
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler handlerSampler = CustomSampler.Create("Handler", false);
	}
}
