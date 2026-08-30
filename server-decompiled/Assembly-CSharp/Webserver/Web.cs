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
	// Token: 0x02001A9D RID: 6813
	public class Web : IConsoleServer
	{
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x0600CCE6 RID: 52454 RVA: 0x004AC434 File Offset: 0x004AA634
		// (remove) Token: 0x0600CCE7 RID: 52455 RVA: 0x004AC468 File Offset: 0x004AA668
		public static event Action<Web> ServerInitialized;

		// Token: 0x0600CCE8 RID: 52456 RVA: 0x004AC49C File Offset: 0x004AA69C
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

		// Token: 0x0600CCE9 RID: 52457 RVA: 0x004AC668 File Offset: 0x004AA868
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

		// Token: 0x0600CCEA RID: 52458 RVA: 0x004AC734 File Offset: 0x004AA934
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

		// Token: 0x0600CCEB RID: 52459 RVA: 0x004AC7C0 File Offset: 0x004AA9C0
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

		// Token: 0x0600CCEC RID: 52460 RVA: 0x004AC844 File Offset: 0x004AAA44
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

		// Token: 0x0600CCED RID: 52461 RVA: 0x004AC900 File Offset: 0x004AAB00
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

		// Token: 0x0600CCEE RID: 52462 RVA: 0x004AC998 File Offset: 0x004AAB98
		public void SendLine(string _line)
		{
			this.ConnectionHandler.SendLine(_line);
		}

		// Token: 0x0600CCEF RID: 52463 RVA: 0x000027FC File Offset: 0x000009FC
		public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
		{
		}

		// Token: 0x0600CCF0 RID: 52464 RVA: 0x004AC9A8 File Offset: 0x004AABA8
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

		// Token: 0x0600CCF1 RID: 52465 RVA: 0x004ACBA8 File Offset: 0x004AADA8
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

		// Token: 0x0600CCF2 RID: 52466 RVA: 0x004ACC28 File Offset: 0x004AAE28
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

		// Token: 0x04009B36 RID: 39734
		public const string DataPath = "Data/Web";

		// Token: 0x04009B37 RID: 39735
		[PublicizedFrom(EAccessModifier.Private)]
		public const string indexPageUrl = "/app";

		// Token: 0x04009B38 RID: 39736
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<AbsHandler> handlers = new List<AbsHandler>();

		// Token: 0x04009B39 RID: 39737
		public readonly List<WebMod> WebMods = new List<WebMod>();

		// Token: 0x04009B3A RID: 39738
		public readonly ConnectionHandler ConnectionHandler;

		// Token: 0x04009B3B RID: 39739
		public readonly OpenApiHelpers OpenApiHelpers;

		// Token: 0x04009B3C RID: 39740
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly SpaceWizards.HttpListener.HttpListener listener = new SpaceWizards.HttpListener.HttpListener();

		// Token: 0x04009B3D RID: 39741
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Version httpProtocolVersion = new Version(1, 1);

		// Token: 0x04009B3E RID: 39742
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AsyncCallback handleRequestDelegate;

		// Token: 0x04009B3F RID: 39743
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shutdown;

		// Token: 0x04009B40 RID: 39744
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler authSampler = CustomSampler.Create("Auth", false);

		// Token: 0x04009B41 RID: 39745
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler cookieSampler = CustomSampler.Create("ConCookie", false);

		// Token: 0x04009B42 RID: 39746
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler handlerSampler = CustomSampler.Create("Handler", false);
	}
}
