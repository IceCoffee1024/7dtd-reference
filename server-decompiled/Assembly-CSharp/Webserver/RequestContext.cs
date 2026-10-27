using System;
using System.Collections.Specialized;
using SpaceWizards.HttpListener;

namespace Webserver
{
	// Token: 0x02001B1C RID: 6940
	public class RequestContext
	{
		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x0600D0D1 RID: 53457 RVA: 0x004BA554 File Offset: 0x004B8754
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

		// Token: 0x0600D0D2 RID: 53458 RVA: 0x004BA580 File Offset: 0x004B8780
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

		// Token: 0x04009E05 RID: 40453
		public string RequestPath;

		// Token: 0x04009E06 RID: 40454
		public readonly ERequestMethod Method;

		// Token: 0x04009E07 RID: 40455
		public readonly HttpListenerRequest Request;

		// Token: 0x04009E08 RID: 40456
		[PublicizedFrom(EAccessModifier.Private)]
		public NameValueCollection queryParameters;

		// Token: 0x04009E09 RID: 40457
		public readonly HttpListenerResponse Response;

		// Token: 0x04009E0A RID: 40458
		public readonly WebConnection Connection;

		// Token: 0x04009E0B RID: 40459
		public readonly int PermissionLevel;
	}
}
