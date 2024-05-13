using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Busan_Air_Condition.Model
{
    internal class AirSensor
    {
        public string Site { get; set; }    // 지역명

        public DateTime Controlnumber { get; set; } // 측정시간

        public double No2 { get; set; }     // 이산화질소

        public string No2Cai { get; set; }     // 이산화질수 지수

        public double O3 { get; set; }      // 오존

        public string O3Cai { get; set; }      // 오존 지수

        public double So2 { get; set; }     // 이황산가스

        public string So2Cai { get; set; }     // 이황산가스 지수

        public double Pm25 { get; set; }    // 초미세먼지

        public string Pm25Cai { get; set; }    // 초미세먼지 지수

        public double Pm10 { get; set; }    // 미세먼지
        
        public string Pm10Cai { get; set; }    // 미세먼지 지수

        public double Co { get; set; }      // 일산화탄소

        public string CoCai { get; set; }      // 일산화탄소 지수
    }
}
