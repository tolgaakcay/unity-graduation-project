using System.Collections;
using UnityEngine;
using TMPro;

public class DropdownMenuManager : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public Camera[] cameras;

    private int _currentCameraIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentCameraIndex = 0;
        cameras[_currentCameraIndex].gameObject.SetActive(true);
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });
    }

    void OnDropdownValueChanged()
    {
        int _newCameraIndex = dropdown.value;

        if (_newCameraIndex == _currentCameraIndex)
            return;
        

        cameras[_currentCameraIndex].gameObject.SetActive(false);
        cameras[_newCameraIndex].gameObject.SetActive(true);
        _currentCameraIndex = _newCameraIndex;
    }
}
