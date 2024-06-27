﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Bus

{
    public class BusConst
    {
        public const string OrderCreatedEventExchange = "order.created.event.exchange";
        public const string StockOrderCreatedEventQueue = "stock.order.created.event.queue";
    }
}