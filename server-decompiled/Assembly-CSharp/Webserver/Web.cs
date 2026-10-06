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
	// Token: 0x02001AF0 RID: 6896
	public class Web : IConsoleServer
	{
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x0600CFC7 RID: 53191 RVA: 0x004B6CF4 File Offset: 0x004B4EF4
		// (remove) Token: 0x0600CFC8 RID: 53192 RVA: 0x004B6D28 File Offset: 0x004B4F28
		public static event Action<Web> ServerInitialized;

		// Token: 0x0600CFC9 RID: 53193 RVA: 0x004B6D5C File Offset: 0x004B4F5C
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

		// Token: 0x0600CFCA RID: 53194 RVA: 0x004B6F28 File Offset: 0x004B5128
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

		// Token: 0x0600CFCB RID: 53195 RVA: 0x004B6FF4 File Offset: 0x004B51F4
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

		// Token: 0x0600CFCC RID: 53196 RVA: 0x004B7080 File Offset: 0x004B5280
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

		// Token: 0x0600CFCD RID: 53197 RVA: 0x004B7104 File Offset: 0x004B5304
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

		// Token: 0x0600CFCE RID: 53198 RVA: 0x004B71C0 File Offset: 0x004B53C0
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

		// Token: 0x0600CFCF RID: 53199 RVA: 0x004B7258 File Offset: 0x004B5458
		public void SendLine(string _line)
		{
			this.ConnectionHandler.SendLine(_line);
		}

		// Token: 0x0600CFD0 RID: 53200 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600CFD1 RID: 53201 RVA: 0x004B7268 File Offset: 0x004B5468
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

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x004B7468 File Offset: 0x004B5668
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

		// Token: 0x0600CFD3 RID: 53203 RVA: 0x004B74E8 File Offset: 0x004B56E8
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

		// Token: 0x04009D2D RID: 40237
		public const string DataPath = "Data/Web";

		// Token: 0x04009D2E RID: 40238
		[PublicizedFrom(EAccessModifier.Private)]
		public const string indexPageUrl = "/app";

		// Token: 0x04009D2F RID: 40239
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AbsHandler> handlers = new List<AbsHandler>();

		// Token: 0x04009D30 RID: 40240
		public readonly List<WebMod> WebMods = new List<WebMod>();

		// Token: 0x04009D31 RID: 40241
		public readonly ConnectionHandler ConnectionHandler;

		// Token: 0x04009D32 RID: 40242
		public readonly OpenApiHelpers OpenApiHelpers;

		// Token: 0x04009D33 RID: 40243
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListener listener = new SpaceWizards.HttpListener.HttpListener();

		// Token: 0x04009D34 RID: 40244
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Version httpProtocolVersion = new Version(1, 1);

		// Token: 0x04009D35 RID: 40245
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AsyncCallback handleRequestDelegate;

		// Token: 0x04009D36 RID: 40246
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009D37 RID: 40247
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler authSampler = CustomSampler.Create("Auth", false);

		// Token: 0x04009D38 RID: 40248
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler cookieSampler = CustomSampler.Create("ConCookie", false);

		// Token: 0x04009D39 RID: 40249
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler handlerSampler = CustomSampler.Create("Handler", false);
	}
}
