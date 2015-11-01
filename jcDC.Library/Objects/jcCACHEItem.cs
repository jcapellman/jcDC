using System;

namespace jcDC.Library.Objects {
    public class jcCACHEItem {
        public Type ItemType { get; set; }

        public dynamic ItemValue { get; set; }

        public jcCACHEItem() { }

        public jcCACHEItem(dynamic value) {
            ItemValue = value;

            ItemType = value.GetType();
        }
    }
}