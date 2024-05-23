using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication.Core.Database
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MovieStatus
    {
        [EnumMember(Value = "В планах")]
        Planned,
        [EnumMember(Value = "Смотрим")]
        Watching,
        [EnumMember(Value = "На паузе")]
        Paused,
        [EnumMember(Value = "Закончили")]
        Done
    }

    public class WrappedEnum<T> where T: Enum
    {
        public T Unwrapped {get; set;}
        public WrappedEnum(T value)
        {
            Unwrapped = value;
        }
        public override string ToString()
        {
            return Unwrapped.ToEnumMember();
        }
    }

}
