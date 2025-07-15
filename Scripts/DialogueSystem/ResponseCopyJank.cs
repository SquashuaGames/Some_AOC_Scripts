using TMPro;
using UnityEngine;

public class ResponseCopyJank : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh;
    

    private void OnEnable()
    {
        gameObject.GetComponent<TMP_Text>().text = textMesh.text;
    }
}
