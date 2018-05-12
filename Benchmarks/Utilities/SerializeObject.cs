using System;
using System.Runtime.Serialization;

namespace Benchmarks.Utilities
{
    [DataContract]
    public class SerializeObject
    {
        [DataMember(Name = "foo")]
        public int Foo { get; set; }

        [DataMember(Name = "bar")]
        public string Bar { get; set; }

        [DataMember(Name = "baz")]
        public Baz Baz { get; set; }

        [DataMember(Name = "qux")]
        public DateTimeOffset Qux { get; set; }
    }

    public enum Baz
    {
        [EnumMember(Value = "1")]
        One = 1,
        [EnumMember(Value = "2")]
        Two = 2
    }
}
