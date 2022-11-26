using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AskBox : MonoBehaviour
{

    [Header("Visible features")]
    [SerializeField] float qTextSize = 15;


    [SerializeField] List<GameObject> questionButtons; //list of buttons

    #region private variables 
    //Question lists
    List<(string, string)> questions = new List<(string, string)>(); //empty question list.

    List<(string, string)> questionsTaenia = new List<(string, string)>();
    List<(string,string)> questionsWillow = new List<(string, string)>();
    List<(string, string)> questionsMortti = new List<(string, string)>();
    List<(string, string)> questionsFather = new List<(string, string)>();
    List<(string, string)> questionsDoge = new List<(string, string)>();

    int qAmount; //how many buttons we have
    #endregion private variables

    private void OnEnable()
    {
        qAmount = questionButtons.Count;
        initializeQuestions();
        ChangeQuestions(GameManager.charNow);
    }


    void ChangeQuestions(GameManager.Character character)
    {
        //Check what questions are available for the character and update boxes.

        switch (character)
        {
            case GameManager.Character.Taenia:
                questions = questionsTaenia;
                break;
            case GameManager.Character.Willow:
                questions = questionsWillow;
                break;
            case GameManager.Character.Mortti:
                questions = questionsMortti;
                break;
            case GameManager.Character.Father:
                questions = questionsFather;
                break;
            case GameManager.Character.Doge:
                questions = questionsDoge;
                break;
            default:
                Debug.Log("There is no character like that.");
                break;
        } //Change questions according to the character.

        for (int i = 0; i < qAmount; i++)
        {
            //Edit buttons.
            TextMeshProUGUI textbox = questionButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (questions.Count > i)
            {
                textbox.text = questions[i].Item1;
                textbox.fontSize = qTextSize;
            }
            else
            {
                questionButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }

        } //Change buttons manually. Also change button settings if needed.
    }

    public void Ask(int button)
    {
        if (button < qAmount)
        {
            this.GetComponent<AnswerBox>().EditAnswer(questions[button].Item2);
        }
        else
        {
            Debug.Log("There is no button " + button);
        }
    }

    private void initializeQuestions()
    {

 
        //Taenia questions
        questionsTaenia.Add(("What would you like to inherit?", "It pains me to think about your will when you're about to die... But yea, I would like to have your fortune."));
        questionsTaenia.Add(("What are your future plans?", "I think I'll keep playing games. I have became pretty good with online poker, believe or not."));
        questionsTaenia.Add(("Do you remember how we met?", "It was one of these online dating apps. At first I wasn't that interested but decided to give you an another change after googling your name."));
        questionsTaenia.Add(("What do you think about my father?", "A senile old man. I always wished he'd get a phone and stop visiting our house every time he needed help with something ridiculous..."));
        questionsTaenia.Add(("What do you think about Willow?", "Willow? ...Oh yea, the child. She is a lost cause. If I weren't around, our house would be a complete mess."));
        questionsTaenia.Add(("Will you survive without me?", "I'm afraid not. I've never had a job. I will probably starve if you don't bequeath your money to me."));
        questionsTaenia.Add(("Are you sad about my death?", "Darling, you are the best husband I have had. And I have had many of those."));

        //Willow questions

        questionsWillow.Add(("What would you like to inherit?", "Inerit? I want more frogs."));
        questionsWillow.Add(("Do you have any plans for the future?", "That's easy! I want to be a brain surgeon for animals."));
        questionsWillow.Add(("Your favorite hobby?", "I like running around the forest and collecting little bugs. Usually, I take them home and pretend that they are my family."));
        questionsWillow.Add(("Do you like school?", "Not really, my classmates are not nice so I don't play with them. But I know it's important to study good."));
        questionsWillow.Add(("What do you think about grandpa?", "Grandpa hates children. Last christmas, he gave me a rock as a present. But I liked it."));
        questionsWillow.Add(("What do you think about ma?", "Taenia is always playing but never with me. I'm okay with that."));
        questionsWillow.Add(("Are you sad about my death?", "Taenia said she will buy me a new bicycle if I don't bother her with crying."));

        //Mortti questions
        questionsMortti.Add(("What would you like to inherit?", "If I could make a wish... Maybe your house? Our current house is way too small and poor."));
        questionsMortti.Add(("What do you think about my family?", "I have only met your father. What a man. True hero. I always wanted to grow up like him."));
        questionsMortti.Add(("How are your life?", "I'm still searching for my calling. Maybe I'll become a musician one day. Or a professional golfer."));
        questionsMortti.Add(("How did we become friends?", "Was it during our class trip to the zoo? We had to stay at school since I was allergic to all the animals and your father didn't want you to have any fun."));
        questionsMortti.Add(("What is your family like?","I'm the happiest dad in the world. My wife is an appreciated doctor and my sons are such a gentlemen. I make sure that there is always time for my family!"));
        questionsMortti.Add(("What do you think about my company?", "Your company? Isn't it a IT company? I know something about computers but not that much."));
        questionsMortti.Add(("Are you sad about my death?", "Of course I am. I am your best buddy."));

        questionsFather.Add(("What are you doing here?", "Just give me your company, you know your good-for-nothing daughter or devious wife deserve nothing."));
        questionsFather.Add(("Don't you have anything else to do?", "I would love to be in my cottage reading books and listening to birds, so yes. But someone has to be here knocking some sense into you."));
        questionsFather.Add(("Do you remember the day I was born?", "I always wanted to have a 3D-child, but no. I got a two dimensional wimp."));
        questionsFather.Add(("What do you think about my wife?", "I have always tried to tell you that witch is poisoning you. Well, now it's too late."));
        questionsFather.Add(("What do you think about Willow?", "That brat is nowhere as ugly as you were as a child, but I hope she never tries to touch me again."));
        questionsFather.Add(("Do you remember Mortti?", "Mortti was like a son I never had. I wish he had visited us more often. But oh, that boy was bad with money. When I gave him his weekly allowance he tend to spend it on the first scam he encountered."));
        questionsFather.Add(("Are you sad about my death?", "Kind of. I can't really keep writing my weekly column \"Son I Never Wanted\" if you are dead."));
        //Father questions
        questionsDoge.Add(("Who is a good boi?", "Woof!"));

        
    }
}
