﻿using SamplePlugin.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.CatalogModule
{
    public class MyService : IMyService
    {
        public void SayHello()
        {
            Console.WriteLine("Hello");
        }
    }
}
