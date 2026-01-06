using TMPro;
using UnityEngine;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    [Header("대화 말풍선")]
    public GameObject talkPanel;
    [Header("대화 텍스트( | 넣으면 숨 고름)")]
    public TextMeshProUGUI talkText;

    [Header("텍스트 흔들림")]
    public float shakeAmount = 2f;      // 흔들림 세기
    public float shakeSpeed = 15f;      // 흔들림 속도

    [TextArea]
    public string dialogText;

    public float typingSpeed = 0.05f;
    bool isTyping;

    [HideInInspector]
    public bool isAction;

    Coroutine typingCoroutine;

    public void Action()
    {
        isAction = !isAction;

        if (isAction)
        {
            talkPanel.SetActive(true);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText());
        }
        else
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            talkPanel.SetActive(false);
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        talkText.text = "";

        int visibleCharCount = 0;

        StartCoroutine(ShakeText());

        foreach (char c in dialogText)
        {
            // | 하나당 즉시 0.1초 대기
            if (c == '|')
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            talkText.text += c;
            visibleCharCount++;

            if (visibleCharCount % 1 == 0)
            {
                GameManager.Instance.audioManager.TextTypingSound(1f);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // 흔들리는 텍스트
    IEnumerator ShakeText()
    {
        while (isAction)
        {
            talkText.ForceMeshUpdate();
            TMP_TextInfo textInfo = talkText.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int meshIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

                float offsetY = Mathf.Sin(Time.time * shakeSpeed + i) * shakeAmount;

                Vector3 offset = new Vector3(0, offsetY, 0);

                vertices[vertexIndex + 0] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                talkText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

}
