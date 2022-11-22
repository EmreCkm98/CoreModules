using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Module_9_Caching.Models;

namespace Module_9_Caching.Controllers
{
    //cache işlemi için inmemorycache ve distrubuted olmak üzere 2 yöntem vardır.bu örnekte inmemory-cache özelliği kullanılmıştır.

    //inmemory-cache ozelliğini ekledikten sonra performansı test etmek için Postman kullanarak bu controllerdaki get metoduna get isteği yollayalım.API'mize ilk kez bir istek gönderdiğimizde postmande sağ tarafta time kısmından response süresini görebiliriz.aynı adrese bir kere daha istek atınca data cache'e alındığı için bu response süresi kısalmış olur.

    [Route("api/[controller]")]
    [ApiController]
    public class CachingController : ControllerBase
    {
        //Core projelerinde in-memory cache kullanmamızı sağlayan arayüzün adı IMemoryCache. Bu interface'e ait metotları vs. kullanarak cache set,get,remove gibi işlemleri yapabiliriz.
        private readonly IMemoryCache _memoryCache;
        private readonly ICarService _carService;
        public CachingController(IMemoryCache memoryCache, ICarService carService)
        {
            _memoryCache = memoryCache;
            _carService = carService;
        }
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            const string cacheKey = "carListKey";//cahe işlemi için key tanımladık.
            //Gelen ilk request için cache'de o key'e ait bir obje olmadığından ilk response source'a yani bu örnekte carservisimize gidip(bir repository yada service layer olabilir) dönen değer alınıp 30 dkka expire süresi set edilerek oluşturacaktır. Artık ondan sonraki bütün request'ler 30 dkka süresince source'a gitmeden response'u cache'de bulup Get işlemi yapıp return edecektir. Expire süresi dolduğunda ise ilgili key ve obje cache'den silinecektir.
            if (!_memoryCache.TryGetValue(cacheKey, out List<Car> response))
            {
                response = _carService.GetAll();//araba listemizi servisimizden alıyoruz.

                //cache için belirli ayarlarımızı giriyoruz.
                var cacheExpirationOptions =
                    new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(30),//cache expire süresini ayarlıyoruz.
                        //memory şiştiğinde cache objelerini hangi priority'de silecek bunu belirliyoruz
                        Priority = CacheItemPriority.Normal,
                        //Önbelleğin herhangi biri tarafından kullanılmaması durumunda süresinin dolacağı belirli bir zaman aralığını veriyoruz. SlidingExpirationı 10 dakikaya ayarladığımız için önbellek girişinden sonra 10 dakika boyunca müşteri talebi olmazsa önbelleğin süresi dolacak demektir.
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };
                //Set metodu parametre olarak 1:key, 2:value, 3:cacheOptions . Cache options yukarıda tanımladığımız cacheExpirationOptions option değerini vereceğiz.
                _memoryCache.Set(cacheKey, response, cacheExpirationOptions);
                //Set, Get yapabildiğimiz gibi Remove işlemide yapabiliriz. Bunun için cacheKey değerini parametre olarak Remove metoduna verip call yapmak yeterli.
                //_memoryCache.Remove(cacheKey);
            }
            return response;
        }
    }
}
