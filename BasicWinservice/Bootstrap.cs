using Autofac;
using BasicWinservice.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicWinservice
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()
        {
            ContainerBuilder cb = new ContainerBuilder();
            cb.RegisterType<BasicService>();
            return cb.Build();
        }
    }
}
