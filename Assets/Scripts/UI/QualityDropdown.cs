using System.Linq;
using TMPro;
using UnityEngine;

public class QualityDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;

    private const string VAR_NAME = "quality";

    private void Start()
    {
        _dropdown.options = QualitySettings.names.Select(s => new TMP_Dropdown.OptionData(s)).ToList();
        var level = PlayerPrefs.GetInt(VAR_NAME, QualitySettings.GetQualityLevel());
        _dropdown.value = level;
        QualitySettings.SetQualityLevel(level);
        _dropdown.onValueChanged.AddListener(i =>
        {
            QualitySettings.SetQualityLevel(i);
            PlayerPrefs.SetInt(VAR_NAME, i);
        });
    }
}
