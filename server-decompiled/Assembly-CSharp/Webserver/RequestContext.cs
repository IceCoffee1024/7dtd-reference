using System;
using System.Collections.Specialized;
using SpaceWizards.HttpListener;

namespace Webserver
{
	// Token: 0x02001ABF RID: 6847
	public class RequestContext
	{
		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600CE65 RID: 52837 RVA: 0x004B258C File Offset: 0x004B078C
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

		// Token: 0x0600CE66 RID: 52838 RVA: 0x004B25B8 File Offset: 0x004B07B8
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

		// Token: 0x04009C56 RID: 40022
		public string RequestPath;

		// Token: 0x04009C57 RID: 40023
		public readonly ERequestMethod Method;

		// Token: 0x04009C58 RID: 40024
		public readonly HttpListenerRequest Request;

		// Token: 0x04009C59 RID: 40025
		[PublicizedFrom(EAccessModifier.Private)]
		public NameValueCollection queryParameters;

		// Token: 0x04009C5A RID: 40026
		public readonly HttpListenerResponse Response;

		// Token: 0x04009C5B RID: 40027
		public readonly WebConnection Connection;

		// Token: 0x04009C5C RID: 40028
		public readonly int PermissionLevel;
	}
}
