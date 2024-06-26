using System;

namespace Sample.Services;

public class DateTimeProvider
{
    public virtual long CurrentUnixTimeSeconds() => DateTimeOffset.Now.ToUnixTimeSeconds();
}
