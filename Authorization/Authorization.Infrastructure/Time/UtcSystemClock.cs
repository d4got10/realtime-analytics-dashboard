﻿using Authorization.Application;
using Authorization.Application.Time;

namespace Authorization.Infrastructure.Time;

public class UtcSystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}