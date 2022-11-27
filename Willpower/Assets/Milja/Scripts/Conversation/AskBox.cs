using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AskBox : MonoBehaviour
{

    [Header("Visible features")]
    [SerializeField] float qTextSize = 15;


    [SerializeField] List<GameObject> questionButtons; //list of buttons

    [SerializeField] GameObject counter;

    #region private variables 
    //Question lists
    List<(string, string)> questions = new List<(string, string)>(); //empty question list.
    List<bool> isAsked = new List<bool>();

    List<(string, string)> questionsTaenia = new List<(string, string)>();
    List<(string,string)> questionsWillow = new List<(string, string)>();
    List<(string, string)> questionsMortti = new List<(string, string)>();
    List<(string, string)> questionsFather = new List<(string, string)>();
    List<(string, string)> questionsDoge = new List<(string, string)>();

    List<bool> isAskedTaenia = new List<bool>();
    List<bool> isAskedWillow = new List<bool>();
    List<bool> isAskedMortti = new List<bool>();
    List<bool> isAskedFather = new List<bool>();
    List<bool> isAskedDoge = new List<bool>();

    int qAmount; //how many buttons we have
    #endregion private variables

    private void OnEnable()
    {
        counter = GameObject.Find("TextCounter");

        qAmount = questionButtons.Count;

        //Initialize questions if you haven't done that before.
        if(questionsTaenia.Count < 1)
        {
            initializeQuestions();
        }

        switch (GameManagerMK.charNow)
        {
            case GameManagerMK.Character.Taenia:
                ChangeQuestions(questionsTaenia, isAskedTaenia);
                break;
            case GameManagerMK.Character.Willow:
                ChangeQuestions(questionsWillow, isAskedWillow);
                break;
            case GameManagerMK.Character.Mortti:
                ChangeQuestions(questionsMortti, isAskedMortti);
                break;
            case GameManagerMK.Character.Father:
                ChangeQuestions(questionsFather, isAskedFather);
                break;
            case GameManagerMK.Character.Doge:
                ChangeQuestions(questionsDoge, isAskedDoge);
                break;
            default:
                Debug.Log("There is no character like that.");
                break;
        }
    }


    void ChangeQuestions(List<(string, string)> que, List<bool> isask)
    {
        //Change questions according to the character.
        Debug.Log(que.Count);

        questions.Clear();
        isAsked.Clear();

        foreach ((string ,string) i in que)
        {
            questions.Add(i);
        }

        foreach(bool i in isask)
        {
            isAsked.Add(i);
        }

        qAmount = questions.Count;

        for (int i = 0; i < questionButtons.Count; i++)
        {

            //Edit buttons.
            TextMeshProUGUI textbox = questionButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            //Print question
            if (qAmount > i)
            {
                questionButtons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                textbox.text = questions[i].Item1;
                textbox.fontSize = qTextSize;

                if (isAsked[i]) //If question has already been asked change the colour.
                {
                    questionButtons[i].GetComponent<Image>().color = new Color32(160, 160, 160, 255);
                }
            }
            else
            {
                questionButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; //Make question text zero.
                questionButtons[i].GetComponent<Image>().color = new Color32(160, 160, 160, 255);
            }

        } //Change buttons manually. Also change button settings if needed.
    }

    public void Ask(int button)
    {
        if (button < qAmount)
        {
            if (questions[button].Item1 != "") //print the answer and take one question out.
            {
                //Edit buttons.
                questionButtons[button].GetComponent<Image>().color = new Color32(160, 160, 160, 255); //change colour.
                GetIsListForChar()[button] = true;

                //Edit answer box.
                this.GetComponent<AnswerBox>().EditAnswer(questions[button].Item2);

                //Edit amount of questions.
                GameManagerMK.qLeft--;
                counter.GetComponent<TextMeshProUGUI>().text = GameManagerMK.qLeft + " QUESTIONS LEFT";
            }
            else
            {
                Debug.Log("Button is empty");
            }
        }
        else
        {
            Debug.Log("There is no button " + button);
        }
    } //Ask a question.

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
        questionsMortti.Add(("What would you like to inherit?", "If I could make a wish... Maybe your house? We are soon losing our current house so we would really need a new one."));
        questionsMortti.Add(("What do you think about my family?", "I have only met your father. What a man. True hero. I always wanted to grow up like him."));
        questionsMortti.Add(("How are your life?", "I'm still searching for my calling. Maybe I'll become a musician one day. Or a professional golfer."));
        questionsMortti.Add(("How did we become friends?", "Was it during our class trip to the zoo? We had to stay at school since I was allergic to all the animals and your father didn't want you to have any fun."));
        questionsMortti.Add(("What is your family like?","I'm the happiest dad in the world. My wife is an appreciated doctor and my sons are such a gentlemen. I make sure that there is always time for my family!"));
        questionsMortti.Add(("What do you think about my company?", "Your company? Isn't it a IT company? I know something about computers but not that much."));
        questionsMortti.Add(("Are you sad about my death?", "Of course I am. I am your best buddy."));

        //Father questions
        questionsFather.Add(("What are you doing here?", "Just give me your company, you know your good-for-nothing daughter or devious wife deserve nothing."));
        questionsFather.Add(("Don't you have anything else to do?", "I would love to be in my cottage reading books and listening to birds, so yes I have. But someone has to be here knocking some sense into you."));
        questionsFather.Add(("Do you remember the day I was born?", "I always wanted to have a 3D-child, but no. I got a two dimensional wimp."));
        questionsFather.Add(("What do you think about my wife?", "I have always tried to tell you that witch is poisoning you. Well, now it's too late."));
        questionsFather.Add(("What do you think about Willow?", "That brat is nowhere as ugly as you were as a child, but I hope she never tries to touch me again."));
        questionsFather.Add(("Do you remember Mortti?", "Mortti was like a son to me, I wish he had visited us more often. But oh, that boy was bad with money. He tended to spend his weekly allowance on the first scam he encountered."));
        questionsFather.Add(("Are you sad about my death?", "Kind of. I can't really keep writing my weekly column \"Son I Never Wanted\" if you are dead."));

        //Doge questions
        questionsDoge.Add(("Who is a good boi?", "Woof!"));
        questionsDoge.Add(("What do you think about Taenia?", "Growl..."));
        questionsDoge.Add(("What do you think about Willow?", "I am very fond of the young lady Willow."));
        questionsDoge.Add(("What do you think about Mortti?", "Arf?"));
        questionsDoge.Add(("What do you think about Father?", "Woof woof woof!"));

        for (int i = 0; i < questionButtons.Count; i++)
        {
            isAskedTaenia.Add(false);
            isAskedWillow.Add(false);
            isAskedFather.Add(false);
            isAskedMortti.Add(false);
            isAskedDoge.Add(false);
        }
        
    }


    private List<bool> GetIsListForChar()
    {
        switch (GameManagerMK.charNow)
        {
            case GameManagerMK.Character.Taenia:
                return isAskedTaenia;
            case GameManagerMK.Character.Willow:
                return isAskedWillow;
            case GameManagerMK.Character.Mortti:
                return isAskedMortti;
            case GameManagerMK.Character.Father:
                return isAskedFather;
            case GameManagerMK.Character.Doge:
                return isAskedDoge;
            default:
                Debug.Log("There is no character like that.");
                break;
        }
        return null;
    }
}
