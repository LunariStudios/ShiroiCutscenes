using System;

namespace Shiroi.Cutscenes.Attributes {
    public class TokenCategoryAttribute : Attribute {
        public TokenCategoryAttribute(string category) {
            this.Category = category;
        }

        public string Category {
            get;
        }
    }
}