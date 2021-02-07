using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] DialogueContainer debugDialogueContainer;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI nameTag;
    [SerializeField] [Range(0f, 1f)] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    [SerializeField] float skipTextWaitTime = 0.1f;
    [SerializeField] SpriteManager spriteManager;
    [SerializeField] SpriteManager backgroundManager;

    DialogueContainer currentDialogue;
    Coroutine skipTextCoroutine;

    float totalTimeToType, currentTime;
    string lineToShow;
    int index;

    

    // Start is called before the first frame update
    void Start()
    {
        if (debugDialogueContainer != null)
        {
            InitiateDialogue(debugDialogueContainer);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            skipTextCoroutine = StartCoroutine(SkipText());
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (skipTextCoroutine != null)
            {
                StopCoroutine(skipTextCoroutine);
            }
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

        DialogueLine line = currentDialogue.lines[index];

        if (line.spriteChanges != null)
        {
            for (int i = 0; i < line.spriteChanges.Count; i++)
            {
                if(line.spriteChanges[i].actor == null)
                {
                    spriteManager.Hide(line.spriteChanges[i].onScreenImageID);
                    continue;
                }
                int expressionID = line.spriteChanges[i].expression;
                spriteManager.Set(
                    line.spriteChanges[i].actor.sprites[expressionID],
                    line.spriteChanges[i].onScreenImageID
                    );
            }
        }

        // TODO refactor this part to not repeat code
        if (line.backgroundChanges != null)
        {
            for (int i = 0; i < line.backgroundChanges.Count; i++)
            {
                if (line.backgroundChanges[i].sprite == null)
                {
                    backgroundManager.Hide(line.backgroundChanges[i].onScreenImageID);
                    continue;
                }
                backgroundManager.Set(
                    line.backgroundChanges[i].sprite,
                    line.backgroundChanges[i].onScreenImageID
                    );
            }
        }

        lineToShow = line.line;

        if (line.actor != null)
        {
            nameTag.text = line.actor.Name;
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

    IEnumerator SkipText()
    {
        while (true)
        {
            yield return new WaitForSeconds(skipTextWaitTime);
            PushText();
        }
    }
}
