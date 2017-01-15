﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using EventWebApi.Models;

namespace EventWebApi.Controllers
{
    public class StatsController : ApiController
    {
        [Route("stats/status")]
        public dynamic GetAlive()
        {
            return new
            {
                alive = true,
                _links = new Dictionary<string, HypermediaLink>
                {
                    { "self", new HypermediaLink("/stats/status") },
                    { "uptime", new HypermediaLink("/stats/uptime") }
                }
            };
        }

        [Route("stats/uptime")]
        public dynamic GetUptime()
        {
            return new
            {
                upSince = new DateTime(2014, 6, 27, 23, 51, 12, DateTimeKind.Utc),
                _links = new Dictionary<string, HypermediaLink>
                {
                    { "self", new HypermediaLink("/stats/uptime") }
                }
            };
        }
    }
}