using System;
using System.Collections.Specialized;
using SpaceWizards.HttpListener;

namespace Webserver
{
	// Token: 0x02001AEB RID: 6891
	public class RequestContext
	{
		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600CFBD RID: 53181 RVA: 0x004B6A58 File Offset: 0x004B4C58
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

		// Token: 0x0600CFBE RID: 53182 RVA: 0x004B6A84 File Offset: 0x004B4C84
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

		// Token: 0x04009D1C RID: 40220
		public string RequestPath;

		// Token: 0x04009D1D RID: 40221
		public readonly ERequestMethod Method;

		// Token: 0x04009D1E RID: 40222
		public readonly HttpListenerRequest Request;

		// Token: 0x04009D1F RID: 40223
		[PublicizedFrom(EAccessModifier.Private)]
		public NameValueCollection queryParameters;

		// Token: 0x04009D20 RID: 40224
		public readonly HttpListenerResponse Response;

		// Token: 0x04009D21 RID: 40225
		public readonly WebConnection Connection;

		// Token: 0x04009D22 RID: 40226
		public readonly int PermissionLevel;
	}
}
