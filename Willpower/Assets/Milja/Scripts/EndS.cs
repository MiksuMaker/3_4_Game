using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndS : MonoBehaviour
{

    [SerializeField] GameObject picBox;
    [SerializeField] GameObject textBox;

    Object[] backs;

    float waitTime = 5f;

    private void OnEnable()
    {
        backs = Resources.LoadAll("MiljaEnds",typeof(Sprite));
        StartCoroutine(GoThroughEnds());
    }

    IEnumerator GoThroughEnds()
    {
        TextMeshProUGUI textSays = textBox.GetComponent<TextMeshProUGUI>();
        Image pic = picBox.GetComponent<Image>();
        backs = Resources.LoadAll("MiljaEnds", typeof(Sprite));

        switch (WillScript.whoGetsHouse)
        {
            case "Taenia":
                textSays.text = "Taenia didn't care about the house at all, but at least she had a place to live till she found herself a new husband.";
                pic.sprite = FindSpriteWithName("1-1");
                break;
            case "Willow":
                textSays.text = "Willow simply wasn't mature enough to take care of the house. Finally, all the chores left undone caused a major accident and destroyed the whole place.";
                pic.sprite = FindSpriteWithName("1-2");
                break;
            case "Mortti":
                textSays.text = "Mortti and his family (wife, three children) moved happily to the house and therefore avoided years of homelessness.";
                pic.sprite = FindSpriteWithName("1-3");
                break;
            case "Father":
                textSays.text = "Father didn't want to move out from his cozy cottage so he pretty much left the house as it was. Later, it became a well-known abandoned house where only the ghost of Will lived.";
                pic.sprite = FindSpriteWithName("1-4");
                break;
            case "Doge":
                textSays.text = "Doge invited every abandoned dog to live together at his fabulous home.";
                pic.sprite = FindSpriteWithName("1-5");
                break;
        }

        yield return new WaitForSeconds(waitTime);

        switch (WillScript.whoGetsMoney)
        {
            case "Taenia":
                textSays.text = "Taenia was actually a pretty good CEO of the company after years of manipulation and plotting. Simultaneously, she learnt a few things about responsibilities too.";
                pic.sprite = FindSpriteWithName("2-1");
                break;
            case "Willow":
                textSays.text = "Willow was unable to feel any kind of empathy towards people, so her new, unwanted power made her pretty much a monster.";
                pic.sprite = FindSpriteWithName("2-2");
                break;
            case "Mortti":
                textSays.text = "Mortti wanted to take it easy and changed many things inside the company. Eventually, the company made no profit after all the parties they had.";
                pic.sprite = FindSpriteWithName("2-3");
                break;
            case "Father":
                textSays.text = "In the end, Father was just a senile old man who didn't know anything about Will's company. It went bankrupt quickly and everyone got unemployed.";
                pic.sprite = FindSpriteWithName("2-4");
                break;
            case "Doge":
                textSays.text = "Doge became a loved and loyal boss for the company.";
                pic.sprite = FindSpriteWithName("2-5");
                break;
        }

        yield return new WaitForSeconds(waitTime);

        switch (WillScript.whoGetsCompany)
        {
            case "Taenia":
                textSays.text = "With all that money, Taenia developed a serious gambling problem. After spending it all, she got both a serious debt and depression.";
                pic.sprite = FindSpriteWithName("3-1");
                break;
            case "Willow":
                textSays.text = "Even though Willow was always the weirdest one, Will had taught her what is the most important thing in life: Education. After she turned 18, she used the money to enroll in the medical school.";
                pic.sprite = FindSpriteWithName("3-2");
                break;
            case "Mortti":
                textSays.text = "Mortti was never good with money. At all. He accidentally funded a small organization that later started a war in Denmark. But at least only in Denmark, right?";
                pic.sprite = FindSpriteWithName("3-3");
                break;
            case "Father":
                textSays.text = "Nobody told Father that he needed a bank account to get the money. He spent rest of his life waiting for a letter.";
                pic.sprite = FindSpriteWithName("3-4");
                break;
            case "Doge":
                textSays.text = "With all the money, Doge created a robotic version of his master Will to keep him company even after his death. Needless to say, Will the Robot was much more nicer than the original one.";
                pic.sprite = FindSpriteWithName("3-5");
                break;
        }

        yield return new WaitForSeconds(waitTime);

        switch (WillScript.whoGetsDog)
        {
            case "Taenia":
                textSays.text = "Only reason why the wife had endured the dog all these years was her husband. Poor dog…";
                pic.sprite = FindSpriteWithName("4-1");
                break;
            case "Willow":
                textSays.text = "Owning a dog wasn't a big deal to Willow, who already had a pet lizard, pet spider and three pet frogs.";
                pic.sprite = FindSpriteWithName("4-2");
                break;
            case "Mortti":
                textSays.text = "Mortti was deadly allergic to dogs. It's weird that Will didn't remember that, though.";
                pic.sprite = FindSpriteWithName("4-3");
                break;
            case "Father":
                textSays.text = "Even though Father hated children and women and everybody else, he actually loved dogs. Doge was really happy in Father's cozy cottage.";
                pic.sprite = FindSpriteWithName("4-4");
                break;
            case "Doge":
                textSays.text = "The dog was perfectly capable of taking care of himself. Now he was free, ready to fulfill all his dreams.";
                pic.sprite = FindSpriteWithName("4-5");
                break;
        }

        yield return new WaitForSeconds(waitTime);


    }

    Sprite FindSpriteWithName(string name)
    {
        foreach (Object i in backs)
        {
            if (i.name == name)
            {
                return (Sprite)i;
            }
        }
        Debug.Log("There is no sprite named " + name);
        return null;
    }
}
