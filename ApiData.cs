using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebIP
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ApiData
    {
        [JsonProperty(PropertyName = "НОМЕР")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "СМЕНА")]
        public string Shift { get; set; }

        [JsonProperty(PropertyName = "ДАТА")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "НАРЯД")]
        public string Outfit { get; set; }

        [JsonProperty(PropertyName = "МЕСТО_РАБОТЫ")]
        public string Workplace { get; set; }

        [JsonProperty(PropertyName = "ОБОРУДОВАНИЕ")]
        public string Equip { get; set; }

        [JsonProperty(PropertyName = "БОРТ_НОМЕР")]
        public string SideNumber { get; set; }

        [JsonProperty(PropertyName = "ПЛАН")]
        public string Plan { get; set; }

        [JsonProperty(PropertyName = "ФАКТ")]
        public string Fact { get; set; }

        [JsonProperty(PropertyName = "ЕД_ИЗМ")]
        public string Unit { get; set; }

        [JsonProperty(PropertyName = "КАТ_РАБОТ")]
        public string JobCategory { get; set; }

        [JsonProperty(PropertyName = "ФИО_РАБОЧИХ")]
        public string NameWorkers { get; set; }

        [JsonProperty(PropertyName = "ДОЛЖНОСТИ")]
        public string Posts { get; set; }

        [JsonProperty(PropertyName = "МЕРЫ_БЕЗОПАС")]
        public string SafetyMeasures{ get; set; }

        [JsonProperty(PropertyName = "СТАТУС")]
        public string Status { get; set; }
    }
}
