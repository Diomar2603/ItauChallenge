using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ItauChallenge.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CriterioBuscaCliente
    {
        [EnumMember(Value = "Posicao")]
        Posicao,
        [EnumMember(Value = "Corretagem")]
        Corretagem
    }
}
