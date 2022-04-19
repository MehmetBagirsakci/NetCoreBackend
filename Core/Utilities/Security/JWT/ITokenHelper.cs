using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
//User, OperationClaim ve UserOperationClaim classlarının Core katmanında yazılma sebebi tam olarak burası
//Biz oluşturduğumuz tokenda kullanıcının bilgilerini ve claimlerini tutmak istiyoruz.
//Eğer User ve OperationClaim classları Entities katmanında olursa, Core katmanı Entities katmanına bağımlı olur.
//Biz Core katmanı hiçbiryere bağımlı olmasın istiyoruz. Bağımlı olursa Core olmaz.