using System;
using System.Collections.Specialized;
using SpaceWizards.HttpListener;

namespace Webserver
{
	// Token: 0x02001A98 RID: 6808
	public class RequestContext
	{
		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x0600CCDC RID: 52444 RVA: 0x004AC198 File Offset: 0x004AA398
		public NameValueCollection QueryParameters
		{
			get
			{
				NameValueCollection nameValueCollection;
				if ((nameValueCollection = this.queryParameters) == null)
				{
					nameValueCollection = (this.queryParameters = this.Request.QueryString);
				}
				return nameValueCollection;
			}
		}

		// Token: 0x0600CCDD RID: 52445 RVA: 0x004AC1C4 File Offset: 0x004AA3C4
		public RequestContext(string _requestPath, HttpListenerRequest _request, HttpListenerResponse _response, WebConnection _connection, int _permissionLevel)
		{
			this.RequestPath = _requestPath;
			this.Request = _request;
			this.Response = _response;
			this.Connection = _connection;
			this.PermissionLevel = _permissionLevel;
			string httpMethod = _request.HttpMethod;
			ERequestMethod erequestMethod;
			if (!(httpMethod == "GET"))
			{
				if (!(httpMethod == "PUT"))
				{
					if (!(httpMethod == "POST"))
					{
						if (!(httpMethod == "DELETE"))
						{
							if (!(httpMethod == "HEAD"))
							{
								if (!(httpMethod == "OPTIONS"))
								{
									erequestMethod = ERequestMethod.Other;
								}
								else
								{
									erequestMethod = ERequestMethod.OPTIONS;
								}
							}
							else
							{
								erequestMethod = ERequestMethod.HEAD;
							}
						}
						else
						{
							erequestMethod = ERequestMethod.DELETE;
						}
					}
					else
					{
						erequestMethod = ERequestMethod.POST;
					}
				}
				else
				{
					erequestMethod = ERequestMethod.PUT;
				}
			}
			else
			{
				erequestMethod = ERequestMethod.GET;
			}
			this.Method = erequestMethod;
		}

		// Token: 0x04009B25 RID: 39717
		public string RequestPath;

		// Token: 0x04009B26 RID: 39718
		public readonly ERequestMethod Method;

		// Token: 0x04009B27 RID: 39719
		public readonly HttpListenerRequest Request;

		// Token: 0x04009B28 RID: 39720
		[PublicizedFrom(EAccessModifier.Private)]
		public NameValueCollection queryParameters;

		// Token: 0x04009B29 RID: 39721
		public readonly HttpListenerResponse Response;

		// Token: 0x04009B2A RID: 39722
		public readonly WebConnection Connection;

		// Token: 0x04009B2B RID: 39723
		public readonly int PermissionLevel;
	}
}
