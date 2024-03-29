﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();

            var methodAttributes = type.GetMethod(method.Name)?.GetCustomAttributes<MethodInterceptionBaseAttribute>(true);

            if (methodAttributes != null)
            {
                classAttributes.AddRange(methodAttributes);
            }
            //classAttributes.Add(new PerformanceAspect(5)); Bütün metotlara PerformanceAspect ekle..

            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger))); Bütün metotlara ExceptionLogAspect ekle. Hata veren metodun logu alınacak.

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
