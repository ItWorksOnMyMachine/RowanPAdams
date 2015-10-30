﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace RowanAdams.ActionResults
{
	public class NoContentActionResult : IHttpActionResult
	{
		private readonly HttpRequestMessage _request;
		private readonly string _location;

		public NoContentActionResult(HttpRequestMessage request, string location)
		{
			_request = request;
			_location = location;
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response = _request.CreateResponse(HttpStatusCode.NoContent);
			response.Headers.Location = new Uri(_location);
			return Task.FromResult(response);
		}
	}
}