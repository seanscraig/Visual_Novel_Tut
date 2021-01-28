using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI nameTag;
    [SerializeField] [Range(0f, 1f)] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;

    DialogueContainer currentDialogue;

    float totalTimeToType, currentTime;
    string lineToShow;
    int index;

    [SerializeField] DialogueContainer debugDialogueContainer;

    // Start is called before the first frame update
    void Start()
    {
        if (debugDialogueContainer != null)
        {
            InitiateDialogue(debugDialogueContainer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }

        TypeOutText();
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        CycleLine();
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f)
        {
            return;
        }

        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0f, 1f);

        UpdateText();
    }

    private void UpdateText()
    {
        int totalLetterToShow = (int)(lineToShow.Length * visibleTextPercent);
        text.text = lineToShow.Substring(0, totalLetterToShow);
    }

    private void CycleLine()
    {
        if (index >= currentDialogue.lines.Count)
        {
            Debug.Log("There is no more lines to show");
            return;
        }

        lineToShow = currentDialogue.lines[index].line;

        if (currentDialogue.lines[index].actor != null)
        {
            nameTag.text = currentDialogue.lines[index].actor.Name;
        }

        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;

        text.text = "";

        index += 1;
    }

    public void InitiateDialogue(DialogueContainer dialogueContainer)
    {
        currentDialogue = dialogueContainer;
        index = 0;
        CycleLine();
    }
}
