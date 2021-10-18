using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GraphicCardChecker
{
    public class Function : IDisposable
    {
        internal ServiceProvider _serviceProvider;
        internal bool _disposed;

        public Function()
        {
            _serviceProvider = LambdaServiceProvider.Build();
        }
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler()
        {
            var handlder = _serviceProvider.GetService<IHandler>();
            handlder.HandleAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing) 
            {
                _serviceProvider.Dispose();
                _serviceProvider = null;
            }

            _disposed = true;
        }

    }
}
