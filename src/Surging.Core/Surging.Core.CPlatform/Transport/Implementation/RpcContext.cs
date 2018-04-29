﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Surging.Core.CPlatform.Transport.Implementation
{
    public class RpcContext
    {
        private ConcurrentDictionary<String, Object> contextParameters;

        public ConcurrentDictionary<String, Object> GetContextParameters()
        {
            return contextParameters;
        }

        public void SetAttachment(string key,object value)
        {
            contextParameters.AddOrUpdate(key, value,(k,v)=>value);
        }

        public object GetAttachment(string key)
        {
            contextParameters.TryGetValue(key, out object result);
            return result;
        }

        public void SetContextParameters(ConcurrentDictionary<String, Object> contextParameters)
        {
            this.contextParameters = contextParameters;
        }

        private static ThreadLocal<RpcContext> rpcContextThreadLocal = new ThreadLocal<RpcContext>(() =>
        {
            RpcContext context = new RpcContext();
            context.SetContextParameters(new ConcurrentDictionary<string, object>());
            return context;
        });
        
        public static RpcContext GetContext()
        {
            return rpcContextThreadLocal.Value;
        }

        public static void RemoveContext()
        {
            rpcContextThreadLocal.Dispose();
        }

        private RpcContext()
        {
        }
    }
}
