using System;

namespace EventWebApi.Client.Models
{
    public class UptimeResponseBody : RestResponseBody
    {
        public DateTime UpSince { get; set; }
    }
}