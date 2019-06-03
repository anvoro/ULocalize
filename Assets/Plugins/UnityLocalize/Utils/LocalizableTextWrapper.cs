using UnityEngine;
using UnityEngine.UI;

namespace UnityLocalize.Utils
{
    [Localizable]
    public class LocalizableTextWrapper : MonoBehaviour
    {
        [SerializeField]
        private LocalizableString _localizableString;

        public LocalizableString LocalizableString => this._localizableString;

        public Text Text => this.GetComponent<Text>();

        public void Start()
        {
            this.Text.text = this._localizableString.GetString();
        }
    }
}
