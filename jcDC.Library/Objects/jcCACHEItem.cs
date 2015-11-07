using System;
using System.Runtime.Serialization;

namespace jcDC.Library.Objects {
    [DataContract]
    public class jcCACHEItem {
        [DataMember]
        public Type ItemType { get; set; }

        [DataMember]
        public dynamic ItemValue { get; set; }

        [DataMember]
        public DateTime Expiration { get; set; }

        public jcCACHEItem() { }

        public jcCACHEItem(dynamic value) {
            ItemValue = value;

            ItemType = value.GetType();
        }
    }
}