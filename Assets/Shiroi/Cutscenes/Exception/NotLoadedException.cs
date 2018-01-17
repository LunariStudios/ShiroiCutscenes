namespace Shiroi.Cutscenes.Exception {
    public class NotLoadedException : System.Exception {
        public NotLoadedException(string propertyName) : base("Property '" + propertyName + "' is not yet loaded!") { }
    }
}