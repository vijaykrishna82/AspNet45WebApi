using System;

namespace EventWebApi.Client
{
    public class DateTimeFactory
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
    }
}
