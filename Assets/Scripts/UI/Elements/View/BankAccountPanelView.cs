using UnityEngine;
using TMPro;

namespace UI.View
{
    public sealed class BankAccountPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueLabel;

        public void SetValue(int value)
        {
            _valueLabel.text = $"{value}$";
        }
    }
}