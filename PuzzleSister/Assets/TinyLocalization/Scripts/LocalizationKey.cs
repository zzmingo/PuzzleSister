namespace TinyLocalization {
    [System.Serializable]
    public class LocalizationKey {
        public string key = "";
        public string textValue = "";
        
        public LocalizationKey(string key, string textValue) {
            this.key = key;
            this.textValue = textValue;
        }
    }
}