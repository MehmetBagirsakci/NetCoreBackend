using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;//Bu olmazsa GetService<> hata veriyor.
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspects.Autofac
{
    //HttpContext: Web sayfasına bir istek yaptığınızda (bu isteklerinizin içerisine JWT'ye de koyacaksınız),
    //her bir isteğiniz için bir tane size özel HttpContext oluşur. Bir thread oluşur aslında
    //IHttpContexAccessor ile siz bu HttpContext'e erişirsiniz.

    //Aspect içerisinde IHttpContextAccessor'ı enjekte edemiyoruz. WebApi -> Business'a bağlı. Business -> DataAccess'e bağlı. Aspect bu zincirin içinde değil.
    //Onun için ServiceTool'u yazdık. ServiceTool BİZİM IOC ALT YAPIMIZI OKUMAYA YARIYOR.
    //Ben IHttpContextAccessor'ı istiyorum deyince ServiceTool IHttpContextAccessor'ın karşılığını bana vermiş olacak.

    //ServiceTool sayesinde ConstructorInjection yapamadığımız bir nesnenin, mesela IHttpContextAccessor'ın karşılığını elde ediyoruz.
    //Yine ConstructorInjection'a ihtiyaç duymadığımız yerde ServiceTool'u kullanabiliriz.

    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); //IoC containerde IHttpContextAccessor'ın karşılığı varsa getir.
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
