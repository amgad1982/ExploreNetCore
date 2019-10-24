using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.Features.WeatherForecasts.Models;

namespace WebAPI.Features.WeatherForecasts.Handelers {
    public static class Get {
        public class Query : IRequest<Response> {

        }

        public class Response {
            public IEnumerable<WeatherForecast> Forecasts { get; set; }
        }
        public class Handeler : IRequestHandler<Query, Response> {
            public Handeler () { }

            public async Task<Response> Handle (Query request, CancellationToken cancellationToken) {
                string[] Summaries = new [] {
                    "Freezing",
                    "Bracing",
                    "Chilly",
                    "Cool",
                    "Mild",
                    "Warm",
                    "Balmy",
                    "Hot",
                    "Sweltering",
                    "Scorching"
                };
                var rng = new Random ();
                var result = Enumerable.Range (1, 5).Select (index => new WeatherForecast {
                        Date = DateTime.Now.AddDays (index),
                            TemperatureC = rng.Next (-20, 55),
                            Summary = Summaries[rng.Next (Summaries.Length)]
                    })
                    .ToArray ();
                return await Task.FromResult (new Response { Forecasts = result });
            }
        }
    }
}