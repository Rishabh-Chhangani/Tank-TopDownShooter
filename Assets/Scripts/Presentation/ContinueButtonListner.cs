using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButtonListner : MonoBehaviour
{
    [SerializeField] private SaveSystem saveSystem;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        // Until SaveSystem tells us whether a save exists,
        // don't allow interaction.
        btn.interactable = false;
    }

    private void OnEnable()
    {
        if (saveSystem != null)
        {
            saveSystem.OnDataLoadedResult += HandleDataLoadedResult;
        }
    }

    private void OnDisable()
    {
        if (saveSystem != null)
        {
            saveSystem.OnDataLoadedResult -= HandleDataLoadedResult;
        }
    }

    private void HandleDataLoadedResult(bool hasSaveData)
    {
        btn.interactable = hasSaveData;
    }
}