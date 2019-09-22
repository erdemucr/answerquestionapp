using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AnswerQuestionApp.Service.Infrastructure
{
    public class WebApiHandler : DelegatingHandler
    {
        private readonly string _token;
        private const string TOKEN = "Authorization";
        public WebApiHandler(string token)
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //if (!request.Headers.Contains(TOKEN) && string.IsNullOrEmpty(_token))
            //{
            //    return new HttpResponseMessage(HttpStatusCode.BadRequest)
            //    {
            //        Content = new StringContent("Missing auth token.")
            //    };
            //}
            //else if (!request.Headers.Contains(TOKEN) && !string.IsNullOrEmpty(_token))
            //{
            //    request.Headers.Add(TOKEN, $"Bearer {_token}");
            //}
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            return response;
        }
    }
}
