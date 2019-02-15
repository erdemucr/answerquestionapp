using DotNetify;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AqApplication.Service.Hubs
{
    public class LiveGaugeVM : BaseVM
    {
        /*
         * Her 1sn'yede bir timer nesnesinin çalışması için 1000ms değerini burada belirliyoruz ve bu field'ı LiveGaugeVM contructor'ında yer alan timer nesnesi constructor'ında kullanıyoruz.
         */
        private const int _timeInterval = 1000;

        private static List<Client> clients = new List<Client>();

  
        public DateTime ServerTime => DateTime.Now;

        /*
         * React-native tarafındaki gauge chart'ımızı beslemek için fake olarak daha üreteceğimiz timer nesnemiz.
         */
        private Timer _timer;

        /*
         * Random olarak üretilen değerin hafızadan tutulacağı property.
         */
        public int Value { get; set; }

        public LiveGaugeVM()
        {
            // Random olarak değer üreteceğimiz nesne.
            var random = new Random();
            _timer = new Timer(state =>
            {
                // 1-100 arası bir değer üretimi sağlanıyor Value adında react-native tarafında LiveGaugeVM'i kullandığımız ekranda state nesnesi içerisinde yer alan Value adındaki özelliğe buradaki değerin set edilmesi için tetiklenme sağlanıyor.
                Value = random.Next(1, 100);
                this.Changed(nameof(Value));
                // PushUpdates ile yapılan değişikliklerin bu LiveGaugeVM'i kullanan aktif ilgili ekranlara yansıması sağlanıyor.
                PushUpdates();
            }, null, _timeInterval, _timeInterval);
        }
        public override void Dispose()
        {
            _timer.Dispose();
            base.Dispose();
        }
    }
    public class Client
    {
        public string UserId { get; set; }
    }
}
