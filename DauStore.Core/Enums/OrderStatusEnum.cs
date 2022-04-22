﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Enums
{
    public enum OrderStatus
    {
        All = 0,
        Pending = 1,
        Confirmed = 2,
        Delivering = 3,
        Delivered = 4,
        Cancelled = 5,
        Return = 6
    }
}
