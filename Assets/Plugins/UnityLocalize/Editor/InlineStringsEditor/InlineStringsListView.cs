using System.Collections.Generic;
using System.Linq;
using UnityLocalize.DataStorage;

namespace UnityLocalize.Editor
{
    internal class InlineStringsListView
    {
        private readonly List<InlineStringEditor> _inlineStringEditorsPool = new List<InlineStringEditor>();

        public IEnumerable<InlineStringEditor> InlineStringEditors
        {
            get { return this._inlineStringEditorsPool.Where(e => e.InlineString != null); }
        }

        public void AddInlineString(InlineString inlineString, InlineStringsPresenter inlineStringsPresenter)
        {
            InlineStringEditor freeEditor = this._inlineStringEditorsPool.FirstOrDefault(e => e.InlineString == null);

            if (freeEditor == null)
            {
                this._inlineStringEditorsPool.Add(new InlineStringEditor(inlineString, inlineStringsPresenter));
            }
            else
            {
                freeEditor.InlineString = inlineString;
            }
        }

        public void RemoveInlineString(StoredString inlineString)
        {
            InlineStringEditor editor = this._inlineStringEditorsPool.First(e => e.InlineString.Equals(inlineString));

            editor.InlineString = null;
        }
    }
}
