using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class FirstAidRequestDto
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
