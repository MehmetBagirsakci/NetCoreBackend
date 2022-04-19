using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    //ClaimPrincipal:Bir kişi sisteme erişirken JWT ile erişir. Kişinin claimleri JWT içerisinde yer alır.
    //ClaimPrincipal ile JWT içindeki kişiye ait claimleri okuruz.
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }

        //JWT içerisinde kişinin Id bilgiside var. Onu okumak için bu metodu yazdım.
        //Loglama yaparken ProductAdd metodunu örneğin Id si 5 olan kişi çalıştırdı diyebilmek için.
        public static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.NameIdentifier).FirstOrDefault();
        }
    }
}


//var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
//?: claimsPrincipal NULL ise, result'a NULL değerini ata.
//   EĞER claimsPrincipal NULL değil ise resulta ilgili işlemin sonucunu ata
